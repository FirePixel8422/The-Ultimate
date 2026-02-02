using UnityEngine;


public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    public void StartAttackingPhase()
    {

    }
}
