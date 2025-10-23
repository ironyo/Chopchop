using UnityEngine;

public class EnemyHQ : MonoBehaviour
{
    public HealthSystem HealthCompo {get; private set;}
    private void Awake()
    {
        HealthCompo = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        HealthCompo.OnDead += BattleManager.Instance.Win;
    }
}
