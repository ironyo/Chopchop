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

        if (minionCount == 0) return 100;   // 미니언 없으면 당연히 정상

        // ---- 비율 계산 ----
        float ratio = (float)tileCount / minionCount;

        //  최소 비율 바닥(보정) — 미니언 많아도 ratio가 너무 떨어지지 않게
        float minSafeRatio = 1.5f;
        ratio = Mathf.Max(ratio, minSafeRatio);

        // ---- 튜닝 가능한 수치 ----
        float goodRatio = 6f;          // 적당
        float normalRatio = 20f;       // 보통
        float highRatio = 80f;         // 매우 넓음

        float sliderValue;

        //  비율이 낮을 때도 50 아래로 너무 급하게 떨어지지 않도록 완화
        if (ratio <= normalRatio)
        {
            float t = Mathf.InverseLerp(minSafeRatio, normalRatio, ratio);
            sliderValue = Mathf.Lerp(80f, 50f, t);   // 기존의 100 → 50이 너무 급함 → 80 → 50으로 완화
        }
        else
        {
            float t = Mathf.InverseLerp(normalRatio, highRatio, ratio);
            sliderValue = Mathf.Lerp(50f, 0f, t);    // 넓을수록 안정적
        }

        return Mathf.RoundToInt(Mathf.Clamp(sliderValue, 0f, 100f));
    }

}


