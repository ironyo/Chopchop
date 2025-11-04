using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JobButtonManager : MonoBehaviour
{
    public Minion Minion { get; private set; }
    public static JobButtonManager Instance;
    public Action<Minion> OnValueChanged;
    [SerializeField] private GetJobButton[] buttons;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        OnValueChanged += ChangeValue;
    }

    private void ChangeValue(Minion minion)
    {
        Minion = minion;
        foreach (var button in buttons)
        {
            button.targetMinion = Minion;
        }
    }

    private void OnDestroy()
    {
        OnValueChanged -= ChangeValue;
    }
}
