using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Febucci.UI;
using Member.CHJ._02.Scripts;
using UnityEngine.Rendering;

public class MinionCounterSystem : MonoBehaviour
{
    [SerializeField] private Slider counterSlider;
    [SerializeField] private TextMeshProUGUI minionCounterTxt;

    [SerializeField] private GameObject OverloadTxt;
    [SerializeField] private GameObject MopeTxt;
    [SerializeField] private GameObject OverloadTxt_anim;
    [SerializeField] private GameObject MopeTxt_anim;

    private RectTransform handle;
    private float lastSliderVal;           // 실제로 회전 처리 기준 값
    private Sequence rotateSequence;

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
        minionCounterTxt.text = "미니언: " + MinionManager.Instance.minionList.Count;

        // 슬라이더 부드럽게 이동
        counterSlider.DOValue(sliderVal, 0.3f).SetEase(Ease.OutCubic);

        if (Mathf.Abs(sliderVal - lastSliderVal) >= 1f)
        {
            bool isIncrease = sliderVal > lastSliderVal;
            RotateHandleOnce(isIncrease);
            lastSliderVal = sliderVal;
        }

        if (sliderVal <= 25)
        {
            GameEventManager.Instance.RunEvent(GameEventType.MinionMope);
            MopeTxt_anim.SetActive(true);
            MopeTxt.SetActive(false);
        }
        else if (sliderVal >= 75)
        {
            GameEventManager.Instance.RunEvent(GameEventType.MinionBomb);
            OverloadTxt.SetActive(false);
            OverloadTxt_anim.SetActive(true);
        }
        else
        {
            GameEventManager.Instance.StopEvent();
            OverloadTxt.SetActive(true);
            OverloadTxt_anim.SetActive(false);
            MopeTxt_anim.SetActive(false);
            MopeTxt.SetActive(true);
        }
    }

    private void RotateHandleOnce(bool isIncrease)
    {
        float targetZ = isIncrease ? 160f : 200f;

        if (rotateSequence != null && rotateSequence.IsActive())
            rotateSequence.Kill();

        rotateSequence = DOTween.Sequence();
        rotateSequence.Append(handle.DORotate(new Vector3(0, 0, targetZ), 0.2f).SetEase(Ease.OutCubic));
        rotateSequence.Append(handle.DORotate(new Vector3(0, 0, 180f), 0.2f).SetEase(Ease.OutCubic));
    }

    private int CalculateValue()
    {
        int tileCount = MapManager.Instance.GetTileCount();
        int minionCount = MinionManager.Instance.minionList.Count;

        if (minionCount == 0) return 0;

        float ratio = (float)tileCount / minionCount;

        float minRatio = 4f;
        float midRatio = 32f;
        float maxRatio = 200f;

        float sliderValue;

        if (ratio <= midRatio)
        {
            float t = Mathf.InverseLerp(minRatio, midRatio, ratio);
            sliderValue = Mathf.Lerp(100f, 50f, t);
        }
        else
        {
            float t = Mathf.InverseLerp(midRatio, maxRatio, ratio);
            sliderValue = Mathf.Lerp(50f, 0f, t);
        }

        return Mathf.RoundToInt(Mathf.Clamp(sliderValue, 0f, 100f));
    }
}


