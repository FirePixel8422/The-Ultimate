using UnityEngine;



[CreateAssetMenu(fileName = "New SkillSO", menuName = "ScriptableObjects/SkillSO", order = -1000)]
public class BaseSkillSO : ScriptableObject
{
    [SerializeReference]
    public SkillBase Skill;
}