using UnityEngine;



[CreateAssetMenu(fileName = "New Player Stats", menuName = "ScriptableObjects/DefaultPlayerStatsSO", order = -1000)]
public class DefaultPlayerStatsSO : ScriptableObject
{
    [SerializeField] private float health;
    [SerializeField] private int energy;

    public PlayerStats GetStatsCopy() => new PlayerStats(health, energy);
}