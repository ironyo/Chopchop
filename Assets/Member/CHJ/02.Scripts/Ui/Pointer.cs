using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIPointerAlpha : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 15f;
    public float borderOffset = 0.05f;
    public GameObject foundEffectPrefab;

    private RectTransform _rect;
    private Canvas _canvas;
    private Camera _cam;
    private CanvasGroup _cg;
    private float baseOrthoSize;
    private bool isFound;

    void Start()
    {
        try
        {
            target = GameObject.FindWithTag("HQ").transform;
        }
        catch (Exception e)
        {
            throw;
        }
        _rect = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
        _cam = Camera.main;
        _cg = GetComponent<CanvasGroup>();
        if (_cg == null) _cg = gameObject.AddComponent<CanvasGroup>();

        if (_cam.orthographic)
            baseOrthoSize = _cam.orthographicSize;

        _rect.SetAsLastSibling();
    }

    void Update()
    {
        if (target == null) return;

        Vector3 vp = _cam.WorldToViewportPoint(target.position);
        bool targetVisible = vp.z > 0 && vp.x > 0 && vp.x < 1 && vp.y > 0 && vp.y < 1;

        // 화면 안 → 포인터 숨기기 + 연출
        if (targetVisible)
        {
            if (_cg.alpha > 0f)
            {
                _cg.DOFade(0f, 0.2f);
                PlayFoundEffect();
            }
        }
        else
        {
            if (_cg.alpha < 1f) _cg.DOFade(1f, 0.1f);
            MovePointerToScreenEdge();
            isFound = false;
        }
    }

    private void MovePointerToScreenEdge()
    {
        Vector2 screenPos = _cam.WorldToScreenPoint(target.position);
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 localPoint);

        localPoint /= canvasRect.localScale.x;

        float halfW = canvasRect.rect.width * 0.5f;
        float halfH = canvasRect.rect.height * 0.5f;

        localPoint.x = Mathf.Clamp(localPoint.x, -halfW + borderOffset * canvasRect.rect.width, halfW - borderOffset * canvasRect.rect.width);
        localPoint.y = Mathf.Clamp(localPoint.y, -halfH + borderOffset * canvasRect.rect.height, halfH - borderOffset * canvasRect.rect.height);

        _rect.anchoredPosition = Vector2.Lerp(_rect.anchoredPosition, localPoint, Time.deltaTime * moveSpeed);

        Vector2 dir = screenPos - RectTransformUtility.WorldToScreenPoint(null, _rect.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _rect.localRotation = Quaternion.Euler(0, 0, angle + 90);
    }

    private void PlayFoundEffect()
    {
        if (foundEffectPrefab == null) return;
        if(isFound) return;
        isFound = true;
        GameObject effect = Instantiate(foundEffectPrefab, _canvas.transform);
        RectTransform effectRect = effect.GetComponent<RectTransform>();

        Vector2 screenPos = _cam.WorldToScreenPoint(target.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.GetComponent<RectTransform>(), screenPos, null, out Vector2 localPoint);
        localPoint /= _canvas.transform.localScale.x;
        effectRect.anchoredPosition = localPoint;

        Image cg = effect.GetComponent<Image>();

        effectRect.DOLocalMoveY(effectRect.localPosition.y + 120f, 0.8f).SetEase(Ease.OutCubic);
        cg.DOFade(0f, 0.8f).OnComplete(() => Destroy(effect));
    }
}
