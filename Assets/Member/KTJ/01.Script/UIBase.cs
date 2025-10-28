using System.Net.NetworkInformation;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    [Header("Toggle UI Setting")]
    [SerializeField] private GameObject toggleObject;
    [SerializeField] private UIType uiType;
    private bool isOpen = false; // ÃÊ±â°ª: ´ÝÈû

    public void Open()
    {
        toggleObject.SetActive(true);
        isOpen = true;
    }
    public void Close()
    {
        toggleObject.SetActive(false);
        isOpen = false;
    }

    public void ToggleBtn()
    {
        UIManager.Instance.ToggleUI(this, isOpen);
    }
}
