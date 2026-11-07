using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Scripts.Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        // Inspector fields
        [SerializeField] private InputActionAsset InputActions;

        // Private members
        private InputAction m_moveAction;
        private InputAction m_pushAction;
        private bool m_AreActionsEnabled = false;

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

        private void StepForward()
        {
            m_InputMovementVector = transform.forward;       
            StartCoroutine(ClearInput());
        }

        private void StepBackward()
        {
            m_InputMovementVector = -1f * transform.forward;        
            StartCoroutine(ClearInput());
        }
        
        private void TurnLeft()
        {
            m_InputRotationVector = -1f * transform.up;         
            StartCoroutine(ClearInput());
        }

        private void TurnRight()
        {
            m_InputRotationVector = transform.up;
            StartCoroutine(ClearInput());
        }

        private void Push()
        {
            m_InputPushVector = transform.forward;
            StartCoroutine(ClearInput());
        }

        private void EnableActions()
        {
            if (m_AreActionsEnabled) return;
            m_AreActionsEnabled = true;

            m_moveAction.Enable();
            m_moveAction.performed += OnMove;

            m_pushAction.Enable();
            m_pushAction.performed += OnPush;
        }

        private void DisableActions()
        {
            if (!m_AreActionsEnabled) return;
            m_AreActionsEnabled = false;

            m_moveAction.performed -= OnMove;
            m_moveAction.Disable();

            m_pushAction.performed -= OnPush;
            m_pushAction.Disable();
        }

        IEnumerator ClearInput()
        {
            yield return null;

            m_InputMovementVector = Vector3.zero;
            m_InputRotationVector = Vector3.zero;
            m_InputPushVector = Vector3.zero;
        }

        // The Player doesn't move while the gamepad is navigating the menu (Reset / Quit buttons)
        private void OnMenuFocusCallback(bool isMenuFocused)
        {
            if (isMenuFocused)
            {
                DisableActions();
            }
            else if (isActiveAndEnabled)
            {
                EnableActions();
            }
        }

        // MonoBehaviour Callbacks
        private void OnEnable()
        {
            EnableActions();
            EventHandler.OnMenuFocus += OnMenuFocusCallback;
        }

        private void OnDisable()
        {
            EventHandler.OnMenuFocus -= OnMenuFocusCallback;
            DisableActions();
        }
        
        private void OnDestroy()
        {
            EventHandler.OnMenuFocus -= OnMenuFocusCallback;
            DisableActions();
        }
    }
}
