using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.Cinemachine;

public class ParticleCallBack : MonoBehaviour
{
    [SerializeField] private UnityEvent onParticleEnd;

    private void Start()
    {
        var ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnParticleSystemStopped()
    {
        onParticleEnd.Invoke();
    }
}
 