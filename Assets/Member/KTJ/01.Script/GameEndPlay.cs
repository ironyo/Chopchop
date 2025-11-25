using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum GameEndType
{
    GameClear, GameOver
}

public class GameEndPlay : MonoBehaviour
{
    public UnityEvent<GameEndType, string> OnGameEndEvent;
    private bool isGameEnded = false;

    [Header("UI Settings")]
    [SerializeField] private List<CanvasGroup> showUis = new List<CanvasGroup> ();
    [SerializeField] private Image Background;
    [SerializeField] private TextMeshProUGUI TitleTxt;
    [SerializeField] private TextMeshProUGUI SubTitleTxt;

    //private void Start()
    //{
    //    OnGameEndEvent.Invoke(GameEndType.GameClear, "성공");
    //}

    private void Awake()
    {
        OnGameEndEvent.AddListener((type, msg) =>
        {
            StartCoroutine(RunGameOverUI(type, msg));
        });
    }

    IEnumerator RunGameOverUI(GameEndType type, string endMessage)
    {
        if (isGameEnded) yield break;
        isGameEnded = true;

        if (type == GameEndType.GameClear)
        {
            TitleTxt.text = "-1000달성-";
            SubTitleTxt.text = "수고하셨습니다";
        }
        else
        {
            TitleTxt.text = "-게임오버-";
            SubTitleTxt.text = endMessage;
        }

        yield return Background.DOFade(1f, 0.5f).WaitForCompletion();

        Sequence seq = DOTween.Sequence();

        foreach (CanvasGroup group in showUis)
        {
            group.gameObject.SetActive(true);
            RectTransform rec = group.GetComponent<RectTransform>();

            float originY = rec.anchoredPosition.y;

            rec.anchoredPosition = new Vector2(rec.anchoredPosition.x, originY - 30f);
            group.alpha = 0f;

            seq.Append(group.DOFade(1f, 0.5f));
            seq.Join(rec.DOAnchorPosY(originY, 0.5f).SetEase(Ease.OutCubic));
            seq.AppendInterval(0.05f);
        }

        yield return seq.WaitForCompletion();
    }

}
