using UnityEngine;

public class SkillUIHandler : MonoBehaviour
{
    public static SkillUIHandler Instance { get; private set; }

    [SerializeField] private SkillUIBlock[] skillUIBlocks;


    private void Awake()
    {
        Instance = this;

        skillUIBlocks = GetComponentsInChildren<SkillUIBlock>(true);
    }

    public static void UpdateSkillUI(Weapon weapon)
    {
        int skillCount = weapon.Skills.Length;
        for (int i = 0; i < skillCount; i++)
        {
            Instance.skillUIBlocks[i].UpdateUI(weapon.Skills[i].Skill.Info);
        }
    }
}
