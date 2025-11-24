using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    public WeaponDataSO weaponData;
    public WeaponAnimation AnimCompo { get; private set; }
    [SerializeField] private SpriteRenderer weaponSprite;
    private Animator animator;
    private float reloadTime = 0.2f;
    private Transform _target;

    private void Awake()
    {
        AnimCompo = GetComponent<WeaponAnimation>();
        animator = GetComponent<Animator>();
        
        SetWeapon();
    }

    public void SetWeapon()
    {
        animator.runtimeAnimatorController = weaponData.animatorController;
        if (weaponData.sprite != null && weaponSprite != null)
            weaponSprite.sprite = weaponData.sprite;
    }

    public void Swing(Transform target)
    {
        AnimCompo.FireWeapon();
        _target = target;
    }

    public void Hit()
    {
        if (_target == null) return;
        _target.GetComponent<HealthSystem>().GetDamage(weaponData.damage);
    }
}
