using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

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

    private Dictionary<UIType, UIBase> uiDict;
    private UIBase currentUI;

    private void Awake()
    {
        Instance = this;
        uiDict = uiList.ToDictionary(x => x.type, x => x.uibase);

        CloseAll();
    }

    public void ToggleUI(UIBase uibase, bool isOpen)
    {
        if (isOpen) // 열린상태라면
        {
            if (currentUI = uibase) // 현재 ui가 눌린 버튼소속의 ui라며ㅑㄴ
            {
                currentUI.Close();
                currentUI = null;

                toolInventory.DOAnchorPosY(0, 0.5f);
                miniMap.DOAnchorPosX(0, 0.5f);
            }
        }
        else // 닫힌상태라면
        {
            CloseAll();

            currentUI = uibase;
            currentUI.Open();

            toolInventory.DOAnchorPosY(-200, 0.5f);
            miniMap.DOAnchorPosX(400, 0.5f);
        }
    }

    private void CloseAll()
    {
        foreach (var ui in uiDict.Values)
            ui.Close();
    }

    //public void togle(UIType type)
    //{
    //    if (currentUI == null)
    //    {
    //        toolInventory.DOAnchorPosY(-600, 0.5f);
    //    }
    //    currentUI?.Toggle();

    //    currentUI = uiDict[type];
    //    currentUI.Toggle();
    //}

    //public void CloseCurrent()
    //{
    //    toolInventory.DOAnchorPosY(-415, 0.5f);
    //    currentUI?.Toggle();
    //    currentUI = null;
    //}
}
