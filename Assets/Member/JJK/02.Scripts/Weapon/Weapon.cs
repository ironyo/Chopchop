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

    private Unit _target;

    private void Awake()
    {
        AnimCompo = GetComponent<WeaponAnimation>();
        animator = GetComponent<Animator>();
        
        animator.runtimeAnimatorController = weaponData.animatorController;
    }

    public void ShotBullet()
    {
        AnimCompo.FireWeapon();
        SpawnBullet(firePos.position, weaponData.damage);
    }

    public IEnumerator TripleShot()
    {
        ShotBullet();
        yield return new WaitForSeconds(reloadTime);
        ShotBullet();
        yield return new WaitForSeconds(reloadTime);
        ShotBullet();
    }

    public void Swing(Unit target)
    {
        AnimCompo.FireWeapon();
        _target = target;
    }

    public void Hit()
    {
        if (_target == null) return;
        _target.GetComponent<HealthSystem>().GetDamage(weaponData.damage);
    }

    private void SpawnBullet(Vector2 position, int dmg)
    {
        //PoolManager.Instance.GetMinionFromPool("BulletPool", position);
        var bullet = Instantiate(weaponData.bulletPrefab, position, transform.rotation * Quaternion.Euler(0, 0, -90));
        bullet.GetComponent<Bullet>().Initialize(dmg);
    }
}
