using UnityEngine;
using UnityEngine.Events;

public class AddGroundBtn : MonoBehaviour
{
    [SerializeField] private UnityEvent AddGroundEvent;
    private void OnMouseDown()
    {
        AddGroundEvent.Invoke();
    }
}
