using UnityEngine;
using UnityEngine.EventSystems;

public class MinionInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject minionHighlight;
    [SerializeField] private string[] randomMessage;
    private MinionChat chat;

    private void Awake()
    {
        chat = GetComponent<MinionChat>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("미니언 클릭");
        if (ToolManager.Instance.UseTool(gameObject) == false)
        {
            chat.AddMessage(randomMessage[Random.Range(0, randomMessage.Length)]);
        }
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
