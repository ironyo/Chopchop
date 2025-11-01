using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSelector : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);

            if (hit == boxCollider)
            {
                Debug.Log($"{gameObject.name} Å¬¸¯µÊ");
            }
        }
    }
}
