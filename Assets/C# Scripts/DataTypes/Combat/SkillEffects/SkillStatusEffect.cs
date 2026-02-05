using UnityEngine;


/// <summary>
/// SkillEffect that adds a <see cref="StatusEffectInstance"/> to the skill's attack
/// </summary>
[System.Serializable]
public class SkillStatusEffect : SkillBaseEffect
{
    [Header("Status effects applied to the defender")]
    public StatusEffectInstance ToApplyStatusEffect;

    public override void Resolve(CombatContext ctx, DefenseAbsorptionParameters absorptionParams)
    {
        ctx.Defender.AddStatusEffect(ToApplyStatusEffect);
    }
}