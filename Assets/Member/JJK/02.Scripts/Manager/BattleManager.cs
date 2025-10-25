using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    private EnemyHQ enemyHq;

    [SerializeField] private UnitData[] data;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void BattleStart()
    {
        SceneManager.LoadScene("BattleScene");
        //전투 시작
        //적 유닛 생성
        //아군 유닛 생성
    }

    private void SpawnAttacker()
    {
        foreach (var unitPrefab in data)
        {
            
        }
    }

    public void Win()
    {
        //살아있는 적 미니언들 포로로 잡아가기
        //땅에 있는 자원 가져가기
    }
    
    public void Surrender()
    {
        //가지고 있는 자원의 절반 상대에게 나눠주기
    }

    public void Lose()
    {
        //게임 오버
    }

    public void CheckBattleEnd()
    {
        //if (playerUnits.Count <= 0) return;
    }
}
