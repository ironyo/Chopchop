using System;
using UnityEngine;

public class MateManager : MonoBehaviour
{
    public static MateManager Instance;
    public bool canMate = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
