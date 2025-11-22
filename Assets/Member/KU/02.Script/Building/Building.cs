using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using Member.CHJ._02.Scripts;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public bool isNowBuilding { get; private set; } = true;

    public int buildCount = 0;

    public int nowHealth = 0;

    private int maxHealth = 0;
    public int maxMinion { get; private set; } = 0;

    public BoxCollider2D boxCollider { get; private set; }
    private LineRenderer lineRenderer;

    public SpriteRenderer spr { get; set; }
    GameObject _timerBuildingPref;

    public BuildingSO buildingSO;
    public BuildingSelector buildingSelector { get; private set; }
    ParticleSystem _minionSpawnParticle;
    ParticleSystem _minionBuildParticle;
    TextMeshPro _logPrefab;
    Image _timerPref;
    TextMeshPro _minionText;

    public int level { get; private set; } = 1;
    private int minionCount = 0;
    private float spawnTime = 0;
    private float spawnCurrentTime = 0;
    [SerializeField]private List<ResourceTypeCost> spawnAmount = new();

    [Header("Collider View Settings")]
    public bool showCollider { get; set; } = true;
    [SerializeField] Color colliderColor = Color.green;
    [SerializeField] float lineWidth = 0.05f;

    public int NowLevel
    {
        get
        {
            return level;
        }
        set
        {
            if (buildingSO.maxLevel >= value)
                level = value;
        }
    }
    public int NowMinion
    {
        get
        {
            return minionCount;
        }
        set
        {
            if(maxMinion >= value)
                minionCount = value;
        }
    }

    public int showMinion = 0;
    private void Start()
    {
        LevelManager.Instance.IncreseLevel(10);
        
        if (buildingSO.resourceTypeCost.Length != 0)
        {
            gameObject.tag = "Building";
        }
        else
            gameObject.tag = "HQ"; MinionManager.Instance.MinionsBuildingManager.AddBuilding(this);
        BuildingSetUp();


        boxCollider = GetComponent<BoxCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        _minionText = GetComponentInChildren<TextMeshPro>();

        buildingSelector = GetComponent<BuildingSelector>();
        int wSize = Mathf.RoundToInt(buildingSO.width / buildingSO.maxW);
        boxCollider.size = new Vector2(buildingSO.maxW + 2f,  wSize + 2f);

        InitializeLineRenderer();

        boxCollider.size = new Vector2(buildingSO.maxW, wSize);

        spr = Instantiate(BuildManager.Instance.buildSpritePref, boxCollider.bounds.center, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
        spr.sprite = buildingSO.buildSprite;
        spr.size = new Vector2(buildingSO.maxW, buildingSO.maxW);
        SetAColor(0.25f);
        Image timerImg = Instantiate(_timerPref, new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.center.y-2f), Quaternion.identity, transform);
        NowBuildingTimer buildTimer = Instantiate(_timerBuildingPref, boxCollider.bounds.center, Quaternion.identity, spr.gameObject.transform).GetComponent<NowBuildingTimer>();
        buildTimer.GetData(this, timerImg);

        _minionText.text = $"{buildingSO.buildName}";
        if (buildingSO.maxMinion[0] != 0)
            _minionText.text += $"\n{showMinion} / {maxMinion}";
    }

    private void Update()
    {
        UpdateColliderView();


        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            MinionPlus(1);
        }
        if(buildingSO != null)
        {
            if (buildingSO.levelResourceType.Length != 0)
                UpdateSpawnResource();
        }

    }

    private int minusTimer;
    private void UpdateSpawnResource()
    {
        if (isNowBuilding) return; 

        minusTimer = buildingSO.levelResourceType[0].minion != null ? level : 0;
        if (minionCount == 0 && buildingSO.levelResourceType[level-1].minion == null) return;

        spawnCurrentTime += Time.deltaTime;
        if (spawnCurrentTime >= spawnTime - minusTimer)
        {
            if (buildingSO.levelResourceType[level - 1].minion != null)
            {
                spawnCurrentTime = 0;
                Instantiate(_minionSpawnParticle, transform.position, Quaternion.identity);
                Instantiate(buildingSO.levelResourceType[level - 1].minion, new Vector2(transform.position.x + 1.5f, transform.position.y -1.5f), Quaternion.identity);
                ResourceLog(level - 1, true);

            }
            else
            {
                spawnCurrentTime = 0;
                ResourceManager.Instance.AddResource(spawnAmount[level - 1].resourceTypeSO, spawnAmount[level - 1].amount * minionCount);
                ResourceLog(level - 1, false);
            }
        }
    }

    public void BuildingRealClear()
    {
        if (buildingSO.levelResourceType.Length != 0)
        {
            if (buildingSO.levelResourceType[0].minion != null)
            {
                GameObject particle = Instantiate(_minionBuildParticle, transform.position, Quaternion.identity, transform).gameObject;
                particle.transform.position += new Vector3(-0.5f, 1.1f);
            }
        }
        SetAColor(1);
        isNowBuilding = false;
    }
    private void OnDrawGizmos()
    {
        if (boxCollider != null)
        {
            Gizmos.color = Color.green;

            Vector3 boxPos = transform.position + (Vector3)boxCollider.offset;

            Vector3 boxSize = new Vector3(boxCollider.size.x, boxCollider.size.y, 0f);

            Gizmos.DrawWireCube(boxPos, boxSize);
        }
    }

    public void BuildSpawnSetting(TextMeshPro logpre, BuildingSO buildSo, ParticleSystem spawn, ParticleSystem build, Image timer, GameObject timerObj)
    {
        _logPrefab = logpre;
        buildingSO = buildSo;
        _minionSpawnParticle = spawn;
        _minionBuildParticle = build;
        _timerPref = timer;
        _timerBuildingPref = timerObj;
    }

    public void BuildUpgrade()
    {
        if (isNowBuilding) return;

        NowLevel++;
        BuildingSetUp();
        BuildUISetUp();
    }

    private void SetAColor(float amount)
    {
        Color color = spr.color;
        color.a = amount;
        spr.color = color;
    }
    public bool CanReserve()
    {
        Debug.Log($"{NowMinion} < {maxMinion} = {NowMinion < maxMinion}");
        return NowMinion < maxMinion;
    }
    public bool TryReserve()
    {
        if (!CanReserve())
            return false;
        Debug.Log("Can Add Minion On Building");
        MinionPlus(1);
        return true;
    }

    public void AddShowMinion()
    {
        if (isNowBuilding) return;

        showMinion++;
        BuildUISetUp();
        Debug.Log($"Add Show Minion {showMinion}");
    }
    public void Release()
    {
        if (isNowBuilding) return;

        NowMinion = Mathf.Max(0, NowMinion - 1);
        showMinion--;
        BuildUISetUp();
    }
    public void MinionPlus(int plus)
    {
        if (isNowBuilding) return;

        NowMinion += plus;
        BuildUISetUp();
    }
    private void BuildingSetUp()
    {
        maxHealth = buildingSO.MaxHealth[level-1];
        maxMinion = buildingSO.maxMinion[level-1];
        spawnTime = buildingSO.spawnTime;
        nowHealth = maxHealth;
        if (buildingSO.levelResourceType.Length != 0)
        {
            for (int i = 0; i < buildingSO.levelResourceType[level - 1].resourceTypeSOs.Length; i++)
            {
                spawnAmount.Add(buildingSO.levelResourceType[level-1].resourceTypeSOs[i]);
            }
        }
        else if(spawnAmount.Count != 0)
            SpawnResourceTypeChange();

    }
    private void ResourceLog(int num, bool isMinion)
    {
        TextMeshPro obj = Instantiate(_logPrefab, new Vector2(transform.position.x, transform.position.y + buildingSO.width/buildingSO.maxW-1), Quaternion.identity,transform);
        if(isMinion)
        {
            obj.text = $"+미니언";
            obj.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        else
        {
            obj.text = $"+{spawnAmount[num].amount * minionCount}";
            obj.GetComponentInChildren<SpriteRenderer>().sprite = buildingSO.levelResourceType[0].resourceTypeSOs[0].resourceTypeSO.Icon;
        }
    }
    private void SpawnResourceTypeChange()
    {
        for (int i = 0; i < spawnAmount.Count; i++)
        {
            spawnAmount[i].resourceTypeSO = buildingSO.levelResourceType[level].resourceTypeSOs[i].resourceTypeSO;
            spawnAmount[i].amount = buildingSO.levelResourceType[level].resourceTypeSOs[i].amount;
        }
        BuildUISetUp();
    }

    public void AttackBuild(int damage)
    {
        if (isNowBuilding) return;

        nowHealth -= damage;
        if (nowHealth <= 0)
        {
            BuildManager.Instance.DestroyBuilding(this);
            MinionManager.Instance.MinionsBuildingManager.RemoveBuilding(this);
        }
        BuildUISetUp();
    }

    public void BuildUISetUp()
    {
        _minionText.text = $"{buildingSO.buildName}";
        if (buildingSO.levelResourceType.Length == 0)
            _minionText.text += $"\n{showMinion} / {maxMinion}";
        else if (buildingSO.levelResourceType[0].minion == null)
            _minionText.text += $"\n{showMinion} / {maxMinion}";
    }

    private void InitializeLineRenderer()
    {
        lineRenderer.positionCount = 4;

        lineRenderer.loop = true;

        lineRenderer.useWorldSpace = false;

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;

        lineRenderer.enabled = showCollider;
    }
    private void UpdateColliderView()
    {
        lineRenderer.enabled = !InventoryManager.Instance.IsNowClose;

        if (!showCollider || boxCollider == null) return;

        Vector2 size = new Vector2(boxCollider.size.x +2, boxCollider.size.y+2);
        Vector2 offset = boxCollider.offset;

        Vector3[] points = new Vector3[5]
        {
            offset + new Vector2(-size.x/2, -size.y/2),
            offset + new Vector2(-size.x/2, size.y/2),
            offset + new Vector2(size.x/2, size.y/2),
            offset + new Vector2(size.x/2, -size.y/2),
            offset + new Vector2(-size.x/2, -size.y/2)
        };

        lineRenderer.SetPositions(points);
    }
}