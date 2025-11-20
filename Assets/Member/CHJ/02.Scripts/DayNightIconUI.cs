using UnityEngine;
using UnityEngine.UI;

public class DayNightIconUI : MonoSingleton<DayNightIconUI>
{
    [SerializeField] private Image _image;
    
    [SerializeField] private Sprite _day;
    [SerializeField] private Sprite _night;
    protected override void Awake()
    {
        base.Awake();
    }

    public void Check(string time)
    {
        if (time == "AM")
            _image.sprite = _day;
        else
            _image.sprite = _night;
    }
}
