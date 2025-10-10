using UnityEngine;

public class CursorSet : MonoBehaviour
{
    [SerializeField] private RectTransform cursorUI; // 커서 이미지
    [SerializeField] private float cursorScale = 1.5f; // 커서 크기 배율

    void Start()
    {
        Cursor.visible = false; // 시스템 커서 숨김
        cursorUI.localScale = Vector3.one * cursorScale;
    }
    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        cursorUI.position = mousePos;
    }
}
