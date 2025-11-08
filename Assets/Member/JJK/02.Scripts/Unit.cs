using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum UnitType {Player, Enemy, HQ}

public class Unit : MonoBehaviour
{
    public HealthSystem healthCompo {get; private set;}
    public UnitDataSO data;
    public UnitType _unitType;
    [SerializeField] private float attackRange = 2f;
    public bool isLanding = false;
    [field: SerializeField] public UnityEvent<Vector3> OnTargetChanged { get; private set; }
    
    private NavMeshAgent navAgent;
    private float attackCooldown;
    private Unit target;
    private Rifle rifle;

    private void Awake()
    {
        if (_unitType != UnitType.HQ)
            NavAgentSet();
        
        healthCompo = GetComponent<HealthSystem>();
        //healthCompo.maxHealth = data.hp;
        healthCompo.OnDead += Die;
        
        if (_unitType == UnitType.Player)
            rifle = transform.Find("WeaponParent/Rifle").GetComponent<Rifle>();
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

        if (_unitType != UnitType.HQ)
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
        else if (_unitType == UnitType.Enemy && isLanding)
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
        
        _target.GetComponent<HealthSystem>().GetDamage(data.attack);
        attackCooldown = data.attackSpeed;
        
        if (_unitType == UnitType.Player)
            rifle.ShootBullet();
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
