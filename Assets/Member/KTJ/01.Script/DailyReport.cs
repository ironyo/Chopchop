using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyReport : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private TextMeshProUGUI reportTxt;
    [SerializeField] private GameObject CheckImage;

    [Header("Animation")]
    [SerializeField] private RectTransform Anchor;
    [SerializeField] private RectTransform Base_front;
    [SerializeField] private RectTransform Base_back;
    [SerializeField] private Image Stamp;

    public UnityEvent OnDailyReport;

    Sequence seq;

    private void Awake()
    {
        seq = DOTween.Sequence();
        OnDailyReport.AddListener(RunReport);
    }

    private void Start()
    {
    }

    private void RunReport()
    {
        if (seq != null)
        {
            seq.Kill();
            seq = DOTween.Sequence();
        }
        CheckImage.gameObject.SetActive(false);
        reportTxt.text = "오늘도 수고하셨습니다! 보고 드립니다.\n미니언들의 상태는 양호함.\n1개의 민원이 들어왔음.\n날씨는 매우 화창함\n\n본부장 김철수 드림";
        seq.Append(Anchor.DOAnchorPosY(0f, 1f));
        seq.Insert(0.2f ,Base_front.DOAnchorPosY(0f, 1f).SetEase(Ease.OutBack));
    }

    public void OnComfilm()
    {
        if (seq != null)
        {
            seq.Kill();
            seq = DOTween.Sequence();
        }
        seq.Append(Stamp.rectTransform.DOAnchorPosX(300, 0.5f));
        seq.Join(Stamp.DOFade(1f, 0.5f));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() =>
        {
            CheckImage.gameObject.SetActive(true);
        });
        seq.Append(Stamp.rectTransform.DOScale(0.8f, 0.5f));
        seq.AppendInterval(0.7f);
        seq.Append(Stamp.rectTransform.DOScale(1f, 0.5f));
        seq.AppendInterval(0.2f);
        seq.Append(Stamp.rectTransform.DOAnchorPosX(560, 0.5f));
        seq.Join(Stamp.DOFade(0f, 0.5f));
        seq.AppendInterval(0.5f);
        seq.Append(Anchor.DOAnchorPosY(-835f, 1f));
        seq.Append(Base_front.DOAnchorPosY(-172f, 1f).SetEase(Ease.InQuad));
    }
}
