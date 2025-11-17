using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildManager : MonoSingleton<BuildManager>
{
    private Vector3Int currentCell;
    private Vector3Int lastCell;
    private int width;
    private int maxW = 3;

    [SerializeField] private TextMeshProUGUI _logPrefab;
    [SerializeField] private Grid grid;

    [SerializeField] private GameObject _clone;
    [SerializeField] private GameObject _buildClone;
    [SerializeField] private GameObject _helpUI;
    [SerializeField] private GameObject _buildingUI;
    [SerializeField] private GameObject _buildingCanvus;
    [SerializeField] private GameObject _blockTilemap;

    [SerializeField] List<Building> buildingParent = new();

    [Header("UISetting")]
    [SerializeField] private float _moveDistance = 600f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private RectTransform _uiRectTransform;
    [SerializeField] private TextMeshProUGUI _buildNameTex;
    [SerializeField] private TextMeshProUGUI _buildHPTex;
    [SerializeField] private TextMeshProUGUI _levelTex;
    [SerializeField] private TextMeshProUGUI _spawnKindTex;
    [SerializeField] private Button _selectBtn;
    [SerializeField] private Button _upgradeBtn;
    [SerializeField] private Button _destroyBtn;

    private bool _nowSelect; //true면 빌딩 false면 파괴
    private Vector2 _targetPos;
    private float _time = 0;

    [Header("Collider View Setting")]
    [SerializeField] Color colliderColor = Color.green;
    [SerializeField] float lineWidth = 0.05f;
    private bool showCollider = false;

    private List<GameObject> spawnGrid = new();
    private List<BuildingSelector> selectorCompo = new();
    private bool isBuilding;
    private BuildingSO buildingSO;
    private int buildingCount = 0;

    int selectLevel = 0;
    bool isDestroing = false;
    public bool isMoveInv = false;

    private int selectCount = 0;


    private LineRenderer lineRenderer;

    private Vector2 spawnPos;

    private BoxCollider2D boxCollider;

    //public static BuildManager Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        //if(Instance == null)
        //    Instance = this;
        boxCollider = GetComponent<BoxCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _selectBtn.onClick.AddListener(() =>
        {
            SelectButton();
        });
        _selectBtn.gameObject.SetActive(false);
        _upgradeBtn.onClick.AddListener(() =>
        {
            UpgradeDestroy(true);
        });
        _destroyBtn.onClick.AddListener(() =>
        {
            UpgradeDestroy(false);
        });
        InitializeLineRenderer();
    }

    private void Update()
    {
        UpdateColliderView();
        BuildOrCancle();
        BuildUISetting(!BuildingSelect());


        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()); // 강유야 여기 아아아아아아아아아ㅏ 진짜ㅏㅏㅏㅏㅏㅏㅏㅏㅏ
        currentCell = grid.WorldToCell(mouseWorldPos);

        if (currentCell != lastCell)
        {
            Vector3 snappedPos = grid.GetCellCenterWorld(currentCell);
            snappedPos.z = 0;
            transform.position = snappedPos;
            lastCell = currentCell;
        }
        _helpUI.SetActive(isBuilding);
        boxCollider.enabled = isBuilding;
    }

    private void BuildOrCancle()
    {
        if (spawnGrid != null && Mouse.current.rightButton.wasPressedThisFrame && isBuilding)
        {
            showCollider = false;
            isBuilding = false;
            GridDestroy();
        }
        if (spawnGrid != null && Mouse.current.leftButton.wasPressedThisFrame && isBuilding)
        {
            Debug.Log("aaaaaaaa");
            BuildedClear();
        }
    }
    public void Buildings(bool idBuilding, BuildingSO buildSO)
    {
        isBuilding = idBuilding;
        if (isBuilding)
        {
            buildingSO = buildSO;
            GridDestroy();
            width = buildSO.width;
            maxW = buildSO.maxW;
            int wSize = Mathf.RoundToInt(width / maxW);
            boxCollider.size = new Vector2(maxW+2, wSize+2);
            GridSpawn();
        }
    }
    private int GetZigzagOffset(int index)
    {
        if (index == 0) return 0;
        int k = (index + 1) / 2;
        return (index % 2 == 1) ? -k : k;
    }
    private void GridSpawn()
    {
        showCollider = true;
        float baseX = transform.position.x;
        float baseY = transform.position.y;

        for (int i = 0; i < width; i++)
        {
            int row = i / maxW;
            int col = i % maxW;

            float offsetX = GetZigzagOffset(col);
            float offsetY = GetZigzagOffset(row);

            spawnPos = new Vector3(baseX + offsetX, baseY + offsetY, 0f);

            GameObject obj = Instantiate(_clone, spawnPos, Quaternion.identity, transform);
            spawnGrid.Add(obj);
        }

        Vector2 center = Vector2.zero;
        foreach (var obj in spawnGrid)
        {
            center += (Vector2)obj.transform.position;
        }
        center /= spawnGrid.Count;

        if (boxCollider != null)
        {
            boxCollider.offset = center - (Vector2)transform.position;
        }

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastCell = grid.WorldToCell(mouseWorldPos);
    }
    private void GridDestroy()
    {
        for (int i = 0; i < spawnGrid.Count; i++)
        {
            Destroy(spawnGrid[i]);
        }
        spawnGrid.Clear();
    }
    private void BuildedClear()
    {
        if (!CanSpawn() || !CanResourceAmount()) return;

        showCollider = false;
        isBuilding = false;
        GameObject par = new GameObject(buildingSO.name);
        par.transform.parent = _buildingCanvus.transform;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        par.transform.position = mousePos;

        Building building = par.AddComponent<Building>();
        building.LogPrefab = _logPrefab;
        building.gameObject.AddComponent<LineRenderer>();
        building.gameObject.AddComponent<BuildingSelector>();
        building.BuildingSO = buildingSO;
        BoxCollider2D col = par.AddComponent<BoxCollider2D>();
        buildingParent.Add(building);
        selectorCompo.Add(building.GetComponent<BuildingSelector>());

        for (int i = 0; i < spawnGrid.Count; i++)
        {
            //이건 이미지 스파리트 이미지로 하고싶을때 -> Instantiate(_buildClone, spawnGrid[i].transform.position, Quaternion.identity, par.transform);
            Instantiate(_blockTilemap, spawnGrid[i].transform.position, Quaternion.identity, par.transform);
            Destroy(spawnGrid[i]);
        }
        spawnGrid.Clear();

        int childCount = par.transform.childCount;
        if (childCount == 0)
        {
            col.offset = Vector2.zero;
            return;
        }

        Vector2 localSum = Vector2.zero;
        for (int i = 0; i < childCount; i++)
        {
            Vector3 childWorld = par.transform.GetChild(i).position;
            Vector3 childLocal = par.transform.InverseTransformPoint(childWorld);
            localSum += (Vector2)childLocal;
        }

        Vector2 centerLocal = localSum / childCount;

        if (float.IsNaN(centerLocal.x) || float.IsNaN(centerLocal.y))
        {
            col.offset = Vector2.zero;
        }
        else
        {
            col.offset = centerLocal;
        }

        float yIf = width / maxW % 2 == 1 ? 0.5f : 0;
        float xIf = maxW % 2 == 1 ? 0f : -0.5f;
        GameObject ui = Instantiate(_buildingUI, buildingParent[buildingCount].transform);
        ui.GetComponentInChildren<TextMeshProUGUI>().text = $"{buildingSO.buildName}\n{buildingParent[buildingParent.Count - 1].NowMinion} / {buildingSO.maxMinion[0]}";
        ui.transform.position = new Vector3(transform.position.x + xIf,
            transform.position.y + width/maxW * 0.5f + yIf, 0);
        building.buildCount = buildingCount;
        buildingCount++;
        foreach (var item in buildingSO.resourceTypeCost)
        {
            ResourceManager.Instance.UseResource(item.resourceTypeSO, item.amount);
        }

    }
    private bool CanSpawn()
    {
        Vector2 center = boxCollider.bounds.center;
        Vector2 size = boxCollider.bounds.size;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f);

        foreach (var hit in hits)
        {
            if (hit.GetComponent<Building>() != null)
            {
                return false;
            }
        }

        return true;
    }
    private bool CanResourceAmount()
    {
        foreach (var item in buildingSO.resourceTypeCost)
        {
            int typeData = ResourceManager.Instance.resourceAmountDictionary[item.resourceTypeSO].Item1;
            if(typeData < item.amount)
            {
                return false;
            }
        }

        return true;
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



    public void BuildingMode()
    {
        if (!isMoveInv) return;
        CloseAllBuildUI(null);
        GridDestroy();
        isBuilding = false;
        showCollider = false;

        foreach (var parent in buildingParent)
        {
            parent.ShowCollider = !parent.ShowCollider;
        }
    }
    private bool BuildingSelect()
    {
        foreach (var item in selectorCompo)
        {
            if (item.isOpen)
            {
                if(buildingParent.Count != 0)
                {
                    if (item == buildingParent[selectCount].BuildingSelector)
                        return true;
                }

            }
        }
        return false;
    }
    private void BuildUISetting(bool isClose)
    {
        BuildTextSet();
        if (isClose)
        {
            _targetPos = new Vector2(_moveDistance, 0);
        }
        else
        {
            _targetPos = Vector2.zero;
        }
        _time += Time.deltaTime * _moveSpeed;
        _uiRectTransform.anchoredPosition = Vector3.Lerp(_uiRectTransform.anchoredPosition, _targetPos, _time);
    }
    private void BuildTextSet()
    {
        if (buildingParent.Count != 0 && Mouse.current.leftButton.wasPressedThisFrame && !isDestroing)
        {
            _buildNameTex.text = $"{buildingParent[selectCount].BuildingSO.buildName}";
            _buildHPTex.text = $"체력: {buildingParent[selectCount].nowHealth}";
            _levelTex.text = $"레벨: {buildingParent[selectCount].NowLevel}";
            _spawnKindTex.text = "자원:";
            if (buildingParent[selectCount].BuildingSO.levelResourceType.Length != 0)
                _spawnKindTex.text += buildingParent[selectCount].BuildingSO.levelResourceType.Length == 0 ? "생성안함" : " " + buildingParent[selectCount].BuildingSO.levelResourceType[buildingParent[selectCount].NowLevel - 1].resourceTypeSOs[0].resourceTypeSO.name + " +" + buildingParent[selectCount].BuildingSO.levelResourceType[buildingParent[selectCount].NowLevel - 1].resourceTypeSOs[0].amount + "/s";
        }
    }
    private void SelectButton()
    {
        isDestroing = true;
        _selectBtn.gameObject.SetActive(false);
        if (_nowSelect)
        {
            buildingParent[selectCount].BuildUpgrade();
        }
        else
        {
            for (int i = 0; i < buildingParent.Count; i++)
            {
                if (buildingParent[i].buildCount > selectCount)
                {
                    buildingParent[i].buildCount -= 1;
                }
            }
            buildingCount--;
            selectorCompo.Remove(buildingParent[selectCount].BuildingSelector);
            Destroy(buildingParent[selectCount].gameObject);
            buildingParent.Remove(buildingParent[selectCount]);
        }
        isDestroing = false;
    }
    private void UpgradeDestroy(bool isUpgrade)
    {
        _selectBtn.gameObject.SetActive(true);
        if (isUpgrade)
        {
            _nowSelect = true;
        }
        else
        {
            _nowSelect = false;
        }
    }



    public BuildingSO GetBuildData() => buildingSO;
    public void CloseAllBuildUI(Building me)
    {
        if(me == null)
        {
            foreach (var b in buildingParent)
                b.BuildingSelector.isOpen = false;
        }
        else
        {
            foreach (var b in buildingParent)
            {
                if (b != me)
                    b.BuildingSelector.isOpen = false;
                else
                    b.BuildingSelector.isOpen = true;
            }
        }
    }
    public void GetSelectData(int count, int level)
    {
        _time = 0;
        selectCount = count;
        selectLevel = level;
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

        lineRenderer.enabled = showCollider;
    }
    private void UpdateColliderView()
    {
        lineRenderer.enabled = showCollider;

        if (!showCollider || boxCollider == null) return;

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