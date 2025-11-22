using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "SO/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject
{
    public RuntimeAnimatorController animatorController;
    public Sprite sprite;
    public float attackRange;
    public int damage;
}
