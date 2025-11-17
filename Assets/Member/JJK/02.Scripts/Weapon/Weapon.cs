using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    public WeaponDataSO weaponData;
    public WeaponAnimation AnimCompo { get; private set; }
    private Animator animator;
    private float reloadTime = 0.2f;

    private void Awake()
    {
        AnimCompo = GetComponent<WeaponAnimation>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        animator.runtimeAnimatorController = weaponData.animatorController;
    }

    public void ShotBullet()
    {
        AnimCompo.FireWeapon();
        SpawnBullet(firePos.position, weaponData.damage);
    }

    public IEnumerator TripleShot()
    {
        for (int i = 0; i < 3; i++)
        {
            ShotBullet();
            yield return new WaitForSeconds(reloadTime);
        }
    }

    public void Swing()
    {
        AnimCompo.FireWeapon();
    }

    private void SpawnBullet(Vector2 position, float dmg)
    {
        //PoolManager.Instance.GetMinionFromPool("BulletPool", position);
        var bullet = Instantiate(weaponData.bulletPrefab, position, transform.rotation * Quaternion.Euler(0, 0, -90));
        bullet.GetComponent<Bullet>().Initialize(dmg);
    }
}
