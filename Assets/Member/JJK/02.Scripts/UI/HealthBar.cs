using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _bar;
    [SerializeField] private float changeSpeed = 2f;
    [SerializeField] private float disableTime = 5f;
    
    private HealthSystem healthSystem;
    
    private float targetHealthNormalized;
    private float currentHealthNormalized;

    private GameObject visual;
    private SpriteRenderer[] sprites;
    private Color[] originalColors;

    public bool OnDead { get; private set; } = false;

    private void Awake()
    {
        healthSystem = GetComponentInParent<HealthSystem>();
        visual = transform.GetChild(0).gameObject;
        
        sprites = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            originalColors[i] = sprites[i].color;
        }
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        healthSystem.OnDamaged += OnDamagedHandler;
        targetHealthNormalized = currentHealthNormalized = healthSystem.GetNormalizeHealth();
        healthSystem.OnDamaged += UpdateBar;        
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

    private void OnDamagedHandler()
    {
        visual.SetActive(true);
        
        foreach (var s in sprites)
            s.DOKill();
        
        foreach (var s in sprites)
        {
            for (int i = 0; i < sprites.Length; i++)
                sprites[i].color = originalColors[i];
            
            s.DOFade(0f, 1f)
                .SetDelay(disableTime);
        }
    }
    
    private void OnDestroy()
    {
        foreach (var sr in sprites)
            if (sr != null) sr.DOKill();
        
        healthSystem.OnDamaged -= UpdateBar;
        healthSystem.OnDamaged -= OnDamagedHandler;
    }
}
