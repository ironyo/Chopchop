using System;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private float cooldown;
    private WeaponHolder weaponHolder;

    public bool CanAttack => cooldown <= 0f;

    private void Awake()
    {
        weaponHolder = GameObject.Find("WeaponParent/Weapon").GetComponent<WeaponHolder>();
    }

    private void Update()
    {
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
    }
    
    public void TryAttack(Transform target, float atkSpeed)
    {
        if (!CanAttack || target == null) return;
        cooldown = atkSpeed;
    }
}
