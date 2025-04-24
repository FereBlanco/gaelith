using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

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
        private Player m_Player;

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
            Assert.IsTrue(m_Player, "ERROR: m_Player value not found in PlayerAction class");
        }
                
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

            if (m_Player.IsInteractionAllowed)
            {
                int vertical = 0;

                if (Gamepad.current != null) {
                    if (Gamepad.current.dpad.up.isPressed)
                    {
                        vertical = 1;
                    }
                    else if (Gamepad.current.dpad.down.isPressed)
                    {
                        vertical = -1;
                    }
                }

                if (Input.GetKeyUp(m_ForwardKey) || Input.GetKeyUp(m_ForwardKeyAlt) || m_IsButtonUpPressed || vertical == 1)
                {
                    m_InputMovementVector = transform.forward;
                }

                if (Input.GetKeyUp(m_BackwardKey) || Input.GetKeyUp(m_BackwardKeyAlt) || m_IsButtonDownPressed || vertical == -1)
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
                int horizontal = 0;

                if (Gamepad.current != null) {
                    if (Gamepad.current.dpad.left.isPressed)
                    {
                        horizontal = -1;
                    }
                    else if (Gamepad.current.dpad.right.isPressed)
                    {
                        horizontal = 1;
                    }
                }

                if (Input.GetKeyUp(m_RotateLeftKey) || Input.GetKeyUp(m_RotateLeftKeyAlt) || m_IsButtonLeftPressed || horizontal == -1)
                {
                    m_InputRotationVector = -1f * transform.up;
                }

                if (Input.GetKeyUp(m_RotateRightKey) || Input.GetKeyUp(m_RotateRightKeyAlt) || m_IsButtonRightPressed || horizontal == 1)
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
                if (Input.GetKeyUp(m_PullKey) || m_IsButtonSpacePressed || (Gamepad.current != null && Gamepad.current.buttonWest.isPressed))
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
