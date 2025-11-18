
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class InvasionManager : MonoSingleton<InvasionManager>
{
    [SerializeField] private int minCount = 3, maxCount = 7;
    [SerializeField] private int minTime = 100, maxTime = 300;
    public bool isLanding = false;
    
    private int enemyCount;
    private float invasionTime;
    private float timer;
    private bool isInvading = false;
    
    private ShipSpawner spawner;

    protected override void Awake()
    {
        base.Awake();
        
        spawner = GetComponentInChildren<ShipSpawner>();
    }

    private void Start()
    {
        InitInvasion();
    }

    private void InitInvasion()
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
            InitInvasion();
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            DialogManager.Instance.InvasionDialog();
        }
    }

    private IEnumerator InvasionWarning()
    {
        Debug.Log($"적{enemyCount}명이 5초 뒤에 침략합니다");
        
        yield return new WaitForSeconds(5f);
        
        Invasion();
    }

    public void Invasion()
    {
        InitInvasion();
        spawner.SpawnShip(enemyCount);
        isInvading = true;
    }
}
