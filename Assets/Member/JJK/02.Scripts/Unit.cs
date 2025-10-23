using System;
using UnityEngine;

public enum UnitTeam {Attacker, Defender}

public class Unit : MonoBehaviour
{
    public UnitData data;
    public UnitTeam _team;
    public bool IsDead => currentHP <= 0;
    
    private int currentHP;
    private float attackCooldown;

    private void Start()
    {
        currentHP = data.hp;
        UnitManager.Instance.RegisterUnit(this);
    }

    private void Update()
    {
        if (IsDead) return;
        HandleAttackCooldown();
    }

    private void HandleAttackCooldown()
    {
        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;
        
        currentHP -= damage;

        if (currentHP <= 0)
            Die();
    }

    public void Attack(Unit target)
    {
        if (IsDead || target.IsDead || attackCooldown > 0) return;
        
        target.TakeDamage(data.attack);
        attackCooldown = data.attackSpeed;
    }

    private void Die()
    {
        UnitManager.Instance.UnregisterUnit(this);
        Destroy(gameObject);
    }
}
