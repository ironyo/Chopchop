using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BuildingSelector : MonoBehaviour, IPointerClickHandler
{
    public bool isOpen { get; set; } = false;
    
    Building buildCompo;


    private void Start()
    {
        buildCompo = GetComponent<Building>();
    }

    private void Update()
    {
        if (!buildCompo.isNowBuilding)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && InventoryManager.Instance.IsNowClose && !EventSystem.current.IsPointerOverGameObject())
            {
               
            }
        }
    }
    public void OpenCloseUI()
    {
        if (buildCompo.isNowBuilding) return;

        if (isOpen)
        {
            isOpen = false;
            buildCompo.spr.sprite = buildCompo.buildingSO.buildSprite;
            BuildManager.Instance. cameraSystem.UnFocusOnBuilding();
        }
        else
        {
            BuildManager.Instance.CloseAllBuildUI(buildCompo);
            buildCompo.spr.sprite = buildCompo.buildingSO.buildSelcetSprite;
            BuildManager.Instance.cameraSystem.FocusOnBuilding(gameObject);
        }
        int count = buildCompo.buildCount;
        BuildManager.Instance.GetSelectData(count, buildCompo.level);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("sddssdds");


        if (!buildCompo.isNowBuilding)
        {
            if (InventoryManager.Instance.IsNowClose)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

                if (hits.Length == 0)
                    return;
                bool hasBuilding = false;

                foreach (var h in hits)
                {
                    if (h == null) continue;


                    h.TryGetComponent(out Building building);
                    if (building != null)
                    {
                        if (building.buildCount == buildCompo.buildCount)
                        {
                            hasBuilding = true;
                            break;
                        }
                    }
                }

                if (hasBuilding)
                {
                    OpenCloseUI();
                }
            }
        }
    }
}