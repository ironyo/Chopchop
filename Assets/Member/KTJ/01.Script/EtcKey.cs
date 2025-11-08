using UnityEngine;
using UnityEngine.InputSystem;

public class EtcKey : MonoBehaviour
{
    [SerializeField] private PlayerInput inputSO;

    [Header("------")]
    [SerializeField] private ShowControlUI ShowControl;
    private void Awake()
    {
        inputSO.OnKeyPressed += KeyPressed;
        inputSO.OnKeyReleased += KeyReleased;
    }
    private void KeyPressed(Key key)
    {
        if (key == Key.P)
        {
            ShowControl.Show();
        }
    }

    private void KeyReleased(Key key)
    {
        if (key == Key.P)
        {
            ShowControl.Close();
        }
    }

}
