using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Febucci.UI;
using Member.CHJ._02.Scripts;

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

    private void Update()
    {
        UpdateOverloadSlider();
    }

    private void UpdateOverloadSlider()
    {
        int sliderVal = CalculateValue();
        minionCounterTxt.text = "미니언: " + MinionManager.Instance.minionList.Count.ToString();
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
        int minionCount = MinionManager.Instance.minionList.Count;
        //int minionCount = 100;

        if (minionCount == 0)
            return 0;

        // �̴Ͼ� 1������ Ÿ�� ���� ����
        float ratio = (float)tileCount / minionCount;

        // ������ ����
        float minRatio = 4f;     // ���� �� 100
        float midRatio = 32f;    // ���� �� 50
        float maxRatio = 100f;   // ��� �� 0

        float sliderValue;

        if (ratio <= midRatio)
        {
            // 4~32 ����: 100 �� 50
            float t = Mathf.InverseLerp(minRatio, midRatio, ratio);
            sliderValue = Mathf.Lerp(100f, 50f, t);
        }
        else
        {
            // 32~100 ����: 50 �� 0
            float t = Mathf.InverseLerp(midRatio, maxRatio, ratio);
            sliderValue = Mathf.Lerp(50f, 0f, t);
        }

        return Mathf.RoundToInt(Mathf.Clamp(sliderValue, 0f, 100f));
    }

}
