using UnityEngine;

public class CursorSet : MonoBehaviour
{
    [SerializeField] private RectTransform cursorUI; // Ŀ�� �̹���
    [SerializeField] private float cursorScale = 1.5f; // Ŀ�� ũ�� ����

    void Start()
    {
        Cursor.visible = false; // �ý��� Ŀ�� ����
        cursorUI.localScale = Vector3.one * cursorScale;
    }
    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        cursorUI.position = mousePos;
    }
}
