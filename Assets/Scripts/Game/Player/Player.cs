using UnityEngine;

namespace Scripts.Game.Player
{
    /// <summary>
    /// Class Player with Single Responsibility Principle (SRP) applied
    /// Instead of using a monolithic class, this Player implementation divides responsibilities among specialized components.
    /// Each component focuses on a specific aspect of the player's behavior (input handling, movement, action...).
    /// </summary>

    [RequireComponent(typeof(PlayerInput), typeof(PlayerMovement), typeof(PlayerAction))]
    public class Player : MonoBehaviour
    {
        private PlayerInput m_PlayerInput;
        private PlayerMovement m_PlayerMovement;
        private PlayerAction m_PlayerAction;

        private Vector3 m_InitialPosition;
        private Quaternion m_InitialRotation;

        private bool m_IsCelebrating = false;
        public bool IsCelebrating { get => m_IsCelebrating; set => m_IsCelebrating = value; }

        // MonoBehaviour
        private void Awake() {
            m_PlayerInput = GetComponent<PlayerInput>();
            m_PlayerMovement = GetComponent<PlayerMovement>();
            m_PlayerAction = GetComponent<PlayerAction>();

            SetInitialTransform(this.transform);
        }

        private void LateUpdate()
        {
            Vector3 inputMovementVector = m_PlayerInput.InputMovementVector;
            m_PlayerMovement.Move(inputMovementVector);

            Vector3 inputRotationVector = m_PlayerInput.InputRotationVector;
            m_PlayerMovement.Rotate(inputRotationVector);

            Vector3 inputPullVector = m_PlayerInput.InputPullVector;
            m_PlayerAction.Push(inputPullVector);

            if (true == IsCelebrating)
            {
                transform.Rotate(Vector3.up, 250.0f * Time.deltaTime);
            }
        }

        // Initialize & Reset
        public void Initialize()
        {
            IsCelebrating = false;
            transform.position = m_InitialPosition;
            transform.rotation = m_InitialRotation;
            m_PlayerMovement.AllowMovement();
        }

        public void SetInitialTransform(Transform newTransform)
        {
            m_InitialPosition = newTransform.position;
            m_InitialRotation = newTransform.rotation;
        }

        // Logic
        public void Celebrate()
        {
            m_PlayerMovement.DontAllowMovement();
            IsCelebrating = true;
        }   
    }
}