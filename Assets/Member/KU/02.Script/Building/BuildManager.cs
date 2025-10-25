using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    [SerializeField] private List<GameObject> buildingParent = new();

    private List<GameObject> spawnGrid = new();
    private bool isBuilding;
    private BuildingSO buildingSO;
    private int buildingCount = 0;

    private Vector2 spawnPos;

    private BoxCollider2D boxCollider;

    private void Awake()
    {
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
        {
            _helpUI.SetActive(true);
        }
    }
    private void BuildOrCancle()
    {
        if (spawnGrid != null && Mouse.current.rightButton.wasPressedThisFrame && isBuilding)
        {
            isBuilding = false;
            GridDestroy();
        }
        if (spawnGrid != null && Mouse.current.leftButton.wasPressedThisFrame && isBuilding && !EventSystem.current.IsPointerOverGameObject())
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
            boxCollider.size = new Vector2(width-1, width / maxW+2);
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

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastCell = grid.WorldToCell(mouseWorldPos);
    }
    private void GridDestroy()
    {
        _helpUI.SetActive(false);
        for (int i = 0; i < spawnGrid.Count; i++)
        {
            Destroy(spawnGrid[i]);
        }
        spawnGrid.Clear();
    }
    private void BuildedClear()
    {
        if (!CanSpawn()) return;

        GameObject par = new GameObject(buildingSO.name);
        par.transform.parent = _buildingCanvus.transform;
        par.transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        buildingParent.Add(par);
        buildingParent[buildingParent.Count - 1].AddComponent<Building>().buildingSO = buildingSO;
        buildingParent[buildingParent.Count - 1].AddComponent<BoxCollider2D>();
        _helpUI.SetActive(false);
        isBuilding = false;
        for (int i = 0; i < spawnGrid.Count; i++)
        {
            Instantiate(_buildClone, spawnGrid[i].transform.position, Quaternion.identity, buildingParent[buildingCount].transform);
            Destroy(spawnGrid[i]);
        }
        spawnGrid.Clear();

        foreach (GameObject obj in spawnGrid)
        {
            obj.transform.SetParent(buildingParent[buildingCount].transform);
        }
        float yIf = width / maxW % 2 == 1 ? 0.5f : 0;
        float xIf = maxW % 2 == 1 ? 0f : -0.5f;
        GameObject ui = Instantiate(_buildingUI, buildingParent[buildingCount].transform);
        ui.GetComponentInChildren<TextMeshProUGUI>().text = $"{buildingSO.buildName}\n{buildingSO.minionCount} / {buildingSO.maxMinion}";
        ui.transform.position = new Vector3(transform.position.x + xIf,
            transform.position.y + width/maxW * 0.5f + yIf, 0);
        buildingCount++;
    }
    private bool CanSpawn()
    {
        Collider2D[] colid = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
        Building[] boxOnly = colid.OfType<Building>().ToArray();
        return boxOnly.Length == 0;
    }
}