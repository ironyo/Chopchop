using DG.Tweening;
using System.Collections;
using UnityEngine;

public class InventoryUI : UIBase
{

    public override IEnumerator CloseEffect()
    {
        var rt = toggleObject.GetComponent<RectTransform>();
        Tween t = rt.DOAnchorPosY(0, 0.5f);
        BuildManager.Instance.BuildingMode();
        InventoryManager.Instance.CloseInv();
        yield return t.WaitForCompletion();
    }

    public override IEnumerator OpenEffect()
    {
        var rt = toggleObject.GetComponent<RectTransform>();
        Tween t = rt.DOAnchorPosY(490, 0.5f);
        BuildManager.Instance.BuildingMode();
        InventoryManager.Instance.CloseInv();
        yield return t.WaitForCompletion();


    }
}
