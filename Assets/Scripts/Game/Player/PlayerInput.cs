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
        [SerializeField] private KeyCode m_ForwardKeyAlt = KeyCode.W;
        [SerializeField] private KeyCode m_BackwardKey = KeyCode.DownArrow;
        [SerializeField] private KeyCode m_BackwardKeyAlt = KeyCode.S;

        [Header("Rotation Controls")]
        [Tooltip("Use these keys to rotate Player left and right")]
        [SerializeField] private KeyCode m_RotateLeftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode m_RotateLeftKeyAlt = KeyCode.A;
        [SerializeField] private KeyCode m_RotateRightKey = KeyCode.RightArrow;
        [SerializeField] private KeyCode m_RotateRightKeyAlt = KeyCode.D;

        [Header("Action Controls")]
        [Tooltip("Use these keys to perform Player ACTIONS")]
        [SerializeField] private KeyCode m_PullKey = KeyCode.Space;

        // Private members
        private Vector3 m_InputMovementVector;
        private Vector3 m_InputRotationVector;
        private Vector3 m_InputPushVector;

        private bool m_IsButtonUpPressed = false;
        private bool m_IsButtonDownPressed = false;
        private bool m_IsButtonLeftPressed = false;
        private bool m_IsButtonRightPressed = false;
        private bool m_IsButtonSpacePressed = false;

        // Properties
        public Vector3 InputMovementVector => m_InputMovementVector;
        public Vector3 InputRotationVector => m_InputRotationVector;
        public Vector3 InputPullVector => m_InputPushVector;

        // MonoBehaviour
        private void Update()
        {
            HandleMovementInput();
            HandleRotationInput();
            HandlePushInput();
        }

        // Logic
        private void HandleMovementInput()
        {
            m_InputMovementVector = Vector3.zero;

            if (Input.GetKeyUp(m_ForwardKey) || Input.GetKeyUp(m_ForwardKeyAlt) || m_IsButtonUpPressed)
            {
                m_IsButtonUpPressed = false;
                m_InputMovementVector = transform.forward;
            }

            if (Input.GetKeyUp(m_BackwardKey) || Input.GetKeyUp(m_BackwardKeyAlt) || m_IsButtonDownPressed)
            {
                m_IsButtonDownPressed = false;
                m_InputMovementVector = -1f * transform.forward;
            }
        }

        private void HandleRotationInput()
        {
            m_InputRotationVector = Vector3.zero;

            if (Input.GetKeyUp(m_RotateLeftKey) || Input.GetKeyUp(m_RotateLeftKeyAlt) || m_IsButtonLeftPressed)
            {
                m_IsButtonLeftPressed = false;
                m_InputRotationVector = -1f * transform.up;
            }

            if (Input.GetKeyUp(m_RotateRightKey) || Input.GetKeyUp(m_RotateRightKeyAlt) || m_IsButtonRightPressed)
            {
                m_IsButtonRightPressed = false;
                m_InputRotationVector = transform.up;
            }
        }

        private void HandlePushInput()
        {
            m_InputPushVector = Vector3.zero;

            if (Input.GetKeyUp(m_PullKey) || m_IsButtonSpacePressed)
            {
                m_IsButtonSpacePressed = false;
                m_InputPushVector = transform.forward;
            }
        }

        // Public
        public void StepForward()
        {
            m_IsButtonUpPressed = true;            
        }

        public void StepBackward()
        {
            m_IsButtonDownPressed = true;            
        }
        
        public void TurnLeft()
        {
            m_IsButtonLeftPressed = true;            
        }

        public void TurnRight()
        {
            m_IsButtonRightPressed = true;
        }

        public void Push()
        {
            m_IsButtonSpacePressed = true;
        }
    }
}
