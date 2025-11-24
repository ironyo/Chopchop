using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoSingleton<BattleManager>
{
    public bool isBattle = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Win()
    {
        isBattle = false;
    }
    
    public void Surrender()
    {
        
    }

    public void Lose()
    {
        isBattle = false;
    }
}
