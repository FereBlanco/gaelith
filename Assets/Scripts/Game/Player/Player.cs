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
        // Private members
        PlayerInput m_PlayerInput;
        PlayerMovement m_PlayerMovement;
        PlayerAction m_PlayerAction;

        // Only for testing purposes
        public bool m_GameCompleted = false;

        // MonoBehaviour methods
        private void Awake() {
            Initialize();
        }

        private void LateUpdate()
        {
            Vector3 inputMovementVector = m_PlayerInput.InputMovementVector;
            m_PlayerMovement.Move(inputMovementVector);

            Vector3 inputRotationVector = m_PlayerInput.InputRotationVector;
            m_PlayerMovement.Rotate(inputRotationVector);

            Vector3 inputPullVector = m_PlayerInput.InputPullVector;
            m_PlayerAction.Push(inputPullVector);


            // Only for testing purposes
            if (true == m_GameCompleted)
            {
                m_PlayerMovement.DontAllowMovement();
                transform.Rotate(Vector3.up, 250.0f * Time.deltaTime);
            }
        }

        // Privatea Methods
        private void Initialize()
        {
            m_PlayerInput = GetComponent<PlayerInput>();
            m_PlayerMovement = GetComponent<PlayerMovement>();
            m_PlayerAction = GetComponent<PlayerAction>();
        }
    }
}