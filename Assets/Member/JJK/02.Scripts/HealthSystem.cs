using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [field: SerializeField] public int HP { get; private set; }
    [SerializeField] private int maxHealth;

    public event Action OnDead;
    
    private void Awake()
    {   
        HP = maxHealth;
    }

    public void GetDamage(int damage, GameObject target)
    {
        HP -= damage;
        HP = Mathf.Clamp(HP, 0, maxHealth);

        if (HP <= 0)
        {
            Debug.Log("Dead");
            OnDead?.Invoke();
        }
    }
}
