using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUpgrade : UIBase
{
    [SerializeField] private int addAmount;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private RuleTile ruleTIle;
    [SerializeField] private TextMeshProUGUI mapSizeTxt;
    [SerializeField] private TextMeshProUGUI miniMapSizeTxt;
    [SerializeField] private SpriteRenderer cameraIconSr;
    [SerializeField] private Camera MinimapCamera;

    // �� ���
    private const float A = 0.5402986f;
    private const float P = 1.0409403f;
    private const float C = 0.7766182f;

    // ī�޶� ũ�� �� ������ ���ذ�
    private const float baseCameraSize = 5.5f;  // currentX=8�� �� ����
    private const float baseIconScale = 1f;     // ������ �ʱ� ������(1��)

    public int currentX { get; private set; } = 8;

    public void Upgrade()
    {
        if (true) // �ڿ��� ����ϴٸ�
        {
            currentX += addAmount * 2;

            Debug.Log("currentX: " + currentX);

            SetAllTiles(currentX);
            UpdateMapSizeTxt();
            AdjustCameraSize();
        }
    }

    private void SetAllTiles(int value)
    {
        int amount = value / 2;
        for (int x = -amount; x < amount; x++)
        {
            for (int y = -amount; y < amount; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, ruleTIle);
            }
        }
    }

    private void UpdateMapSizeTxt()
    {
        string mapsize = currentX.ToString();
        mapSizeTxt.text = $"�������: {mapsize}x{mapsize} �ִ� �̴Ͼ�: ����";
        miniMapSizeTxt.text = $"{mapsize} x {mapsize}";
    }

    private void AdjustCameraSize()
    {
        float x = Mathf.Max(currentX, 1); // x=0 ����
        float newSize = A * Mathf.Pow(x, P) + C;

        // ī�޶� ������ ����
        MinimapCamera.orthographicSize = newSize;

        // ������ ũ�� �����ϰ� ����
        AdjustCameraIconScale(newSize);

        Debug.Log($"[CameraSize] currentX: {x}, newSize: {newSize:F2}");
    }

    private void AdjustCameraIconScale(float cameraSize)
    {
        // ���ذ���
        const float baseCameraSize = 5.5f;  // currentX=8�� ��
        const float baseScale = 2f;          // ������ �ʱ� ������

        // ī�޶� Ŀ������ �����ܵ� Ŀ���� (ȭ��� ũ�� ����)
        float scaleFactor = cameraSize / baseCameraSize;
        cameraIconSr.transform.localScale = Vector3.one * (baseScale * scaleFactor);
    }

}
