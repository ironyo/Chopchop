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

    public void Check(string time)
    {
        if (time == "AM")
            ChangeToDay();
        else
            ChageToNight();
    }

    private void ChageToNight()
    {
        dayimage.DOFade(0, 0.3f);
        nightimage.DOFade(1, 0.3f);
    }

    private void ChangeToDay()
    {
        nightimage.DOFade(0, 0.3f);
        dayimage.DOFade(1, 0.3f);
    }
}
