using UnityEngine;

[CreateAssetMenu(fileName = "BuildingStat", menuName = "SO/BuildingStat")]
public class TowerStat : ScriptableObject
{
    public int damage;
    public int attackRange;
    public int attackSpeed;
}
