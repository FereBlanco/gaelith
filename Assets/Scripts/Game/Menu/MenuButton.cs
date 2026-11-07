using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Game.Menu
{
    /// <summary>
    /// Bridge between a UI menu button and the EventHandler: clicking the button raises the same
    /// menu event as the keyboard / gamepad shortcuts, so every input goes through the event bus.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour
    {
        public enum MenuEvent { Start, Reset, NextRoom, Quit }

        // Inspector fields
        [Tooltip("Menu event raised when the button is clicked")]
        [SerializeField] private MenuEvent m_MenuEvent = MenuEvent.Start;

        // Private members
        private Button m_Button;

        // MonoBehaviour
        private void Awake()
        {
            m_Button = GetComponent<Button>();
        }

        // Logic
        private void OnClick()
        {
            switch (m_MenuEvent)
            {
                case MenuEvent.Start:
                    EventHandler.RaiseOnMenuStart();
                    break;
                case MenuEvent.Reset:
                    EventHandler.RaiseOnMenuReset();
                    break;
                case MenuEvent.NextRoom:
                    EventHandler.RaiseOnMenuNextRoom();
                    break;
                case MenuEvent.Quit:
                    EventHandler.RaiseOnMenuQuit();
                    break;
            }
        }

        // MonoBehaviour Callbacks
        private void OnEnable()
        {
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            m_Button.onClick.RemoveListener(OnClick);
        }
    }
}
