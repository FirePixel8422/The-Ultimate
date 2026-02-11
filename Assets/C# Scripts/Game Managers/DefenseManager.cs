using System;
using Unity.Netcode;
using UnityEngine;


public static class DefenseManager
{
#pragma warning disable UDR0001
    private static DefenseWindowParameters defenseWindow;
    private static float attackImpactGlobalTime;

    private static DefenseResult defenseResult;
    private static int skillId;

    public static event Action<int, DefenseResult> AttackImpact;
#pragma warning restore UDR0001


    public static void StartAttackSequence(int incomingSkillId)
    {
        SkillBase skill = SkillManager.GlobalSkillList[incomingSkillId];

        defenseWindow = skill.DefenseWindows;
        attackImpactGlobalTime = Time.time + skill.AttackStartupTime;

        defenseResult = DefenseResult.None;
        skillId = incomingSkillId;
        AttackImpact = null;

        ExtensionMethods.Invoke(NetworkManager.Singleton, skill.AttackStartupTime, () =>
        {
            AttackImpact.Invoke(skillId, defenseResult);
        });
    }

    /// <returns>Whether the defense action succesfully defends the attack</returns>
    public static DefenseResult DoDefendAction(DefenseType defenseType)
    {
        float timeBeforeAttackImpact = attackImpactGlobalTime - Time.time;

        defenseResult = defenseType switch
        {
            DefenseType.Block => ResolveBlock(timeBeforeAttackImpact),
            DefenseType.Parry => ResolveParry(timeBeforeAttackImpact),
            _ => DefenseResult.None
        };

        return defenseResult;
    }
    private static DefenseResult ResolveBlock(float timeBeforeAttackImpact)
    {
        if (defenseWindow.Block > timeBeforeAttackImpact)
        {
            return DefenseResult.Blocked;
        }
        return DefenseResult.None;
    }
    private static DefenseResult ResolveParry(float timeBeforeAttackImpact)
    {
        if (defenseWindow.PerfectParry > timeBeforeAttackImpact)
        {
            return DefenseResult.PerfectParried;
        }
        if (defenseWindow.Parry > timeBeforeAttackImpact)
        {
            return DefenseResult.Parried;
        }
        return DefenseResult.None;
    }
}