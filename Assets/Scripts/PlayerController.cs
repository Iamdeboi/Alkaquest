using System;
using UnityEngine;


[DefaultExecutionOrder(-1)]
public class PlayerController : Subject
{
    #region Class Variables
    [Header("Components")]
    [SerializeField] private CharacterController m_CharacterController;
    [SerializeField] private Camera m_PlayerCamera;

    [Header("Base Movement")]
    public float runAcceleration = 0.25f;
    public float runSpeed = 4f;
    public float drag = 0.1f;
    public float movingThreshold = 0.01f;

    [Header("Camera Settings")]
    public float lookSensitivityH = 0.1f;
    public float lookSensitivityV = 0.1f;
    public float lookLimitV = 89f;

    private PlayerLocomotionInput m_PlayerLocomotionInput;
    private PlayerState m_PlayerState;

    private Vector2 m_CameraRotation = Vector2.zero;
    private Vector2 m_PlayerTargetRotation = Vector2.zero;
    #endregion

    #region Startup
    private void Awake()
    {
        m_PlayerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        m_PlayerState = GetComponent<PlayerState>();
    }
    

    private void Start()
    {
        NotifyObservers();
    }
    #endregion

    #region Update Logic
    private void Update()
    {
        UpdateMovementState();
        HandleLateralMovement();
    }

    private void UpdateMovementState()
    {
        bool isMovementInput = m_PlayerLocomotionInput.MovementInput != Vector2.zero;
        bool isMovingLaterally = IsMovingLaterally();

        PlayerMovementState lateralState = isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;
        m_PlayerState.SetPlayerMovementState(lateralState);
    }

    private void HandleLateralMovement()
    {
        Vector3 cameraForwardXZ = new Vector3(m_PlayerCamera.transform.forward.x, 0f, m_PlayerCamera.transform.forward.z).normalized;
        Vector3 cameraRightXZ = new Vector3(m_PlayerCamera.transform.right.x, 0f, m_PlayerCamera.transform.right.z).normalized;
        Vector3 movementDirection = cameraRightXZ * m_PlayerLocomotionInput.MovementInput.x + cameraForwardXZ * m_PlayerLocomotionInput.MovementInput.y;

        // Kinematic equation calculations
        Vector3 movementDelta = movementDirection * runAcceleration * Time.deltaTime;
        Vector3 newVelocity = m_CharacterController.velocity + movementDelta;

        // Add drag to the player
        Vector3 currentDrag = newVelocity.normalized * drag * Time.deltaTime;

        // Prevents velocity sending the player backwards if the velocity is too low a value considering drag
        // "IF new velocity's magnitude is > drag, THEN subtract drag from newVelocity. ELSE return Vector3.zero to clamp the newVelocity
        newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
        newVelocity = Vector3.ClampMagnitude(newVelocity, runSpeed);

        // Move character once per tick ONLY!!!
        m_CharacterController.Move(newVelocity * Time.deltaTime);
    }
    #endregion

    #region Late Update Logic
    private void LateUpdate()
    {
        m_CameraRotation.x += lookSensitivityH * m_PlayerLocomotionInput.LookInput.x;
        m_CameraRotation.y = Mathf.Clamp(m_CameraRotation.y - lookSensitivityV * m_PlayerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);
        
        // Rotate the player model based on look direction
        m_PlayerTargetRotation.x += transform.eulerAngles.x + lookSensitivityH * m_PlayerLocomotionInput.LookInput.x;
        transform.rotation = Quaternion.Euler(0f, m_PlayerTargetRotation.x, 0f);

        m_PlayerCamera.transform.rotation = Quaternion.Euler(m_CameraRotation.y, m_CameraRotation.x, 0f);
    }
    #endregion

    #region State Checks
    private bool IsMovingLaterally()
    {
        Vector3 lateralVelocity = new Vector3(m_CharacterController.velocity.x, 0f, m_CharacterController.velocity.z);

        return lateralVelocity.magnitude > movingThreshold;
    }
    #endregion
}
