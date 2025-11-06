using DG.Tweening;
using UnityEngine;

public class SceneChangeManager : MonoSingleton<SceneChangeManager>
{
    [SerializeField] private GameObject ChangePref;
    [SerializeField] private Transform Canvas;

    private Sequence seq;

    protected override void Awake()
    {
        seq = DOTween.Sequence();

        base.Awake();
        OnSceneEnd();
    }

    public void OnSceneStart()
    {
        seq?.Kill();
        seq = DOTween.Sequence();

        SceneChangePref changePref = Instantiate(ChangePref, Canvas).GetComponent<SceneChangePref>();
        seq.Append(changePref.TextGroup.DOFade(0, 1f));
        seq.Append(changePref.MoveObject.DOAnchorPosY(changePref.HidePosY, 1.5f));
    }
    public void OnSceneEnd()
    {
        seq?.Kill();
        seq = DOTween.Sequence();

        SceneChangePref changePref = Instantiate(ChangePref, Canvas).GetComponent<SceneChangePref>();

        changePref.MoveObject.anchoredPosition = new Vector2(0, changePref.HidePosY); // 초기위치로 초기화
        changePref.TextGroup.alpha = 0;

        seq.Append(changePref.MoveObject.DOAnchorPosY(0, 1.5f));
        seq.Append(changePref.TextGroup.DOFade(1f, 1f));
    }
}
