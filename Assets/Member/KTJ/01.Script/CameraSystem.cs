using DG.Tweening;
using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private CanvasGroup zoomUiGroup;
    [SerializeField] private TextMeshProUGUI zoomTxt;

    [SerializeField] private Volume globalVolume;
    private DepthOfField dof;

    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;

    private Vector3 moveDir;
    private Vector2 scroll;

    private Coroutine zoomCoroutine;

    private bool onFocusedBuilding = false;

    private float zoomValSave = 5f;
    private void Start()
    {
        globalVolume.profile.TryGet(out dof);
    }

    private void Update()
    {
        moveDir = new Vector3(playerInput.MoveDir.x, playerInput.MoveDir.y, 0);
        Movement();

        scroll = playerInput.Scroll;
        HandleZoom();
    }

    public void FocusOnBuilding(GameObject building)
    {
        onFocusedBuilding = true;
        cam.Target.TrackingTarget = building.transform;
        zoomValSave = cam.Lens.OrthographicSize;

        cam.Lens.OrthographicSize = 3.5f;
    }

    public void UnFocusOnBuilding()
    {
        onFocusedBuilding = false;
        cam.Target.TrackingTarget = this.transform;
        cam.Lens.OrthographicSize = zoomValSave;
    }

    private void Movement()
    {
        if (onFocusedBuilding == true) return;

        gameObject.transform.position += moveSpeed * moveDir * Time.deltaTime;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);
    }

    private void HandleZoom()
    {
        if (onFocusedBuilding == true) return;

        float zoom = scroll.y * zoomSpeed * Time.deltaTime;
        if (Mathf.Abs(zoom) > 0.001f)
        {
            // OrthographicSize º¯°æ
            cam.Lens.OrthographicSize = Mathf.Clamp(cam.Lens.OrthographicSize - zoom, 3f, 10f);
            zoomTxt.text = "x" + cam.Lens.OrthographicSize.ToString("F1");

            if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
            zoomCoroutine = StartCoroutine(ShowZoomUI());
        }
    }


    private IEnumerator ShowZoomUI()
    {
        zoomUiGroup.gameObject.SetActive(true);
        zoomUiGroup.DOFade(1f, 0.2f);

        dof.active = true;

        yield return new WaitForSeconds(1f);

        dof.active = false;

        zoomUiGroup.DOFade(0f, 0.2f).OnComplete(() =>
        {
            zoomUiGroup.gameObject.SetActive(false);
        });
    }

}
