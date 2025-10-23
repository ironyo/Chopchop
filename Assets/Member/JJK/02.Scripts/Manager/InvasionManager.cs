using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class InvasionManager : MonoBehaviour
{
    [SerializeField] private int minCount = 3, maxCount = 7;
    [SerializeField] private int minTime = 100, maxTime = 300;
    [SerializeField] private UnitData enemyData;
    
    private int enemyCount;
    private float invasionTime;
    private float timer;
    private bool isInvading = false;
    
    private ShipSpawner spawner;

    private void Awake()
    {
        spawner = GetComponentInChildren<ShipSpawner>();
    }

    private void Start()
    {
        InitInvation();
    }

    private void InitInvation()
    {
        enemyCount = Random.Range(minCount, maxCount);
        invasionTime = Random.Range(minTime, maxTime);
        timer = invasionTime;
        isInvading = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (!isInvading && timer <= 5f)
        {
            StartCoroutine(InvasionWarning());
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Invasion();
        }
    }

    private IEnumerator InvasionWarning()
    {
        isInvading = true;
        Debug.Log($"적{enemyCount}명이 5초 뒤에 침략합니다");
        
        yield return new WaitForSeconds(5f);
        
        Invasion();
    }

    private void Invasion()
    {
        spawner.SpawnShip(enemyCount);
        InitInvation();
    }

    public void Win()
    {
        
    }

    public void Lose()
    {
        //게임 오버
    }
}
