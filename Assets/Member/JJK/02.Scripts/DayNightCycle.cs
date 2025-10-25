using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Volume postProcessVolume;

    [Header("Cycle Settings")]
    [SerializeField] private float transitionDuration = 3f; // 전환 시간
    [SerializeField] private float holdDuration = 5f; // 유지 시간

    [Header("Light Colors")]
    [SerializeField] private Color dayColor = Color.white;
    [SerializeField] private Color nightColor = new Color(0.1f, 0.1f, 0.35f);
    
    [Header("Vignette Settings")]
    [SerializeField] private float dayVignetteIntensity = 0.4f;
    [SerializeField] private float nightVignetteIntensity = 0.0f;

    private float timer = 0f;
    private bool isDay = true;
    private enum CycleState { Holding, Transitioning }
    private CycleState state = CycleState.Holding;

    private Color startColor;
    private Color targetColor;
    private float startVignette;
    private float targetVignette;

    private Vignette vignette;

    void Start()
    {
        if (postProcessVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = dayVignetteIntensity;
        }
        
        globalLight.color = dayColor;
        startColor = dayColor;
        targetColor = nightColor;
        startVignette = dayVignetteIntensity;
        targetVignette = nightVignetteIntensity;
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case CycleState.Holding:
                if (timer >= holdDuration)
                {
                    timer = 0f;
                    state = CycleState.Transitioning;
                    
                    startColor = globalLight.color;
                    targetColor = isDay ? nightColor : dayColor;
                    
                    startVignette = vignette.intensity.value;
                    targetVignette = isDay ? nightVignetteIntensity : dayVignetteIntensity;
                }
                break;

            case CycleState.Transitioning:
                float t = timer / transitionDuration;
                globalLight.color = Color.Lerp(startColor, targetColor, t);
                vignette.intensity.value = Mathf.Lerp(startVignette, targetVignette, t);
                
                if (timer >= transitionDuration)
                {
                    timer = 0f;
                    isDay = !isDay;
                    state = CycleState.Holding;
                }
                break;
        }
    }
}