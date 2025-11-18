using DG.Tweening;
using NavMeshPlus.Components;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapBuilding : UIBase
{
    private Tilemap tilemap;
    [SerializeField] private Tilemap visualTilemap;

    [SerializeField] private RuleTile ruleTile;
    [SerializeField] private TextMeshProUGUI mapSizeTxt;
    [SerializeField] private TextMeshProUGUI miniMapSizeTxt;
    [SerializeField] private Slider TileSizeSlider;

    [SerializeField] private AudioClip MapSetSound;

    [SerializeField] private NavMeshSurface navMeshSurface;

    private int currentTileSIze = 2;
    private bool isBuildActivate = false;

    private CinemachineImpulseSource cis;

    private void Awake()
    {
        cis = GetComponent<CinemachineImpulseSource>();
        tilemap = MapManager.Instance.tilemap;
    }

    private void Update()
    {
        if (isBuildActivate)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2Int mousePos = new Vector2Int(
                    Mathf.FloorToInt(mouseWorldPos.x),
                    Mathf.FloorToInt(mouseWorldPos.y)
                );

            SetVisualTIle(mousePos);

            if (Input.GetMouseButtonDown(0))
            {

                SetTile(mousePos);
            }
        }
    }

    public void ActivateBuildMode()
    {
        isBuildActivate = true;
    }

    public void OnSliderValChanged() // �����̴� �� ��ȭ ����
    {
        currentTileSIze = (int)TileSizeSlider.value;
    }

    private int GetTileCount(Vector2Int anchor)
    {
        int setTileCount = 0;

        for (int x = 0; x < currentTileSIze; x++)
        {
            for (int y = 0; y < currentTileSIze; y++)
            {
                if (!visualTilemap.HasTile(new Vector3Int(anchor.x + x, anchor.y + y, 0)))
                {
                    setTileCount++; // 여기하는중
                }
            }
        }

        return setTileCount;
    }

    private void SetTile(Vector2Int anchor)
    {
        cis.GenerateImpulse();
        SoundManager.Instance.SFXPlay("MapSet", MapSetSound);

        int setTileCount = 0;

        for (int x = 0; x < currentTileSIze; x++)
        {
            for (int y = 0; y < currentTileSIze; y++)
            {
                tilemap.SetTile(new Vector3Int(anchor.x + x, anchor.y + y, 0), ruleTile);
            }
        }

        Debug.Log(GetTileCount(anchor));

        StartCoroutine(RebuildNavMeshNextFrame());
    }

    private IEnumerator RebuildNavMeshNextFrame()
    {
        yield return null; // �� ���⼭ 1������ ��� �� Tilemap Mesh ������Ʈ��
        navMeshSurface.BuildNavMesh();
    }



    private void SetVisualTIle(Vector2Int anchor)
    {
        visualTilemap.ClearAllTiles();

        for (int x = 0; x < currentTileSIze; x++)
        {
            for (int y = 0; y < currentTileSIze; y++)
            {
                visualTilemap.SetTile(new Vector3Int(anchor.x + x, anchor.y + y, 0), ruleTile);
            }
        }
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

        isBuildActivate = false;
        visualTilemap.ClearAllTiles();
    }
}
