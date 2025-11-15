using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Febucci.UI;

public class MinionCounterSystem : MonoBehaviour
{
    [SerializeField] private Slider counterSlider;
    [SerializeField] private TextMeshProUGUI minionCounterTxt;

    [SerializeField] private GameObject OverloadTxt;
    [SerializeField] private GameObject MopeTxt;
    [SerializeField] private GameObject OverloadTxt_anim;
    [SerializeField] private GameObject MopeTxt_anim;

    private RectTransform handle;
    
    private float sliderYPos_up;
    private float sliderYPos_down;

    private void Awake()
    {
        handle = counterSlider.targetGraphic.GetComponent<RectTransform>();
        sliderYPos_down = handle.localPosition.y;
        sliderYPos_up = handle.localPosition.y + 100;

        SetHandlePosY();
    }

    private void Start()
    {
        counterSlider.onValueChanged.AddListener(delegate { SetHandlePosY(); });
    }

    private void SetHandlePosY()
    {
        if (counterSlider.value < counterSlider.maxValue / 2.5)
        {
            handle.localPosition = new Vector3(handle.localPosition.x, sliderYPos_up, handle.localPosition.z);
        }
        else
        {
            handle.localPosition = new Vector3(handle.localPosition.x, sliderYPos_down, handle.localPosition.z);
        }
    }

    private void Update()// 임시로 업데이트에서 호출함
    {
        UpdateOverloadSlider();
    }

    private void UpdateOverloadSlider()
    {
        int sliderVal = CalculateValue();
        minionCounterTxt.text = "미니언: " + TestMinionManager.Instance.alivesMinions.Count.ToString();
        counterSlider.value = sliderVal;

        if (counterSlider.value <= 25)
        {
            GameEventManager.Instance.RunEvent(GameEventType.MinionMope);
            MopeTxt_anim.SetActive(true);
            MopeTxt.SetActive(false);
        }
        else if (counterSlider.value >= 75)
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

    private int CalculateValue()
    {
        int tileCount = MapManager.Instance.GetTileCount();
        int minionCount = TestMinionManager.Instance.alivesMinions.Count; // TODO: 실제 미니언 매니저에서 가져오기
        //int minionCount = 100; // x테스트용 임시

        if (minionCount == 0)
            return 0;

        // 미니언 1마리당 타일 개수 비율
        float ratio = (float)tileCount / minionCount;

        // 기준점 설정
        float minRatio = 4f;     // 조밀 → 100
        float midRatio = 32f;    // 정상 → 50
        float maxRatio = 100f;   // 희박 → 0

        float sliderValue;

        if (ratio <= midRatio)
        {
            // 4~32 사이: 100 → 50
            float t = Mathf.InverseLerp(minRatio, midRatio, ratio);
            sliderValue = Mathf.Lerp(100f, 50f, t);
        }
        else
        {
            // 32~100 사이: 50 → 0
            float t = Mathf.InverseLerp(midRatio, maxRatio, ratio);
            sliderValue = Mathf.Lerp(50f, 0f, t);
        }

        return Mathf.RoundToInt(Mathf.Clamp(sliderValue, 0f, 100f));
    }

}
