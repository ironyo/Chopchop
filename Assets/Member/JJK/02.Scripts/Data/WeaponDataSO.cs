using UnityEngine;

public enum WeaponType {Sword, Pistol, SMG}

[CreateAssetMenu(fileName = "WeaponDataSO", menuName = "SO/WeaponDataSO")]
public class WeaponDataSO : ScriptableObject
{
    public WeaponType _WeaponType;
    public RuntimeAnimatorController animatorController;
    public GameObject bulletPrefab;
    public float attackRange;
    public int damage;
}
