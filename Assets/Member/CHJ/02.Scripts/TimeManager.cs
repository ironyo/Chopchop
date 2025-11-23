using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    [field: SerializeField]public int CurrentTime { get; private set; }
    public int Day { get; private set; }
    public event Action OnDayStarted;
    public UnityEvent OnDayEnded; // 보고 시스템 용도
    public Action<int> OnOneSecond;
    private const float Tick = 1;
    private WaitForSeconds _waitT = new WaitForSeconds(Tick);

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(TimeLoop());
    }

    private IEnumerator TimeLoop()
    {
        OnDayStarted?.Invoke();
        while (true)
        {
            yield return _waitT;
            CurrentTime++;
            OnOneSecond?.Invoke(CurrentTime);
            if (CurrentTime >= 60)
            {
                Day++;
                CurrentTime = 0;
                OnDayStarted?.Invoke();

                if (Day == 2)
                {
                    OnDayEnded?.Invoke();
                }
            }
        }
    }


    
}
