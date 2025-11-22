using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UseToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    [SerializeField] private string tip;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTip.Instance.Show(tip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTip.Instance.Hide();
    }
}
