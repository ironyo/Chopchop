using DG.Tweening;
using Member.CHJ._02.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinionCounterSystem : MonoBehaviour
{
    [SerializeField] private Slider counterSlider;
    [SerializeField] private TextMeshProUGUI minionCounterTxt;

    [SerializeField] private GameObject OverloadTxt;
    [SerializeField] private GameObject MopeTxt;
    [SerializeField] private GameObject OverloadTxt_anim;
    [SerializeField] private GameObject MopeTxt_anim;

    private RectTransform handle;
    private float lastSliderVal;
    private Sequence rotateSequence;
    private Tweener sliderTween;

    private enum State { Normal, Mope, Bomb }
    private State currentState = State.Normal;

    private void Awake()
    {
        handle = counterSlider.targetGraphic.GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateOverloadSlider();
    }

    private void UpdateOverloadSlider()
    {
        int sliderVal = CalculateValue();

        minionCounterTxt.text = "미니언: " +
            (MinionManager.Instance ? MinionManager.Instance.minionList.Count : 0);

        //  하루 1회 트윈만 유지
        if (sliderTween != null && sliderTween.IsActive())
            sliderTween.Kill();

        sliderTween = counterSlider.DOValue(sliderVal, 0.3f)
                                   .SetEase(Ease.OutCubic);

        //  핸들 회전
        if (Mathf.Abs(sliderVal - lastSliderVal) >= 1f)
        {
            RotateHandleOnce(sliderVal > lastSliderVal);
            lastSliderVal = sliderVal;
        }

        //  슬라이더 상태에 따른 이벤트 (중복 발동 방지)
        if (sliderVal <= 25)
        {
            if (currentState != State.Mope)
            {
                currentState = State.Mope;
                GameEventManager.Instance.RunEvent(GameEventType.MinionMope);
                MopeTxt_anim.SetActive(true);
                MopeTxt.SetActive(false);
            }
        }
        else if (sliderVal >= 75)
        {
            if (currentState != State.Bomb)
            {
                currentState = State.Bomb;
                GameEventManager.Instance.RunEvent(GameEventType.MinionBomb);
                OverloadTxt.SetActive(false);
                OverloadTxt_anim.SetActive(true);
            }
        }
        else
        {
            if (currentState != State.Normal)
            {
                currentState = State.Normal;
                GameEventManager.Instance.StopEvent();
                OverloadTxt.SetActive(true);
                OverloadTxt_anim.SetActive(false);
                MopeTxt_anim.SetActive(false);
                MopeTxt.SetActive(true);
            }
        }
    }

    private void RotateHandleOnce(bool isIncrease)
    {
        float targetZ = isIncrease ? 160f : 200f;

        rotateSequence?.Kill();

        rotateSequence = DOTween.Sequence()
            .Append(handle.DORotate(new Vector3(0, 0, targetZ), 0.2f).SetEase(Ease.OutCubic))
            .Append(handle.DORotate(new Vector3(0, 0, 180f), 0.2f).SetEase(Ease.OutCubic));
    }

    private int CalculateValue()
    {
        if (!MapManager.Instance || !MinionManager.Instance)
            return 100;

        int tileCount = MapManager.Instance.GetTileCount();
        int minionCount = MinionManager.Instance.minionList.Count;

        if (minionCount == 0)
            return 100;

        float ratio = (float)tileCount / minionCount;
        float sliderValue;

        // 1:1 이하 → 100
        if (ratio <= 1f)
        {
            sliderValue = 100f;
        }
        // 1:40 이상 → 0
        else if (ratio >= 40f)
        {
            sliderValue = 0f;
        }
        else
        {
            // 1~20 → 100~50
            if (ratio <= 20f)
            {
                float t = Mathf.InverseLerp(1f, 20f, ratio);
                sliderValue = Mathf.Lerp(100f, 50f, t);
            }
            // 20~40 → 50~0
            else
            {
                float t = Mathf.InverseLerp(20f, 40f, ratio);
                sliderValue = Mathf.Lerp(50f, 0f, t);
            }
        }

        return Mathf.RoundToInt(sliderValue);
    }
}
