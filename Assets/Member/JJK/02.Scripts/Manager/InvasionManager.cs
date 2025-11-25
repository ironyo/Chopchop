using System;
using System.Collections;
using Member.CHJ._02.Scripts.Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class InvasionManager : MonoSingleton<InvasionManager>
{
    [SerializeField] private int minCount = 3, maxCount = 7;
    [SerializeField] private int minTime = 100, maxTime = 300;
    [SerializeField] private GameObject invasionWarning;
    public bool isLanding = false;
    
    private int enemyCount;
    private float invasionTime;
    private float timer;
    private int count = 0;
    
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
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 5f)
        {
            StartCoroutine(InvasionWarning());
            InitInvasion();
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            StartCoroutine(InvasionWarning());
        }
    }

    private IEnumerator InvasionWarning()
    {
        var warning = invasionWarning.GetComponent<WarningUI>();
        warning.OpenUI();
        
        yield return new WaitForSeconds(3f);
        
        warning.CloseUI();
        
        yield return new WaitForSeconds(1f);
        
        DialogManager.Instance.InvasionDialog();
    }

    public void Invasion()
    {
        InitInvasion();
        count++;
        for (int i = 0; i < count; i++)
        {
            spawner.SpawnShip(enemyCount);
        }
    }
}
