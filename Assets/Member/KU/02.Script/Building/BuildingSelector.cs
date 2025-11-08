using UnityEngine;
using UnityEngine.InputSystem;

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
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint (Mouse.current.position.ReadValue());

            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit != null && hit == boxCollider && hit.isTrigger == false)
            {
                Debug.Log(hit);
                isOpen = !isOpen;
                int count = buildCompo.buildCount;
                Debug.Log(count);
                BuildManager.Instance.GetSelectData(count);
            }
        }
    }
}
