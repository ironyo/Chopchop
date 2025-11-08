using System;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    public WeaponAnimation AnimCompo { get; private set; }

    private void Awake()
    {
        AnimCompo = GetComponent<WeaponAnimation>();
    }

    public void ShootBullet()
    {
        SpawnBullet(firePos.position);
        AnimCompo.FireWeapon();
    }

    private void SpawnBullet(Vector2 position)
    {
        PoolManager.Instance.GetMinionFromPool("BulletPool", position);
    }
}
