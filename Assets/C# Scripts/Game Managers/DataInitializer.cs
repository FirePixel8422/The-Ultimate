using UnityEngine;



public class DataInitializer : MonoBehaviour
{
    [SerializeField] private StatusEffectSettingsSO statusEffectsRulesSO;


    private void Awake()
    {
        GameRules.SetStatusEffectRules(statusEffectsRulesSO.StatusRules);
    }
}