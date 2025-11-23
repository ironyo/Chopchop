using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DayNightIconUI : MonoSingleton<DayNightIconUI>
{
    [SerializeField] private Image dayimage;
    [SerializeField] private Image nightimage;
    protected override void Awake()
    {
        base.Awake();
    }

    public void Check(int time)
    {
        bool currentT = time is > 19 and < 23;
        if (currentT)
            ChangeToDay();
        else
            ChangeToNight();
    }

    private void ChangeToNight()
    {
        nightimage.DOKill();
        dayimage.DOKill();
        dayimage.DOFade(0, 0.3f);
        nightimage.DOFade(1, 0.3f);
    }

    private void ChangeToDay()
    {
        nightimage.DOKill();
        dayimage.DOKill();
        nightimage.DOFade(0, 0.3f);
        dayimage.DOFade(1, 0.3f);
    }
}
