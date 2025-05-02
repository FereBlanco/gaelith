using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Scripts.Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        // Inspector fields
        public InputActionAsset InputActions;

        // Private members
        private InputAction m_moveAction;
        private InputAction m_pushAction;

        private Vector3 m_InputMovementVector;
        public Vector3 InputMovementVector => m_InputMovementVector;

        private Vector3 m_InputRotationVector;
        public Vector3 InputRotationVector => m_InputRotationVector;

        private Vector3 m_InputPushVector;
        public Vector3 InputPullVector => m_InputPushVector;

        // MonoBehaviour
        private void Awake() {
            m_moveAction = InputActions.FindAction("Move");
            Assert.IsNotNull(m_moveAction, "ERROR: m_moveAction value not found in PlayerInput class");

            m_pushAction = InputActions.FindAction("Push");
            Assert.IsNotNull(m_pushAction, "ERROR: m_pushAction value not found in PlayerInput class");
        }
                
        private void OnEnable() {
            m_moveAction.Enable();
            m_moveAction.performed += OnMove;

            m_pushAction.Enable();
            m_pushAction.performed += OnPush;
        }

        private void OnDisable() {
            m_moveAction.performed -= OnMove;
            m_moveAction.Disable();

            m_pushAction.performed -= OnPush;
            m_pushAction.Disable();
        }

        // Logic
        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 moveInputValue = m_moveAction.ReadValue<Vector2>();

            if (moveInputValue.y == 1f)
            {
                StepForward();
            }
            else if (moveInputValue.y == -1f)
            {
                StepBackward();
            }
            else if (moveInputValue.x == 1f)
            {
                TurnRight();
            }
            else if (moveInputValue.x == -1f)
            {
                TurnLeft();
            }
        }
        private void OnPush(InputAction.CallbackContext context)
        {
            Push();
        }

        public void StepForward()
        {
            m_InputMovementVector = transform.forward;       
            StartCoroutine(ClearInput());
        }

        public void StepBackward()
        {
            m_InputMovementVector = -1f * transform.forward;        
            StartCoroutine(ClearInput());
        }
        
        public void TurnLeft()
        {
            m_InputRotationVector = -1f * transform.up;         
            StartCoroutine(ClearInput());
        }

        public void TurnRight()
        {
            m_InputRotationVector = transform.up;
            StartCoroutine(ClearInput());
        }

        public void Push()
        {
            m_InputPushVector = transform.forward;
            StartCoroutine(ClearInput());
        }

        IEnumerator ClearInput()
        {
            yield return null;

            m_InputMovementVector = Vector3.zero;
            m_InputRotationVector = Vector3.zero;
            m_InputPushVector = Vector3.zero;
        }
    }
}
