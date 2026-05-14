using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
{
    // New Input System reference to Input Mapping
    public PlayerControls PlayerControls { get; private set; }
    // Value between -1 and 1 to define the axis directions read from the Inputs
    public Vector2 MovementInput { get; private set; }
    // Mouse delta value when looking
    public Vector2 LookInput { get; private set; }


    // Enable the controls
    private void OnEnable()
    {
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();

        PlayerControls.PlayerLocomotionMap.Enable();
        PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
    }

    private void OnDisable()
    {
        PlayerControls.PlayerLocomotionMap.Disable();
        PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        // Converts the movement input to the read value from the axis inputs
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Converts the mouse input as the input direction of where the player is looking
        LookInput = context.ReadValue<Vector2>();
    }
}
