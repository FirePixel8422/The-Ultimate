



[System.Serializable]
public class SkillEffectStatus : SkillEffectBase
{
    public StatusEffectInstance StatusEffect;

    protected override void Resolve(CombatContext ctx)
    {
        ctx.Defender.AddStatusEffect(StatusEffect);
    }
}