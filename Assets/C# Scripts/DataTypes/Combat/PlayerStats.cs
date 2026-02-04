using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;


[System.Serializable]
public class PlayerStats
{
    public float Health;
    public float Energy;


    private List<StatusEffectInstance> effectsList = new List<StatusEffectInstance>();


    #region Status Effects

    /// <summary>
    /// Add status effect of type <paramref name="statusEffect"/> to player.
    /// </summary>
    public void AddStatusEffect(StatusEffectInstance statusEffect)
    {
        BaseStatusEffectRules statusApplyRules = GameRules.GetStatusApplyOptions(statusEffect.Type);

        switch (statusApplyRules.StackMode)
        {
            case StatusEffectStackMode.Independent:
                effectsList.Add(statusEffect);
                break;

            case StatusEffectStackMode.RefreshDuration:
                if (TryGetEffect(statusEffect.Type, out StatusEffectInstance existingEffect, out int effectId))
                {
                    int newDuration = math.max(existingEffect.Duration, statusEffect.Duration);
                    effectsList.ModifyAt(effectId, effect => effect.Duration = newDuration);
                }
                break;

            case StatusEffectStackMode.CombineDuration:
                if (TryGetEffect(statusEffect.Type, out existingEffect, out effectId))
                {
                    int newDuration = existingEffect.Duration += statusEffect.Duration;
                    effectsList.ModifyAt(effectId, effect => effect.Duration = newDuration);
                }
                break;

            case StatusEffectStackMode.Skip:
                if (HasEffect(statusEffect.Type))
                {
                    effectsList.Add(statusEffect);
                }
                break;

            default: break;
        }
    }

    /// <summary>
    /// Tick down the duration off all current status effect on player and apply the damage of the bleeding and burning status effect
    /// </summary>
    public void ApplyAndTickDownStatusEffects()
    {
        int effectCount = effectsList.Count;
        for (int i = 0; i < effectCount; i++)
        {
            int newDuration = effectsList[i].Duration;
            newDuration -= 1;

            if (newDuration <= 0)
            {
                effectsList.RemoveAtSwapBack(i);
                i--;
                continue;
            }

            effectsList.ModifyAt(i, effect => effect.Duration = newDuration);
        }

        float fireDamage = CalculateEffectStrength(StatusEffectType.Burning, GameRules.StatusEffects.Burning.StrengthRules);
        float bleedDamage = CalculateEffectStrength(StatusEffectType.Bleeding, GameRules.StatusEffects.Bleeding.StrengthRules);

        // Player takes damage
        // fireDamage + bleedDamage
    }


    /// <returns>Wheather player has <paramref name="effectType"/> status effect, that corresponding <see cref="StatusEffectInstance"/> and the effectId in the effectList</returns>
    public bool TryGetEffect(StatusEffectType effectType, out StatusEffectInstance effect, out int effectId)
    {
        int effectCount = effectsList.Count;
        for (int i = 0; i < effectCount; i++)
        {
            if (effectsList[i].Type == effectType)
            {
                effect = effectsList[i];
                effectId = i;
                return true;
            }
        }
        effect = default;
        effectId = -1;
        return false;
    }
    /// <returns>Wheather player has <paramref name="effectType"/> status effect
    public bool HasEffect(StatusEffectType effectType)
    {
        int effectCount = effectsList.Count;
        for (int i = 0; i < effectCount; i++)
        {
            if (effectsList[i].Type == effectType)
            {
                return true;
            }
        }
        return false;
    }

    /// <returns>Does player have the <see cref="StatusEffectType.Broken"/> effect</returns>
    public bool IsBroken => HasEffect(StatusEffectType.Broken);


    /// <returns>Damage multiplier based on all active statusEffects that affect damage output</returns>
    public float GetDamageOutputMultiplier()
    {
        return 1 +
            CalculateEffectStrength(StatusEffectType.Empowered, GameRules.StatusEffects.Empowered.StrengthRules) -
            CalculateEffectStrength(StatusEffectType.Weakened, GameRules.StatusEffects.Weakened.StrengthRules);
    }

    /// <returns>Damage received multiplier based on all active statusEffects that affect damage received</returns>
    public float GetDamageReceivedMultiplier()
    {
        return 1 + CalculateEffectStrength(StatusEffectType.Vulnerable, GameRules.StatusEffects.Vulnerability.StrengthRules);
    }


    /// <returns>EffectStrength based on current stacks of that effect and <see cref="EffectStrengthStackRules"/></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float CalculateEffectStrength(StatusEffectType effectType, EffectStrengthStackRules strengthRules)
    {
        if (!GetEffectStackCount(effectType, out int stacks))
        {
            return 0;
        }

        float strength = 0;
        float strengthMultiplier = 1;

        // Every stack multiplies its power by decayMultiplier after the first stack
        for (int i = 0; i < stacks; i++)
        {
            strength += strengthRules.StrengthPerStack * strengthMultiplier;
            strengthMultiplier *= strengthRules.StackDecayMultiplier;
        }

        return strength;
    }

    /// <returns>Get total stacks of <paramref name="effectType"/> effect on player</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool GetEffectStackCount(StatusEffectType effectType, out int stacks)
    {
        int effectCount = effectsList.Count;
        stacks = 0;

        for (int i = 0; i < effectCount; i++)
        {
            if (effectsList[i].Type == effectType)
            {
                stacks += 1;
            }
        }
        return stacks > 0;
    }

    #endregion
}