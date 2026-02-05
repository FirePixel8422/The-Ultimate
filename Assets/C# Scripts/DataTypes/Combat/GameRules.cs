



public static class GameRules
{
#pragma warning disable UDR0001
    public static StatusEffectRules StatusEffects { get; private set; }
    public static DefenseRules DefenseStrengths { get; private set; }
#pragma warning restore UDR0001

    public static void SetGameRules(StatusEffectRules newStatusRules, DefenseRules newDefenseRules)
    {
        StatusEffects = newStatusRules;
        DefenseStrengths = newDefenseRules;
    }

    public static BaseStatusEffectRules GetStatusApplyOptions(StatusEffectType statusType)
    {
        return statusType switch
        {
            StatusEffectType.Burning => StatusEffects.Burning,
            StatusEffectType.Bleeding => StatusEffects.Bleeding,
            StatusEffectType.Broken => StatusEffects.Broken,
            StatusEffectType.Empowered => StatusEffects.Empowered,
            StatusEffectType.Weakened => StatusEffects.Weakened,
            StatusEffectType.Vulnerable => StatusEffects.Vulnerability,
            _ => null,
        };
    }
    public static DefenseAbsorptionParameters GetDefenseAbsorptionParams(DefenseResult defenseResult)
    {
        return defenseResult switch
        {
            DefenseResult.Blocked => DefenseStrengths.Block,
            DefenseResult.Parried => DefenseStrengths.Parry,
            DefenseResult.PerfectParried => DefenseStrengths.PerfectParry,
            DefenseResult.None or _ => new DefenseAbsorptionParameters(0, false),
        };
    }
}