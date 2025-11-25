using System;
using Member.CHJ._02.Scripts.Ui;
using UnityEngine;
using UnityEngine.InputSystem;

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
        if (healthBar == null)
            healthBar = GetComponentInChildren<HealthBar>();

        if (healthBar != null && healthBar.OnDead)
            OnDead?.Invoke();
    }


    public void GetDamage(int damage)
    {
        HP -= damage;
        HP = Mathf.Clamp(HP, 0, maxHealth);
        OnDamaged?.Invoke();

        if (HP <= 0)
        {
            if (gameObject.CompareTag("HQ"))
                GameEndPlay.Instance.OnGameEndEvent?.Invoke(GameEndType.GameOver, "본부를 지키지 못했습니다.");
            
            OnDead.Invoke();
        }
    }

    public float GetNormalizeHealth()
    {
        return (float)HP / maxHealth;   
    }

    public void SetHealth(int amount)
    {
        maxHealth = amount;
        HP = amount;
    }
}
