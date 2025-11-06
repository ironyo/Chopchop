using System;
using UnityEngine;
using UnityEngine.Events;

public class MateManager : MonoBehaviour
{
    public static MateManager Instance;
    public UnityEvent OnMate;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartMate()
    {
        OnMate?.Invoke();
    }
}
