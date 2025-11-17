using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum UnitType {Player, Enemy, HQ}

public class Unit : MonoBehaviour
{
    public HealthSystem HealthCompo {get; private set;}
    public UnitDataSO data;
    public UnitType _unitType;

    [field: SerializeField] public UnityEvent<Vector3> OnTargetChanged { get; private set; }
    
    private NavMeshAgent navAgent;
    private float attackCooldown;
    private Unit target;
    private Weapon _weapon;

    private void Awake()
    {
        if (_unitType != UnitType.HQ)
            NavAgentSet();
        
        HealthCompo = GetComponent<HealthSystem>();
        HealthCompo.OnDead += Die;
        
        if (_unitType == UnitType.Player)
            _weapon = transform.Find("WeaponParent/Weapon").GetComponent<Weapon>();
    }

    private void NavAgentSet()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        navAgent.avoidancePriority = Random.Range(20, 80);
        navAgent.stoppingDistance = 0.5f;
        navAgent.autoBraking = false;
    }

    private void Update()
    {
        HandleAttackCooldown();

        if (_unitType != UnitType.HQ && InvasionManager.Instance.isLanding)
        {
            SetTarget();
            MoveToTarget();
        }
    }

    private void SetTarget()
    {
        if (_unitType == UnitType.Player)
        {
            target = EnemyManager.Instance.GetNearestEnemy(this);
        }
        else if (_unitType == UnitType.Enemy)
        {
            GameObject hqObj = GameObject.FindGameObjectWithTag("HQ");
            target = hqObj.GetComponent<Unit>();
        }
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            navAgent.SetDestination(target.transform.position);
            float distance = Vector3.Distance(transform.position, target.transform.position);

            float attackRange = (_unitType == UnitType.Player) ? _weapon.weaponData.attackRange : data.attackRange;
            
            if (distance < attackRange)
            {
                navAgent.ResetPath();
                Attack(target);
            }
            
            OnTargetChanged?.Invoke(target.transform.position);
        }
    }

    private void HandleAttackCooldown()
    {
        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;
    }

    public void Attack(Unit _target)
    {
        if (_target == null || attackCooldown > 0) return;
        
        attackCooldown = data.attackSpeed;

        if (_unitType == UnitType.Player)
        {
            switch (_weapon.weaponData._WeaponType) 
            {
                case WeaponType.Sword:
                    _weapon.Swing();
                    _target.GetComponent<HealthSystem>().GetDamage(data.attack);
                    break;
                case WeaponType.Pistol:
                    _weapon.ShotBullet();
                    break;
                case WeaponType.SMG:
                    StartCoroutine(_weapon.TripleShot());
                    break;
            }
        }
        else if (_unitType == UnitType.Enemy)
            _target.GetComponent<HealthSystem>().GetDamage(data.attack);
    }

    private void Die()
    {
        if (_unitType == UnitType.Enemy)
            EnemyManager.Instance.UnregisterEnemy(this);
        
        if (_unitType == UnitType.HQ)
            BattleManager.Instance.Lose();
        
        Destroy(gameObject);
    }
}
