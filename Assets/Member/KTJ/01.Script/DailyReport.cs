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

    Sequence seq;

    private void Start()
    {
        seq = DOTween.Sequence();
    }

    public void RunReport()
    {
        if (seq != null)
        {
            seq.Kill();
            seq = DOTween.Sequence();
        }
        CheckImage.gameObject.SetActive(false);
        reportTxt.text = "하루가 끝났습니다!\n\n오늘도 잘 버텨내셨군요.\n미니언들은 항상 당신에게 감사합니다.\nㅅㄱ";
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
