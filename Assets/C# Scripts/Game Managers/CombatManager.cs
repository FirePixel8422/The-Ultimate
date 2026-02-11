using Fire_Pixel.Networking;
using Unity.Netcode;
using UnityEngine;


public class CombatManager : NetworkBehaviour
{
    public static CombatManager Instance { get; private set; }

    [SerializeField] private CombatContext combatContext;
    private bool canDefend;


    private void Awake()
    {
        Instance = this;

        PlayerStats[] playerStats = new PlayerStats[GlobalGameData.MAX_PLAYERS];
        for (int i = 0; i < GlobalGameData.MAX_PLAYERS; i++)
        {
            playerStats[i] = GameRules.DefaultPlayerStats.GetStatsCopy();
        }

        combatContext = new CombatContext(playerStats);
    }
    private void Start()
    {
        WeaponManager.SwapToWeapon(0);
    }


    [ServerRpc(RequireOwnership = false, Delivery = RpcDelivery.Reliable)]
    public void Attack_ServerRPC(int skillId, int attackerClientGameId)
    {
        StartDefensePhase_ClientRPC(skillId, GameIdRPCTargets.SendToOppositeClient(attackerClientGameId));
    }
    [ClientRpc(RequireOwnership = false, Delivery = RpcDelivery.Reliable)]
    private void StartDefensePhase_ClientRPC(int skillId, GameIdRPCTargets rpcTargets)
    {
        if (rpcTargets.IsTarget == false) return;

        DefenseManager.StartAttackSequence(skillId);
        DefenseManager.AttackImpact += ResolveAttack;
    }


    #region Resolve Attack on defender and attacker

    private void ResolveAttack(int skillId, DefenseResult defenseResult)
    {
        DefenseManager.AttackImpact -= ResolveAttack;

        ResolveAttack_ServerRPC(skillId, defenseResult);
        ResolveAttack_Local(skillId, defenseResult);
    }
    [ServerRpc(RequireOwnership = false, Delivery = RpcDelivery.Reliable)]
    private void ResolveAttack_ServerRPC(int skillId, DefenseResult defenseResult, ServerRpcParams rpcParams = default)
    {
        int attackerClientGameId = ClientManager.GetClientGameId(rpcParams.Receive.SenderClientId);

        ResolveAttack_ClientRPC(skillId, defenseResult, GameIdRPCTargets.SendToOppositeClient(attackerClientGameId));
    }

    [ClientRpc(RequireOwnership = false, Delivery = RpcDelivery.Reliable)]
    private void ResolveAttack_ClientRPC(int skillId, DefenseResult defenseResult, GameIdRPCTargets rpcTargets)
    {
        if (rpcTargets.IsTarget == false) return;

        ResolveAttack_Local(skillId, defenseResult);
    }
    private void ResolveAttack_Local(int skillId, DefenseResult defenseResult)
    {
        SkillManager.GlobalSkillList[skillId].Resolve(combatContext, defenseResult);

        DebugLogger.LogError("Defender Health: " + combatContext.Defender.Health);
        DebugLogger.LogError("Defender Effect Count: " + combatContext.Defender.EffectsList.Count);

        TurnManager.Instance.EndTurn_ServerRPC();
    }

    #endregion


    public override void OnDestroy()
    {
        base.OnDestroy();
        DefenseManager.AttackImpact -= ResolveAttack;
    }
}
