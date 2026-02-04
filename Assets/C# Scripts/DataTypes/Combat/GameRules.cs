



public static class GameRules
{
#pragma warning disable UDR0001
    public static StatusEffectRules StatusEffects { get; private set; }
#pragma warning restore UDR0001
    public static void SetStatusEffectRules(StatusEffectRules newRules)
    {
        StatusEffects = newRules;
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
}