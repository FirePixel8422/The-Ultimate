using TMPro;
using UnityEngine;


public class SkillUIBlock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;


    public void UpdateUI(SkillInfo skill)
    {
        title.text = skill.Name;
        description.text = skill.Description;
    }
}