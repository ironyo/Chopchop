using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BuildManager : MonoBehaviour
{
    private Vector3Int currentCell;
    private Vector3Int lastCell;
    private int width;
    private int maxW = 3;

    [SerializeField] private Grid grid;

    [SerializeField] private GameObject _clone;
    [SerializeField] private GameObject _buildClone;
    [SerializeField] private GameObject _helpUI;
    [SerializeField] private GameObject _buildingUI;
    [SerializeField] private GameObject _buildingCanvus;

    [SerializeField] private List<Building> buildingParent = new();

    private List<GameObject> spawnGrid = new();
    private bool isBuilding;
    private BuildingSO buildingSO;
    private int buildingCount = 0;

    private Vector2 spawnPos;

    private BoxCollider2D boxCollider;

    public static BuildManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        BuildOrCancle();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        currentCell = grid.WorldToCell(mouseWorldPos);

        if (currentCell != lastCell)
        {
            Vector3 snappedPos = grid.GetCellCenterWorld(currentCell);
            snappedPos.z = 0;
            transform.position = snappedPos;
            lastCell = currentCell;
        }
        if (isBuilding)
            _helpUI.SetActive(true);
        else
            _helpUI.SetActive(false);
    }
    private void BuildOrCancle()
    {
        if (spawnGrid != null && Mouse.current.rightButton.wasPressedThisFrame && isBuilding)
        {
            isBuilding = false;
            GridDestroy();
        }
        if (spawnGrid != null && Mouse.current.leftButton.wasPressedThisFrame && isBuilding /*&& !EventSystem.current.IsPointerOverGameObject()*/)
        {
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
        if (!CanSpawn()) return;

        //foreach (var item in buildingSO.resourceTypeCost)
        //{
        //    if(item.amount > 현재 자원)
        //    {
        //        return;
        //    }
        //}
        //foreach (var item in buildingSO.resourceTypeCost)
        //{
        //    필요 자원 만큼 현재 자원에서 감소
        //}


        isBuilding = false;
        GameObject par = new GameObject(buildingSO.name);
        par.transform.parent = _buildingCanvus.transform;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        par.transform.position = mousePos;

        Building building = par.AddComponent<Building>();
        building.buildingSO = buildingSO;
        BoxCollider2D col = par.AddComponent<BoxCollider2D>();
        buildingParent.Add(building);

        for (int i = 0; i < spawnGrid.Count; i++)
        {
            Instantiate(_buildClone, spawnGrid[i].transform.position, Quaternion.identity, par.transform);
            Destroy(spawnGrid[i]);
        }
        spawnGrid.Clear();

        int childCount = par.transform.childCount;
        if (childCount == 0)
        {
            Debug.LogWarning($"BuildedClear: No children were created for {par.name}. Aborting collider offset setup.");
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
            Debug.LogError("Computed centerLocal is NaN — skipping collider offset assignment.");
            col.offset = Vector2.zero;
        }
        else
        {
            col.offset = centerLocal;
        }

        float yIf = width / maxW % 2 == 1 ? 0.5f : 0;
        float xIf = maxW % 2 == 1 ? 0f : -0.5f;
        GameObject ui = Instantiate(_buildingUI, buildingParent[buildingCount].transform);
        ui.GetComponentInChildren<TextMeshProUGUI>().text = $"{buildingSO.buildName}\n{buildingParent[buildingParent.Count - 1].NowMinion} / {buildingSO.maxMinion}";
        ui.transform.position = new Vector3(transform.position.x + xIf,
            transform.position.y + width/maxW * 0.5f + yIf, 0);
        buildingCount++;
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

    public BuildingSO GetBuildData()
    {
        return buildingSO;
    }
}