using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _bar;
    [SerializeField] private float changeSpeed = 2f;
    private HealthSystem healthSystem;
    
    private float targetHealthNormalized;
    private float currentHealthNormalized;

    public bool OnDead { get; private set; } = false;

    private void Awake()
    {
        healthSystem = GetComponentInParent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.OnDamaged += UpdateBar;
        targetHealthNormalized = currentHealthNormalized = healthSystem.GetNormalizeHealth();
    }

    private void Update()
    {
        currentHealthNormalized = Mathf.Lerp(currentHealthNormalized, targetHealthNormalized, Time.deltaTime * changeSpeed);
        _bar.localScale = new Vector3(currentHealthNormalized, transform.localScale.y, transform.localScale.z);

        if (currentHealthNormalized <= 0.01f)
        {
            OnDead = true;
        }
    }

    private void UpdateBar()
    {
        targetHealthNormalized = healthSystem.GetNormalizeHealth();
    }

    private void OnDestroy()
    {
        healthSystem.OnDamaged -= UpdateBar;
    }
}
