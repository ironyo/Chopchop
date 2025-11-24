using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerUnit : MonoBehaviour
{
    public HealthSystem HealthCompo {get; private set;}
    [SerializeField] private UnitDataSO data;
    
    private Transform _target;
    private WeaponHolder _weaponHolder;
    private Chase _chase;
    private Combat _combat;
    private string targetTag = "Enemy";

    [field:SerializeField] public UnityEvent<Vector3> OnTargetChanged { get; set; }
    
    private void Awake()
    {
        HealthCompo = GetComponent<HealthSystem>();
        HealthCompo.OnDead += Die;
        HealthCompo.OnDead += HitEffect;
        
        _chase = GetComponent<Chase>();
        _combat = GetComponent<Combat>();
    }

    private void HitEffect()
    {
        //GetComponentInChildren<SpriteRenderer>().color = Color.red;
    }

    private void Update()
    {
        if (InvasionManager.Instance.isLanding)
        {
            if (_target == null)
            {
                SetTarget();
            }
            else
            {
                OnTargetChanged.Invoke(_target.position);
            
                MoveToTarget();
            }
        }
    }

    private void SetTarget()
    {
        _target = _chase.GetNearestTarget(targetTag);
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
                _combat.TryAttack(_target, data.attackSpeed);
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
