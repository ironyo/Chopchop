using UnityEngine;
using UnityEngine.UI;

public class MinionCounter : MonoBehaviour
{
    [SerializeField] private Slider counterSlider;
    private RectTransform handle;

    private float sliderYPos_up;
    private float sliderYPos_down;

    private void Awake()
    {
        handle = counterSlider.targetGraphic.GetComponent<RectTransform>();
        sliderYPos_down = handle.localPosition.y;
        handle.localPosition = new Vector3(handle.localPosition.x, handle.localPosition.y + 100, handle.localPosition.z);
        sliderYPos_up = handle.localPosition.y;
    }

    private void Start()
    {
        counterSlider.onValueChanged.AddListener(delegate { AA(); });
    }

    private void AA()
    {
        if (counterSlider.value < counterSlider.maxValue / 3)
        {
            handle.localPosition = new Vector3(handle.localPosition.x, sliderYPos_up + 25, handle.localPosition.z);
        }
        else
        {
            handle.localPosition = new Vector3(handle.localPosition.x, sliderYPos_down + 25, handle.localPosition.z);
        }
    }
}
