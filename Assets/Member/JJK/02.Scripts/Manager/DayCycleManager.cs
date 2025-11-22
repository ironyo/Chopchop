using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class DayCycleManager : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Volume postProcessVolume;

    [Header("Cycle Settings")]
    [SerializeField] private float transitionDuration = 3f; // 낮↔밤 전환 시간

    [Header("Light Colors")]
    [SerializeField] private Color dayColor = Color.white;
    [SerializeField] private Color nightColor = new Color(0.1f, 0.1f, 0.35f);
    
    [Header("Vignette Settings")]
    [SerializeField] private float dayVignetteIntensity = 0.4f;
    [SerializeField] private float nightVignetteIntensity = 0.0f;

    private Color startColor;
    private Color targetColor;
    private float startVignette;
    private float targetVignette;

    private Vignette vignette;

    private int minute;
    private int hours = 7;

    public UnityEvent OnNextDay;
    public UnityEvent<string> OnTimeChanged; // <HH:MM AM/PM>

    // 낮/밤 상태
    private bool isDay;             // 현재 실제 상태(전환 완료 기준)
    private bool isTransitioning;   // 전환 중인지
    private float transitionTimer;  // 전환용 타이머

    void Start()
    {
        if (postProcessVolume.profile.TryGet(out vignette))
        {
            // 시작은 낮 기준으로 세팅
            vignette.intensity.value = dayVignetteIntensity;
        }

        // 현재 시간 기준으로 낮/밤 초기화
        isDay = IsDayTime();
        if (isDay)
        {
            globalLight.color = dayColor;
            if (vignette != null) vignette.intensity.value = dayVignetteIntensity;
        }
        else
        {
            globalLight.color = nightColor;
            if (vignette != null) vignette.intensity.value = nightVignetteIntensity;
        }

        TimeManager.Instance.OnOneSecond += DisplayTime;
    }

    private void DisplayTime(int time)
    {
        // 1초마다 24분 증가 → 1분은 2.5초 기준인 듯 (기존 로직 유지)
        minute += 24;
            
        if (minute >= 60)
        {
            minute -= 60;
            hours++;
        }
            
        if (hours >= 24)
        {
            hours = 0;
            OnNextDay?.Invoke();
        }
            
        string ampm = hours < 12 ? "AM" : "PM";
        DayNightIconUI.Instance.Check(ampm);
        int displayHour = hours % 12;
        if (displayHour == 0) displayHour = 12;

        OnTimeChanged?.Invoke($"{displayHour:00}:{minute:00} {ampm}");
    }

    private void Update()
    {
        bool targetIsDay = IsDayTime();
        
        if (targetIsDay != isDay && !isTransitioning)
        {
            isTransitioning = true;
            transitionTimer = 0f;

            startColor = globalLight.color;
            targetColor = targetIsDay ? dayColor : nightColor;

            if (vignette != null)
            {
                startVignette = vignette.intensity.value;
                targetVignette = targetIsDay ? dayVignetteIntensity : nightVignetteIntensity;
            }
        }
        
        if (isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);

            globalLight.color = Color.Lerp(startColor, targetColor, t);

            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(startVignette, targetVignette, t);

            if (t >= 1f)
            {
                isTransitioning = false;
                isDay = targetIsDay;
            }
        }
    }
    
    private bool IsDayTime()
    {
        return hours >= 7 && hours < 19;
    }
}