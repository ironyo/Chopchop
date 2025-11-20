using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildingSelector : MonoBehaviour
{
    private Collider2D boxCollider;
    public bool isOpen = false;
    
    Building buildCompo;


    private void Start()
    {
        boxCollider = GetComponent<Collider2D>();
        buildCompo = GetComponent<Building>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && InventoryManager.Instance.IsNowClose)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint (Mouse.current.position.ReadValue());

            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            hit.gameObject.TryGetComponent<TilemapCollider2D>(out TilemapCollider2D tile);
            hit.gameObject.TryGetComponent<Building>(out Building build);

            if (hit != null && hit.isTrigger == false || tile != null)
            {
                if(build != null)
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
        }
    }
}
