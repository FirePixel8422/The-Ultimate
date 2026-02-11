using UnityEngine;


[System.Serializable]
public class SkillBase
{
    public int Id { get; private set; }
    public void SetId(int id)
    {
        Id = id;
    }

    [SerializeField] private SkillInfo info = SkillInfo.Default;
    [SerializeField] private SkillCosts costs = SkillCosts.Default;
    [SerializeField] private DefenseWindowParameters defenseWindows = new DefenseWindowParameters(0.5f, 0.25f, 0.1f);
    [SerializeField] private float attackStartupTime;

    [SerializeReference] public SkillBaseEffect[] effects;
    
    public SkillInfo Info => info;
    public SkillCosts Costs => costs;
    public DefenseWindowParameters DefenseWindows => defenseWindows;
    public float AttackStartupTime => attackStartupTime;




    public virtual void Resolve(CombatContext ctx, DefenseResult defenseResult)
    {
        DefenseAbsorptionParameters defenseAbsorptionParams = GameRules.GetDefenseAbsorptionParams(defenseResult);

        int effectCount = effects.Length;
        for (int i = 0; i < effectCount; i++)
        {
            effects[i].Resolve(ctx, defenseAbsorptionParams);
        }
    }
}

[System.Serializable]
public struct SkillInfo
{
    public string Name;
    [TextArea]
    public string Description;

    public static SkillInfo Default => new SkillInfo()
    {
        Name = "New Skill",
        Description = "You shouldve entered some skill info here..."
    };
}

[System.Serializable]
public struct SkillCosts
{
    public PlayerResourceType Type;
    public int Amount;

    public static SkillCosts Default => new SkillCosts()
    {
        Type = PlayerResourceType.Energy,
        Amount = 0,
    };
}
