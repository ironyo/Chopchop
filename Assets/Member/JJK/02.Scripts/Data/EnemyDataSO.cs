using UnityEngine;

public enum TargetType {Minion, Building, HQ}

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "SO/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public TargetType targetType;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRange;
    public int damage;
}
