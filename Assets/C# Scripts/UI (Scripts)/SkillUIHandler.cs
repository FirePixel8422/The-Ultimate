using UnityEngine;

public class SkillUIHandler : MonoBehaviour
{
#pragma warning disable UDR0001
    private static SkillUIBlock[] skillUIBlocks;
    private static TooltipHandler toolTipHandler;
#pragma warning restore UDR0001


    private void Awake()
    {
        skillUIBlocks = GetComponentsInChildren<SkillUIBlock>(true);
        toolTipHandler = GetComponent<TooltipHandler>();
    }

    public static void UpdateSkillUI(Weapon weapon)
    {
        int skillCount = weapon.Skills.Length;
        for (int i = 0; i < skillCount; i++)
        {
            skillUIBlocks[i].UpdateUI(weapon.Skills[i].Skill);
        }
        // Update tooltip systems
        toolTipHandler.UpdateColoredWords();
    }

    public static void UpdateSkillsActiveState(bool isActive)
    {
        int skillCount = skillUIBlocks.Length;
        for (int i = 0; i < skillCount; i++)
        {
            skillUIBlocks[i].UpdateSkillActiveState(isActive);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            UpdateSkillsActiveState(true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            UpdateSkillsActiveState(false);
        }
    }
}
