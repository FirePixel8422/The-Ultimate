using Fire_Pixel.Networking;
using Unity.Netcode;
using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private CombatContext combatContext;


    private void Awake()
    {
        Instance = this;

        PlayerStats[] playerStats = new PlayerStats[GlobalGameData.MAX_PLAYERS];
        for (int i = 0; i < GlobalGameData.MAX_PLAYERS; i++)
        {
            playerStats[i] = GameRules.DefaultPlayerStats.GetStatsCopy();
        }

        combatContext = new CombatContext(playerStats);

        TurnManager.TurnStarted += StartAttackingPhase;
    }
    private void Start()
    {
        WeaponManager.SwapToWeapon(0);
    }

    public void StartAttackingPhase()
    {

    }


    [ServerRpc(RequireOwnership = false, Delivery = RpcDelivery.Reliable)]
    public void AttackTargetPlayer_ServerRPC(int targetClientGameId)
    {
        StartDefensePhase_ClientRPC(GameIdRPCTargets.SendToTargetClient(targetClientGameId));
    }

    [ClientRpc(InvokePermission = RpcInvokePermission.Server, Delivery = RpcDelivery.Reliable)]
    public void StartDefensePhase_ClientRPC(GameIdRPCTargets rpcTargets)
    {
        if (rpcTargets.IsTarget == false) return;
    }

    private void OnDestroy()
    {
        TurnManager.TurnStarted -= StartAttackingPhase;
    }


    [SerializeField] private WeaponSO weapon;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkillManager.GlobalSkillList.SelectRandom().Resolve(combatContext, DefenseResult.None);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SkillUIHandler.UpdateSkillUI(weapon.GetAsSkillSet());
        }
    }
}
