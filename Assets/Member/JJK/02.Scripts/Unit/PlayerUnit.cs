using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public HealthSystem HealthCompo {get; private set;}
    [SerializeField] private UnitDataSO data;
    
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
            MoveToTarget();
        }
    }

    private void SetTarget()
    {
<<<<<<< HEAD
        _target = UnitManager.Instance.GetNearestEnemy(transform);
=======
        _target = EnemyManager.Instance.GetNearestEnemy(transform);
>>>>>>> JJK
    }

    private void MoveToTarget()
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
