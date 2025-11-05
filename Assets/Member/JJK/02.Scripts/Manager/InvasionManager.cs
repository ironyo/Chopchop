
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class InvasionManager : MonoBehaviour
{
    public static InvasionManager Instance;
    
    [SerializeField] private int minCount = 3, maxCount = 7;
    [SerializeField] private int minTime = 100, maxTime = 300;
    
    private int enemyCount;
    private float invasionTime;
    private float timer;
    private bool isInvading = false;
    
    private ShipSpawner spawner;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        
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
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            //Invasion();
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
