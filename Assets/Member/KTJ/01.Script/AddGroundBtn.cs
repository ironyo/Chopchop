using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AddGroundBtn : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private UnityEvent AddGroundEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        AddGroundEvent.Invoke();
    }

    //private void OnMouseDown()
    //{
    //    AddGroundEvent.Invoke();
    //}
}
