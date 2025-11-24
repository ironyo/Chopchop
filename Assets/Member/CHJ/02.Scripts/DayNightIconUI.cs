using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightIconUI : MonoSingleton<DayNightIconUI>
{
    [SerializeField] private CanvasGroup dayimage;
    [SerializeField] private CanvasGroup nightimage;
    protected override void Awake()
    {
        base.Awake();
    }

    public void Check(int time)
    {
        bool currentT = time is >= 19 and <=24;
        if (currentT)
            ChangeToNight();
        else
            ChangeToDay();
    }

    private void ChangeToNight()
    {


        dayimage.DOFade(0, 0.3f);
        nightimage.DOFade(1, 0.3f);

        Debug.Log("¹ã");
    }

    private void ChangeToDay()
    {

        nightimage.DOFade(0, 0.3f);
        dayimage.DOFade(1, 0.3f);

        Debug.Log("³·");
    }
}