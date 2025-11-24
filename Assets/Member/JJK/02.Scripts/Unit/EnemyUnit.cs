using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyUnit : MonoBehaviour
{
    public HealthSystem HealthCompo {get; private set;}
    public EnemyDataSO data;
    [SerializeField] private string[] targetTags;
    
    private Transform _target;
    private WeaponHolder _weaponHolder;
    private Chase _chase;
    private Combat _combat;
    
    [field:SerializeField] public UnityEvent<Vector3> OnTargetChanged { get; set; }
    
    private void Awake()
    {
        HealthCompo = GetComponent<HealthSystem>();
        HealthCompo.OnDead += Die;
        
        _chase = GetComponent<Chase>();
        _combat = GetComponent<Combat>();

        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RegisterEnemy(this);
        }
    }

    private void Update()
    {
        if (InvasionManager.Instance.isLanding)
        {
            if (_target == null)
            {
                SetTarget();
            }
            
            OnTargetChanged.Invoke(_target.position);
            ChaseAndAttack();
        }
    }

    private void SetTarget()
    {
        switch (data.targetType)
        {
            case TargetType.Player:
                _target = GetTargetByPriority(
                    targetTags[0], // Minion
                    targetTags[1], // Building
                    targetTags[2]); // HQ
                break;
            case TargetType.Building:
                _target = GetTargetByPriority(
                    targetTags[1], //Building
                    targetTags[2]); //HQ
                break;
            case TargetType.HQ:
                _target = GetTargetByPriority(targetTags[2]); //HQ
                break;
        }
    }
    
    private Transform GetTargetByPriority(params string[] tags)
    {
        foreach (var tag in tags)
        {
            if (string.IsNullOrEmpty(tag)) 
                continue;

            Transform t = _chase.GetNearestTarget(tag);
            if (t != null)
                return t;
        }

        return null;
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
        if (EnemyManager.Instance != null)
            EnemyManager.Instance.UnregisterEnemy(this);
        
        Destroy(gameObject);
    }
}
