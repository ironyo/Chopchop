using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DestroyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private GameObject fillImage;
    [SerializeField] private float holdTime = 2f;

    private bool isHolding = false;
    private float timer = 0f;

    private void Update()
    {
        if (isHolding)
        {
            timer += Time.deltaTime;
            float fill = timer / holdTime;
            fillImage.transform.localScale = new Vector2(fill, fillImage.transform.localScale.y);
            if (fill >= 1f)
            {
                BuildManager.Instance.SelectButton(false);
                isHolding = false;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        timer = 0f;
        fillImage.transform.localScale = new Vector3(0, 1, 1);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetFill();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetFill();
    }

    private void ResetFill()
    {
        isHolding = false;
        timer = 0f;
        fillImage.transform.localScale = new Vector3(0, 1, 1);
    }
}
