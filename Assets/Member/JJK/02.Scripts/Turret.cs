using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private TowerStat _stat;
    private int _damage;
    private int _attackRange;
    private int _attackSpeed;
    
    public HealthSystem HealthSystem {get; private set;}

    private void Awake()
    {
        _damage = _stat.damage;
        _attackRange = _stat.attackRange;
        _attackSpeed = _stat.attackSpeed;
        
        HealthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        HealthSystem.OnDead += () => Destroy(gameObject);
    }

    private void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, _attackRange))
        {
            
        }
    }
}
