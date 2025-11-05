using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    public bool isBattle = false;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Win()
    {
        isBattle = false;
        Debug.Log("Win");
    }
    
    public void Surrender()
    {
        
    }

    public void Lose()
    {
        isBattle = false;
        Debug.Log("Game Over");
    }
}
