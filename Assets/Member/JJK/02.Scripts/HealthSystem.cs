using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [field: SerializeField] public int HP { get; private set; }
    public int maxHealth;

    public Action OnDead;
    public Action OnDamaged;
    
    private HealthBar healthBar;
    
    private void Awake()
    {   
        HP = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Update()
    {
        if (healthBar.OnDead)
        {
            OnDead?.Invoke();
        }
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        HP = Mathf.Clamp(HP, 0, maxHealth);
        OnDamaged?.Invoke();
    }

    public float GetNormalizeHealth()
    {
        return (float)HP / maxHealth;   
    }
}
