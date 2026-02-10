using UnityEngine;



[CreateAssetMenu(fileName = "New Skill", menuName = "ScriptableObjects/SkillSO", order = -1000)]
public class SkillBaseSO : ScriptableObject
{
    [SerializeReference]
    public SkillBase Skill;
}