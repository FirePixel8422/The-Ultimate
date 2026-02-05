using UnityEngine;



public class DataInitializer : MonoBehaviour
{
    [SerializeField] private StatusEffectSettingsSO statusEffectsRulesSO;
    [SerializeField] private DefenseSettingsSO defenseRulesSO;


    private void Awake()
    {
        GameRules.SetGameRules(statusEffectsRulesSO.StatusRules, defenseRulesSO.DefenseStrengthRules);
    }
}