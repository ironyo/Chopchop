using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUpgrade : UIBase
{
    [SerializeField] private int addAmount = 2;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private RuleTile ruleTile;
    [SerializeField] private TextMeshProUGUI mapSizeTxt;
    [SerializeField] private TextMeshProUGUI miniMapSizeTxt;
    [SerializeField] private SpriteRenderer cameraIconSr;
    [SerializeField] private Camera minimapCamera;

    // �� ���
    private const float A = 0.5402986f;
    private const float P = 1.0409403f;
    private const float C = 0.7766182f;

    // ī�޶�/������ ����
    private const float BaseCameraSize = 5.5f; // currentX=8�� ��
    private const float BaseIconScale = 2f;

    public int currentX { get; private set; } = 8;

    public void Upgrade()
    {
        // TODO: �ڿ� üũ ����
        currentX += addAmount * 2;

        SetAllTiles(currentX);
        UpdateMapSizeTxt();
        AdjustCameraSize();
    }

    private void SetAllTiles(int value)
    {
        int half = value / 2;
        // ���� �̽��� �������� BoxFill ��� ���:
        // tilemap.BoxFill(null, ruleTile, -half, -half, half-1, half-1);
        for (int x = -half; x < half; x++)
            for (int y = -half; y < half; y++)
                tilemap.SetTile(new Vector3Int(x, y, 0), ruleTile);
    }

    private void UpdateMapSizeTxt()
    {
        string s = $"{currentX}x{currentX}";
        if (mapSizeTxt) mapSizeTxt.text = $"�������: {s}  �ִ� �̴Ͼ�: ����";
        if (miniMapSizeTxt) miniMapSizeTxt.text = s;
    }

    private void AdjustCameraSize()
    {
        float x = Mathf.Max(currentX, 1);
        float newSize = A * Mathf.Pow(x, P) + C;

        minimapCamera.orthographicSize = newSize;
        AdjustCameraIconScale(newSize);
    }

    private void AdjustCameraIconScale(float cameraSize)
    {
        float scaleFactor = cameraSize / BaseCameraSize; // ȭ��� ũ�� ������
        cameraIconSr.transform.localScale = Vector3.one * (BaseIconScale * scaleFactor);
    }

    public override IEnumerator OpenEffect()
    {
        var rt = toggleObject.GetComponent<RectTransform>();
        Tween t = UIBase.DoY(rt, 0f, 0.5f);
        yield return t.WaitForCompletion();
    }

    public override IEnumerator CloseEffect()
    {
        var rt = toggleObject.GetComponent<RectTransform>();
        Tween t = UIBase.DoY(rt, -400f, 0.5f);
        yield return t.WaitForCompletion();
    }
}
