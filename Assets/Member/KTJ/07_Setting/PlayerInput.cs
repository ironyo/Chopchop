using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static Controls;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "Scriptable Objects/PlayerInput")]
public class PlayerInput : ScriptableObject, IPlayerActions
{
    private Controls _controls;
    public Vector2 MoveDir { get; private set; }
    public Vector2 Scroll {  get; private set; }

    public event Action<Key> OnKeyPressed;
    public event Action<Key> OnKeyReleased;
    public event Action<int> OnItemInvenKeyReleased;
    private void OnEnable() 
    {
        if (_controls == null)
        {
            _controls = new Controls();
        }
        _controls.Player.SetCallbacks(this);
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveDir = context.ReadValue<Vector2>();
    }
    public void OnZoom(InputAction.CallbackContext context)
    {
        Scroll = context.ReadValue<Vector2>();
    }

    public void OnKeyboardEtc(InputAction.CallbackContext context)
    {
        if (context.performed) // ������ ���� ȣ��
        {
            // � Ű�� ���ȴ��� ��������
            if (context.control is KeyControl keyControl)
            {
                OnKeyPressed?.Invoke(keyControl.keyCode);
            }
        }
        else if (context.canceled)
        {
            if (context.control is KeyControl keyControl)
            {
                OnKeyReleased?.Invoke(keyControl.keyCode);
            }
        }
    }
    public void OnItemGive(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // � Ű�� ���ȴ��� Ȯ��
        string keyName = context.control.name;  // "1", "2", "3"

        int number = -1;

        switch (keyName)
        {
            case "1":
                number = 0;
                break;

            case "2":
                number = 1;
                break;

            case "3":
                number = 2;
                break;
        }

        if (number >= 0)
        {
            OnItemInvenKeyReleased?.Invoke(number);
        }
    }


}
