using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePref : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countTxt;
    [SerializeField] private TextMeshProUGUI countTxt_s;
    [SerializeField] private Image icon;
    [SerializeField] private Image icon_s;

    [SerializeField] float shakeValue = 10f;
    [SerializeField] float shakeTime = 0.1f;

    private Sequence shakeSeq;
    private ResourceTypeSO typeData;

    private Vector2 startPos1;
    private Vector2 startPos2;

    void Awake()
    {
        if (countTxt != null && countTxt_s != null)
        {
            startPos1 = countTxt.rectTransform.anchoredPosition;
            startPos2 = countTxt_s.rectTransform.anchoredPosition;
        }
    }

    public void Set(int count, Sprite icon, ResourceTypeSO type)
    {
        countTxt.text = count + " :";
        countTxt_s.text = count + " :";
        this.icon.sprite = icon;
        this.icon_s.sprite = icon;
        typeData = type;
    }

    public void UpdateCount(int count)
    {
        countTxt.text = count + " :";
        countTxt_s.text = count + " :";
    }

    public void UpdateResource(ResourceTypeSO type)
    {
        if (type != typeData) return;
        shakeSeq?.Kill();

        var rt1 = countTxt.rectTransform;
        var rt2 = countTxt_s.rectTransform;

        shakeSeq = DOTween.Sequence();

        shakeSeq.Append(rt1.DOAnchorPosY(startPos1.y + shakeValue, shakeTime))
                .Join(rt2.DOAnchorPosY(startPos2.y + shakeValue, shakeTime))

                .Append(rt1.DOAnchorPosY(startPos1.y - shakeValue, shakeTime))
                .Join(rt2.DOAnchorPosY(startPos2.y - shakeValue, shakeTime))

                .Append(rt1.DOAnchorPosY(startPos1.y, shakeTime))
                .Join(rt2.DOAnchorPosY(startPos2.y, shakeTime));
    }
}
