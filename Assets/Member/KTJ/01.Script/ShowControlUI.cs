using UnityEngine;

public class ShowControlUI : MonoBehaviour
{
    [SerializeField] private GameObject ControlUI;

    public void Show()
    {
        ControlUI.SetActive(true);
    }

    public void Close()
    {
        ControlUI.SetActive(false);
    }
}
