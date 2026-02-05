


/// <summary>
/// Base class for modular effects that run when a skill is used.
/// Skill effects operate on <see cref="CombatContext"/> to modify damage,
/// apply status effects, or execute additional combat logic.
/// </summary>
[System.Serializable]
public abstract class SkillBaseEffect
{
    public virtual void Resolve(CombatContext ctx, DefenseAbsorptionParameters absorptionParams) { }
}