using UnityEngine;

namespace Scripts.Game.Menu
{
    public class MenuInput : MonoBehaviour
    {
        // Inspector fields
        [Header("Menu Options")]
        [Tooltip("Use these keys to select MENU options")]
        [SerializeField] private KeyCode m_StartKey = KeyCode.Return;
        [SerializeField] private KeyCode m_ResetKey = KeyCode.R;
        [SerializeField] private KeyCode m_NextRoomKey = KeyCode.N;
        [SerializeField] private KeyCode m_QuitKey = KeyCode.Q;

        // Private members


        // MonoBehaviour
        private void Update()
        {
            HandleMenuInput();
        }

        // Private Methods
        private void HandleMenuInput()
        {
            if (Input.GetKeyUp(m_ResetKey))
            {
                EventHandler.RaiseOnMenuReset();
            }
            
            if (Input.GetKeyUp(m_ResetKey))
            {
                EventHandler.RaiseOnMenuReset();
            }

            if (Input.GetKeyUp(m_NextRoomKey))
            {
                EventHandler.RaiseOnMenuNextRoom();
            }

            if (Input.GetKeyUp(m_QuitKey))
            {
                EventHandler.RaiseOnMenuQuit();
            }
        }
    }
}
