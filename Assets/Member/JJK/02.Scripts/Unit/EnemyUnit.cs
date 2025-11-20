using Unity.VisualScripting;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public HealthSystem HealthCompo {get; private set;}
    [SerializeField] private EnemyDataSO data;
    
    private Transform _target;
    private WeaponHolder _weaponHolder;
    private Chase _chase;
    private Combat _combat;
    
    private void Awake()
    {
        HealthCompo = GetComponent<HealthSystem>();
        HealthCompo.OnDead += Die;
        
        _chase = GetComponent<Chase>();
        _combat = GetComponent<Combat>();
    }

    private void Update()
    {
        if (InvasionManager.Instance.isLanding)
        {
            SetTarget();
            ChaseAndAttack();
        }
    }

    private void SetTarget()
    {
        UnitManager.Instance.GetNearestPlayer(transform);
    }

    private void ChaseAndAttack()
    {
        if (_target != null)
        {
            _chase.MoveTo(_target);
            float distance = _chase.GetDistance(_target);
            float attackRange = data.attackRange;
            
            if (distance < attackRange)
            {
                _chase.Stop();
                _combat.TryAttack(_target, data.attackSpeed);
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
