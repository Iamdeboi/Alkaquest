using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float locomotionBlendSpeed = 0.02f;

    private PlayerLocomotionInput m_PlayerLocomotionInput;
    private PlayerState m_PlayerState;

    private static int inputXHash = Animator.StringToHash("inputX");
    private static int inputYHash = Animator.StringToHash("inputY");

    private Vector3 m_CurrentBlendInput = Vector3.zero;

    private void Awake()
    {
        m_PlayerLocomotionInput = GetComponent<PlayerLocomotionInput>();
        m_PlayerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        Vector2 inputTarget = m_PlayerLocomotionInput.MovementInput;
        m_CurrentBlendInput = Vector3.Lerp(m_CurrentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);

        m_Animator.SetFloat(inputXHash, m_CurrentBlendInput.x);
        m_Animator.SetFloat(inputYHash, m_CurrentBlendInput.y);
    }
}
