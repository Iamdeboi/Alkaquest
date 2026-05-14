using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private PlayerLocomotionInput m_PlayerLocomotionInput;

    private static int inputXHash = Animator.StringToHash("inputX");
    private static int inputYHash = Animator.StringToHash("inputY");

    private void Awake()
    {
        m_PlayerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        Vector2 inputTarget = m_PlayerLocomotionInput.MovementInput;

        m_Animator.SetFloat(inputXHash, inputTarget.x);
        m_Animator.SetFloat(inputYHash, inputTarget.y);
    }
}
