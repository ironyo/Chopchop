using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UIType
{
    Central,
    Shop,
    MapUpgrade
}

[System.Serializable]
public class UiClass
{
    public UIBase uibase;
    public UIType type;
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private List<UiClass> uiList;
    [SerializeField] private RectTransform toolInventory;
    [SerializeField] private RectTransform miniMap;

    [Header("Anchors")]
    [SerializeField] private float toolInvShownY = 0f;
    [SerializeField] private float toolInvHiddenY = -200f;
    [SerializeField] private float miniMapShownX = 0f;
    [SerializeField] private float miniMapHiddenX = 400f;
    [SerializeField] private float uiTweenDuration = 0.5f;

    private Dictionary<UIType, UIBase> uiDict;
    private UIBase currentUI;
    private bool isTransitioning = false;

    private void Awake()
    {
        Instance = this;
        uiDict = uiList.ToDictionary(x => x.type, x => x.uibase);

        foreach (var ui in uiDict.Values)
            ui.SetActiveImmediate(false);

        toolInventory.anchoredPosition = new Vector2(toolInventory.anchoredPosition.x, toolInvShownY);
        miniMap.anchoredPosition = new Vector2(miniMapShownX, miniMap.anchoredPosition.y);
    }

    public void Toggle(UIBase target)
    {
        if (isTransitioning) return;
        StartCoroutine(SwitchRoutine(target));
    }

    public void Toggle(UIType type)
    {
        if (!uiDict.TryGetValue(type, out var target)) return;
        Toggle(target);
    }

    private IEnumerator SwitchRoutine(UIBase target)
    {
        isTransitioning = true;

        if (currentUI == target)
        {
            yield return StartCoroutine(currentUI.Close());
            currentUI = null;

            toolInventory.DOKill(); miniMap.DOKill();
            toolInventory.DOAnchorPosY(toolInvShownY, uiTweenDuration);
            miniMap.DOAnchorPosX(miniMapShownX, uiTweenDuration);

            isTransitioning = false;
            yield break;
        }

        if (currentUI != null)
            yield return StartCoroutine(currentUI.Close());

        currentUI = target;

        toolInventory.DOKill(); miniMap.DOKill();
        toolInventory.DOAnchorPosY(toolInvHiddenY, uiTweenDuration);
        miniMap.DOAnchorPosX(miniMapHiddenX, uiTweenDuration);

        yield return StartCoroutine(currentUI.Open());

        isTransitioning = false;
    }
}