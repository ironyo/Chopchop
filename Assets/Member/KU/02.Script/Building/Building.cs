using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using Member.CHJ._02.Scripts;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    public int buildCount = 0;

    public int nowHealth = 0;

    private int maxHealth = 0;
    public int MaxMinion { get; private set; } = 0;

    private BoxCollider2D boxCollider;
    private LineRenderer lineRenderer;

    public BuildingSO BuildingSO;
    public BuildingSelector BuildingSelector { get; private set; }

    public TextMeshProUGUI LogPrefab;
    private TextMeshProUGUI _minionText;

    public int Level { get; private set; } = 1;
    private int minionCount = 0;
    private float spawnTime = 0;
    private float spawnCurrentTime = 0;
    [SerializeField]private List<ResourceTypeCost> spawnAmount = new();

    [Header("Collider View Settings")]
    public bool ShowCollider = true;
    [SerializeField] Color colliderColor = Color.green;
    [SerializeField] float lineWidth = 0.05f;

    public int NowLevel
    {
        get
        {
            return Level;
        }
        set
        {
            if (BuildingSO.maxLevel >= value)
                Level = value;
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
            if(MaxMinion >= value)
                minionCount = value;
        }
    }

    private void Start()
    {
        MinionManager.Instance.MinionsBuildingManager.AddBuilding(this);
        BuildingSetUp();

        boxCollider = GetComponent<BoxCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        _minionText = GetComponentInChildren<TextMeshProUGUI>();

        BuildingSelector = GetComponent<BuildingSelector>();
        int wSize = Mathf.RoundToInt(BuildingSO.width / BuildingSO.maxW);
        boxCollider.size = new Vector2(BuildingSO.maxW + 2,  wSize + 2);



        InitializeLineRenderer();
    }

    private void Update()
    {
        // _minionText.text = $"{buildingSO.buildName}\n{minionCount} / {maxMinion}";
        UpdateColliderView();
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            BuildUpgrade();
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            MinionPlus(1);
        }
        if(BuildingSO.levelResourceType.Length != 0)
            UpdateSpawnResource();
    }

    private void UpdateSpawnResource()
    {
        if (minionCount == 0) return;

        spawnCurrentTime += Time.deltaTime;
        if(spawnCurrentTime >= spawnTime)
        {
            spawnCurrentTime = 0;
            ResourceManager.Instance.AddResource(spawnAmount[Level-1].resourceTypeSO, spawnAmount[Level-1].amount * minionCount);
            ResourceLog(Level-1);
        }
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
    public void BuildUpgrade()
    {
        NowLevel++;
        BuildingSetUp();
    }

    public bool CanReserve()
    {
        return NowMinion < MaxMinion;
    }
    public bool TryReserve()
    {
        if (!CanReserve())
            return false;
        Debug.Log("Can Add Minion On Building");
        MinionPlus(1);
        return true;
    }

    public void Release()
    {
        NowMinion = Mathf.Max(0, NowMinion - 1);
    }
    public void MinionPlus(int plus)
    {
        NowMinion += plus;
    }
    private void BuildingSetUp()
    {
        maxHealth = BuildingSO.MaxHealth[Level-1];
        MaxMinion = BuildingSO.maxMinion[Level-1];
        spawnTime = BuildingSO.spawnTime;
        nowHealth = maxHealth;
        if (BuildingSO.levelResourceType.Length != 0)
        {
            for (int i = 0; i < BuildingSO.levelResourceType[Level - 1].resourceTypeSOs.Length; i++)
            {
                spawnAmount.Add(BuildingSO.levelResourceType[Level-1].resourceTypeSOs[i]);
            }
        }
        else if(spawnAmount.Count != 0)
            SpawnResourceTypeChange();
    }
    private void ResourceLog(int num)
    {
        TextMeshProUGUI obj = Instantiate(LogPrefab, new Vector2(transform.position.x, transform.position.y + BuildingSO.width/BuildingSO.maxW-1), Quaternion.identity,transform);
        obj.text = $"{spawnAmount[num].resourceTypeSO.name} +{spawnAmount[num].amount * minionCount}";
    }
    private void SpawnResourceTypeChange()
    {
        for (int i = 0; i < spawnAmount.Count; i++)
        {
            spawnAmount[i].resourceTypeSO = BuildingSO.levelResourceType[Level].resourceTypeSOs[i].resourceTypeSO;
            spawnAmount[i].amount = BuildingSO.levelResourceType[Level].resourceTypeSOs[i].amount;
        }
    }

    private void InitializeLineRenderer()
    {
        lineRenderer.positionCount = 4;

        lineRenderer.loop = true;

        lineRenderer.useWorldSpace = false;

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.startColor = colliderColor;
        lineRenderer.endColor = colliderColor;

        lineRenderer.enabled = ShowCollider;
    }
    private void UpdateColliderView()
    {
        lineRenderer.enabled = ShowCollider;

        if (!ShowCollider || boxCollider == null) return;

        Vector2 size = boxCollider.size;
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