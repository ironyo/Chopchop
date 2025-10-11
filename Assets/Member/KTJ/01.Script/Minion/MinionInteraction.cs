using UnityEngine;
using UnityEngine.EventSystems;

public class MinionInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject minionHighlight;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("�̴Ͼ� Ŭ��");
        ToolManager.Instance.UseTool(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        minionHighlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        minionHighlight.SetActive(false);
    }
}
