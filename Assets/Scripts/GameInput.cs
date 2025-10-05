using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{

    private InputActions inputActions;

    public static GameInput Instance { get; private set; }

    public event EventHandler OnMenuButtonPressed;

    private void Awake()
    {
        Instance = this;

        inputActions = new InputActions();
        inputActions.Enable();

        inputActions.Player.Menu.performed += Menu_performed;
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        inputActions.Disable();
    }

    public bool IsUpActionPressed()
    {
        return inputActions.Player.GoUp.IsPressed();
    }

    public bool IsLeftActionPressed()
    {
        return inputActions.Player.GoLeft.IsPressed();
    }

    public bool IsRightActionPressed()
    {
        return inputActions.Player.GoRight.IsPressed();
    }

    public Vector2 GetMovementInputVector()
    {
        return inputActions.Player.Movement.ReadValue<Vector2>();
    }
}
