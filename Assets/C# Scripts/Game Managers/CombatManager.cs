using Fire_Pixel.Networking;
using Unity.Netcode;
using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    [SerializeField] private DefaultPlayerStatsSO defPlayerStats;
    [SerializeField] private CombatContext combatContext;

    private SyncedAction QuitGame = new SyncedAction();


    private void Awake()
    {
        Instance = this;

        PlayerStats[] playerStats = new PlayerStats[GlobalGameData.MAX_PLAYERS];
        for (int i = 0; i < GlobalGameData.MAX_PLAYERS; i++)
        {
            playerStats[i] = defPlayerStats.GetPlayerStatsCopy();
        }

        combatContext = new CombatContext(playerStats);

        TurnManager.TurnStarted += StartAttackingPhase;

        QuitGame.Create();
        QuitGame += () => Application.Quit();
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
            SkillManager.Instance.GlobalSkillList.SelectRandom().Resolve(combatContext, DefenseResult.None);

            QuitGame.Schedule_ServerRPC(3);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SkillUIHandler.UpdateSkillUI(weapon.Data);
        }
    }
}
