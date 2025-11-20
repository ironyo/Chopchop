using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    public WeaponDataSO weaponData;
    public WeaponAnimation AnimCompo { get; private set; }
    private Animator animator;
    private float reloadTime = 0.2f;
    private Transform _target;

    private void Awake()
    {
        AnimCompo = GetComponent<WeaponAnimation>();
        animator = GetComponent<Animator>();
        
        animator.runtimeAnimatorController = weaponData.animatorController;
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
