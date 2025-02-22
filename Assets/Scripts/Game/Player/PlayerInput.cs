using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        // Inspector fields
        [Header("Movement Controls")]
        [Tooltip("Use these keys to move Player forward and backward")]
        [SerializeField] private KeyCode m_ForwardKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode m_BackwardKey = KeyCode.DownArrow;

        [Header("Rotation Controls")]
        [Tooltip("Use these keys to rotate Player left and right")]
        [SerializeField] private KeyCode m_RotateLeftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode m_RotateRightKey = KeyCode.RightArrow;

        [Header("Action Controls")]
        [Tooltip("Use these keys to perform Player ACTIONS")]
        [SerializeField] private KeyCode m_PullKey = KeyCode.Space;

        // Private members
        private Vector3 m_InputMovementVector;
        private Vector3 m_InputRotationVector;
        private Vector3 m_InputPullVector;

        // Properties
        public Vector3 InputMovementVector => m_InputMovementVector;
        public Vector3 InputRotationVector => m_InputRotationVector;
        public Vector3 InputPullVector => m_InputPullVector;

        // MonoBehaviour
        private void Update()
        {
            HandleMovementInput();
            HandleRotationInput();
            HandlePullInput();
        }

        // Logic
        private void HandleMovementInput()
        {
            m_InputMovementVector = Vector3.zero;

            if (Input.GetKeyUp(m_ForwardKey))
            {
                m_InputMovementVector = transform.forward;
            }

            if (Input.GetKeyUp(m_BackwardKey))
            {
                m_InputMovementVector = -1f * transform.forward;
            }
        }

        private void HandleRotationInput()
        {
            m_InputRotationVector = Vector3.zero;

            if (Input.GetKeyUp(m_RotateLeftKey))
            {
                m_InputRotationVector = -1f * transform.up;
            }

            if (Input.GetKeyUp(m_RotateRightKey))
            {
                m_InputRotationVector = transform.up;
            }
        }

        private void HandlePullInput()
        {
            m_InputPullVector = Vector3.zero;

            if (Input.GetKeyUp(m_PullKey))
            {
                m_InputPullVector = transform.forward;
            }
        }
    }
}
