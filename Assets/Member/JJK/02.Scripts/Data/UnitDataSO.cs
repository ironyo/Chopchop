using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "SO/UnitData")]
public class UnitDataSO : ScriptableObject
{
    public int attack = 10;
    public float attackSpeed = 1.5f;
    public float attackRange = 2f;
}