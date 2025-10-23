using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] private List<GameObject> newParent = new();

    private List<GameObject> spawnGrid = new();
    private bool isBuilding;
    private BuildingSO buildingSO;
    private int buildingCount = 0;

    private void Update()
    {
        if(spawnGrid != null && Mouse.current.rightButton.wasPressedThisFrame && isBuilding)
        {
            isBuilding = false;
            GridDestroy();
        }
        if (spawnGrid != null && Mouse.current.leftButton.wasPressedThisFrame && isBuilding && !EventSystem.current.IsPointerOverGameObject())
        {
            BuildedClear();
        }
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

    public void Buildings(bool idBuilding, BuildingSO buildSO)
    {
        isBuilding = idBuilding;
        if (isBuilding)
        {
            buildingSO = buildSO;
            GridDestroy();
            width = buildSO.width;
            maxW = buildSO.maxW;
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

            Vector3 spawnPos = new Vector3(baseX + offsetX, baseY + offsetY, 0f);

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
        
        GameObject par = new GameObject(buildingSO.name);
        par.transform.parent = GameObject.Find("BuildingUICanvas").transform;
        par.transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        newParent.Add(par);
        _helpUI.SetActive(false);
        isBuilding = false;
        for (int i = 0; i < spawnGrid.Count; i++)
        {
            Instantiate(_buildClone, spawnGrid[i].transform.position, Quaternion.identity, newParent[buildingCount].transform);
            Destroy(spawnGrid[i]);
        }
        spawnGrid.Clear();

        foreach (GameObject obj in spawnGrid)
        {
            obj.transform.SetParent(newParent[buildingCount].transform);
        }
        float yIf = width / maxW % 2 == 1 ? 0.5f : 0;
        float xIf = maxW % 2 == 1 ? 0f : -0.5f;
        GameObject ui = Instantiate(_buildingUI, newParent[buildingCount].transform);
        ui.transform.position = new Vector3(transform.position.x + xIf,
            transform.position.y + width/maxW * 0.5f + yIf, 0);
        buildingCount++;
    }
}