using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "SO/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("Base Data")]
    public GameObject prefab;
    public Transform spawnPoint;
    
    [Header("Stat")]
    public int hp = 100;
    public int attack = 10;
    public float attackSpeed = 1.5f;
}
