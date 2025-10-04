using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "Scriptable Objects/PlayerInput")]
public class PlayerInput : ScriptableObject, IPlayerActions
{
    private Controls _controls;
    public Vector2 MoveDir { get; private set; }
    public Vector2 Scroll {  get; private set; }
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
}
