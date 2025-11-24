using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightIconUI : MonoSingleton<DayNightIconUI>
{
    [SerializeField] private Image dayimage;
    [SerializeField] private Image nightimage;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI nightText;
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
        dayText.DOKill();
        nightText.DOKill();

        dayimage.DOFade(0, 0.3f);
        dayText.DOFade(0, 0.3f);
        nightimage.DOFade(1, 0.3f);
        nightText.DOFade(1, 0.3f);
    }

    private void ChangeToDay()
    {
        nightimage.DOKill();
        dayimage.DOKill();
        dayText.DOKill();
        nightText.DOKill();
        nightimage.DOFade(0, 0.3f);
        nightText.DOFade(0, 0.3f);

        dayText.DOFade(1, 0.3f);
        dayimage.DOFade(1, 0.3f);
    }
}
