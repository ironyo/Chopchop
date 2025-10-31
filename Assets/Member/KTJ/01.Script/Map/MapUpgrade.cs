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

    // 모델 계수
    private const float A = 0.5402986f;
    private const float P = 1.0409403f;
    private const float C = 0.7766182f;

    // 카메라 크기 및 스케일 기준값
    private const float baseCameraSize = 5.5f;  // currentX=8일 때 기준
    private const float baseIconScale = 1f;     // 아이콘 초기 스케일(1배)

    public int currentX { get; private set; } = 8;

    public void Upgrade()
    {
        if (true) // 자원이 충분하다면
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
        mapSizeTxt.text = $"현재면적: {mapsize}x{mapsize} 최대 미니언: 미정";
        miniMapSizeTxt.text = $"{mapsize} x {mapsize}";
    }

    private void AdjustCameraSize()
    {
        float x = Mathf.Max(currentX, 1); // x=0 방지
        float newSize = A * Mathf.Pow(x, P) + C;

        // 카메라 사이즈 조정
        MinimapCamera.orthographicSize = newSize;

        // 아이콘 크기 일정하게 보정
        AdjustCameraIconScale(newSize);

        Debug.Log($"[CameraSize] currentX: {x}, newSize: {newSize:F2}");
    }

    private void AdjustCameraIconScale(float cameraSize)
    {
        // 기준값들
        const float baseCameraSize = 5.5f;  // currentX=8일 때
        const float baseScale = 2f;          // 아이콘 초기 스케일

        // 카메라 커질수록 아이콘도 커지게 (화면상 크기 유지)
        float scaleFactor = cameraSize / baseCameraSize;
        cameraIconSr.transform.localScale = Vector3.one * (baseScale * scaleFactor);
    }

}
