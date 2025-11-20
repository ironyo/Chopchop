using Unity.VisualScripting;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public HealthSystem HealthCompo {get; private set;}
<<<<<<< HEAD
    [SerializeField] private EnemyDataSO data;
=======
    [SerializeField] private UnitDataSO data;
>>>>>>> JJK
    
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
<<<<<<< HEAD
        UnitManager.Instance.GetNearestPlayer(transform);
=======
        _target = EnemyManager.Instance.GetNearestEnemy(transform);
>>>>>>> JJK
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
<<<<<<< HEAD
                _combat.TryAttack(_target, data.attackSpeed);
=======
                _combat.TryAttack(_target, data);
>>>>>>> JJK
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
