using System;
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
        private Player m_Player;

        private InputAction m_moveAction;
        private InputAction m_pushAction;

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
        private void Awake() {
            m_Player = GetComponent<Player>();
            Assert.IsNotNull(m_Player, "ERROR: m_Player value not found in PlayerInput class");

            m_moveAction = InputActions.FindAction("Move");
            Assert.IsNotNull(m_moveAction, "ERROR: m_moveAction value not found in PlayerInput class");

            m_pushAction = InputActions.FindAction("Push");
            Assert.IsNotNull(m_pushAction, "ERROR: m_pushAction value not found in PlayerInput class");
        }
                
        private void OnEnable() {
            InputActions.FindActionMap("Player").Enable();
        }

        private void OnDisable() {
            InputActions.FindActionMap("Player").Disable();
        }

        private void Update()
        {
            CheckInputSystemEntries();
            
            HandleMovementInput();
            HandleRotationInput();
            HandlePushInput();
        }

        private void CheckInputSystemEntries()
        {
            // Movement & Rotation
            if (m_moveAction.WasPressedThisFrame())
            {
                Vector2 moveInputValue = m_moveAction.ReadValue<Vector2>();

                if (moveInputValue.y == 1f)
                {
                    m_IsButtonUpPressed = true;
                }
                else if (moveInputValue.y == -1f)
                {
                    m_IsButtonDownPressed = true;
                }
                else if (moveInputValue.x == 1f)
                {
                    m_IsButtonRightPressed = true;
                }
                else if (moveInputValue.x == -1f)
                {
                    m_IsButtonLeftPressed = true;
                }
            }

            // Push
            if (m_pushAction.WasPressedThisFrame())
            {
                m_IsButtonSpacePressed = true;
            }
        }

        // Logic
        private void HandleMovementInput()
        {
            m_InputMovementVector = Vector3.zero;

            if (m_Player.IsInteractionAllowed)
            {
                if (m_IsButtonUpPressed)
                {
                    m_InputMovementVector = transform.forward;
                }

                if (m_IsButtonDownPressed)
                {
                    m_InputMovementVector = -1f * transform.forward;
                }
            }
            
            m_IsButtonUpPressed = false;
            m_IsButtonDownPressed = false;
        }

        private void HandleRotationInput()
        {
            m_InputRotationVector = Vector3.zero;

            if (m_Player.IsInteractionAllowed)
            {
                if (m_IsButtonLeftPressed)
                {
                    m_InputRotationVector = -1f * transform.up;
                }

                if (m_IsButtonRightPressed)
                {
                    m_InputRotationVector = transform.up;
                }
            }

            m_IsButtonLeftPressed = false;
            m_IsButtonRightPressed = false;
        }

        private void HandlePushInput()
        {
            m_InputPushVector = Vector3.zero;

            if (m_Player.IsInteractionAllowed)
            {
                if (m_IsButtonSpacePressed)
                {
                    m_InputPushVector = transform.forward;
                }
            }
            
            m_IsButtonSpacePressed = false;
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
