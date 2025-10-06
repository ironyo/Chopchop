using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public float currentTime;
    public int day;
    public event Action OnDayStarted;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        OnDayStarted?.Invoke();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 60)
        {
            day++;
            currentTime = 0;
            OnDayStarted?.Invoke();
        }
    }
}
