using DG.Tweening;
using NavMeshPlus.Components;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
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
    [SerializeField] private AudioClip MapSetSound;

    [SerializeField] private NavMeshSurface navMeshSurface;

    [SerializeField] private Transform NextTileGroundEffect;

    [SerializeField] private TextMeshPro costText;

    [SerializeField] private ResourceTypeSO useResource;

    private int currentTileSIze = 8;

    private CinemachineImpulseSource cis;

    private int currentTileCost = 8;

    // 시작 좌표
    private Vector3Int anchorPos = new Vector3Int(-4, -4, 0);

    // 달팽이 이동 방향 (Left → Up → Right → Down)
    private Vector3Int[] dirs = new Vector3Int[]
    {
        new Vector3Int(-8, 0, 0),  // Left
        new Vector3Int(0, 8, 0),   // Up
        new Vector3Int(8, 0, 0),   // Right
        new Vector3Int(0, -8, 0),  // Down
    };

    private int dirIdx = 0;    // 현재 방향 인덱스

    private int step = 1;      // 현재 방향으로 이동해야 하는 칸 수
    private int stepCnt = 0;   // step은 방향 2번 바뀔 때마다 증가
    private int moved = 0;     // 현재 방향에서 이동한 칸 수

    private void Awake()
    {
        cis = GetComponent<CinemachineImpulseSource>();
        tilemap = MapManager.Instance.tilemap;
    }

    private void Update()
    {
        // UI 위면 미리보기 금지
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // 다음 타일이 찍힐 위치 계산
        Vector3Int nextPos = anchorPos + dirs[dirIdx];

        // VisualTile 갱신
        SetVisualTile(nextPos);
    }

    public void SetTile()
    {
        if (TutorialManager.Instance != null)
        {
            if (TutorialManager.Instance.GetCurrentStepId() != "mapBuilding") return;
            else
            {
                TutorialManager.Instance.CompleteCurrentStepExternally();
                InvasionManager.Instance.Invasion();
            }
        }

        if (ResourceManager.Instance.UseResource(useResource, currentTileCost) == false)
        {
            return;
        }
        // 현재 방향으로 1칸 이동
        anchorPos += dirs[dirIdx];
        moved++;

        currentTileCost += 8;

        // 실제 타일 생성
        cis.GenerateImpulse();
        SoundManager.Instance.SFXPlay("MapSet", MapSetSound);

        for (int x = 0; x < currentTileSIze; x++)
        {
            for (int y = 0; y < currentTileSIze; y++)
            {
                tilemap.SetTile(
                    new Vector3Int(anchorPos.x + x, anchorPos.y + y, 0),
                    ruleTile
                );
            }
        }

        StartCoroutine(RebuildNavMeshNextFrame());

        // 이동한 칸 수가 step만큼이면 방향 전환
        if (moved >= step)
        {
            moved = 0;

            // 방향 인덱스 증가 후 순환
            dirIdx = (dirIdx + 1) % 4;

            // 두 번 전환되면 step 증가
            stepCnt++;
            if (stepCnt >= 2)
            {
                stepCnt = 0;
                step++;
            }
        }

        // SetTile 후에도 VisualTile은 다음 위치로 자동 이동되도록 보정
        Vector3Int nextAnchor = anchorPos + dirs[dirIdx];
        NextTileGroundEffect.position += dirs[dirIdx];
        SetVisualTile(nextAnchor);
    }

    private IEnumerator RebuildNavMeshNextFrame()
    {
        yield return null;
        navMeshSurface.BuildNavMesh();
    }

    private void SetVisualTile(Vector3Int anchor)
    {
        visualTilemap.ClearAllTiles();

        costText.text = "흙("+currentTileCost.ToString()+")";

        for (int x = 0; x < currentTileSIze; x++)
        {
            for (int y = 0; y < currentTileSIze; y++)
            {
                visualTilemap.SetTile(
                    new Vector3Int(anchor.x + x, anchor.y + y, 0),
                    ruleTile
                );
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

        visualTilemap.ClearAllTiles();
    }
}
