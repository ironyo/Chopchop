using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildingSelector : MonoBehaviour
{
    public bool isOpen { get; set; } = false;
    
    Building buildCompo;


    private void Start()
    {
        buildCompo = GetComponent<Building>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && InventoryManager.Instance.IsNowClose)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint (Mouse.current.position.ReadValue());

            Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

            if (hits.Length == 0)
                return;
            bool hasBuilding = false;

            foreach (var h in hits)
            {
                if (h == null) continue;


                h.TryGetComponent(out Building building);
                if (building.buildCount == buildCompo.buildCount)
                {
                    hasBuilding = true;
                    Debug.LogError($"Hit: {h.name}");
                    break;
                }
            }

            if (hasBuilding)
            {
                OpenCloseUI();
            }
        }
    }
    public void OpenCloseUI()
    {
        if (isOpen)
        {
            isOpen = false;
            buildCompo.spr.sprite = buildCompo.buildingSO.buildSprite;
        }
        else
        {
            BuildManager.Instance.CloseAllBuildUI(buildCompo);
            buildCompo.spr.sprite = buildCompo.buildingSO.buildSelcetSprite;
        }
        int count = buildCompo.buildCount;
        BuildManager.Instance.GetSelectData(count, buildCompo.level);
    }
}