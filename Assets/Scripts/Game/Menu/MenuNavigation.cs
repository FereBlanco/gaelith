using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Scripts.Game.Menu
{
    /// <summary>
    /// Gamepad control of the menu buttons.
    /// - Start screen (StartMenu): Y (North) starts the game.
    /// - In game (BaseRoom): Home focuses the menu (the Player can't move meanwhile), the D-Pad moves
    ///   between Reset and Quit, A presses the selected one, B or Home return to the game.
    ///   Y (North) resets the room. Start goes back to the start screen.
    /// </summary>
    public class MenuNavigation : MonoBehaviour
    {
        // Inspector fields
        [SerializeField] private InputActionAsset InputActions;

        [Header("Menu Buttons")]
        [Tooltip("In game menu buttons in navigation order. The first active one is selected when the menu gets the focus")]
        [SerializeField] private List<Button> m_Buttons = new List<Button>();

        [Tooltip("ON: in game menu (Home focuses the menu, Start goes to the start screen). OFF: start screen (Y starts the game)")]
        [SerializeField] private bool m_IsInGameMenu = true;

        // Private members
        private InputAction m_FocusAction;
        private InputAction m_BackAction;
        private InputAction m_StartGameAction;
        private InputAction m_ToStartScreenAction;
        private InputAction m_ResetRoomAction;

        private readonly List<Button> m_NavigableButtons = new List<Button>();
        private bool m_IsFocused = false;
        private bool m_AreActionsEnabled = false;

        // MonoBehaviour
        private void Awake()
        {
            Assert.IsNotNull(InputActions, "ERROR: InputActions not assigned in MenuNavigation class");

            m_FocusAction = InputActions.FindAction("Menu/Focus");
            Assert.IsNotNull(m_FocusAction, "ERROR: m_FocusAction value not found in MenuNavigation class");

            m_BackAction = InputActions.FindAction("Menu/Back");
            Assert.IsNotNull(m_BackAction, "ERROR: m_BackAction value not found in MenuNavigation class");

            m_StartGameAction = InputActions.FindAction("Menu/StartGame");
            Assert.IsNotNull(m_StartGameAction, "ERROR: m_StartGameAction value not found in MenuNavigation class");

            m_ToStartScreenAction = InputActions.FindAction("Menu/ToStartScreen");
            Assert.IsNotNull(m_ToStartScreenAction, "ERROR: m_ToStartScreenAction value not found in MenuNavigation class");

            m_ResetRoomAction = InputActions.FindAction("Menu/ResetRoom");
            Assert.IsNotNull(m_ResetRoomAction, "ERROR: m_ResetRoomAction value not found in MenuNavigation class");

            if (m_IsInGameMenu)
            {
                SetUpNavigation();
            }
        }

        private void LateUpdate()
        {
            // A mouse click selects the button: while the menu hasn't the focus, the selection is removed
            // so the D-Pad (that moves the Player) or A don't act on it
            if (m_IsInGameMenu && !m_IsFocused && IsMenuButtonSelected())
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        // Logic
        private void SetUpNavigation()
        {
            m_NavigableButtons.Clear();
            foreach (Button button in m_Buttons)
            {
                if (null != button && button.gameObject.activeInHierarchy)
                {
                    m_NavigableButtons.Add(button);
                    button.onClick.AddListener(OnMenuButtonClicked);
                }
            }

            // Explicit navigation (wrap around): Up/Left = previous button, Down/Right = next button
            int count = m_NavigableButtons.Count;
            for (int i = 0; i < count; i++)
            {
                Button previous = m_NavigableButtons[(i - 1 + count) % count];
                Button next = m_NavigableButtons[(i + 1) % count];

                Navigation navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnUp = previous,
                    selectOnLeft = previous,
                    selectOnDown = next,
                    selectOnRight = next
                };
                m_NavigableButtons[i].navigation = navigation;
            }
        }

        private Button GetFirstSelectableButton()
        {
            foreach (Button button in m_NavigableButtons)
            {
                if (button.gameObject.activeInHierarchy && button.IsInteractable())
                {
                    return button;
                }
            }
            return null;
        }

        private bool IsMenuButtonSelected()
        {
            if (null == EventSystem.current) return false;

            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (null == selected) return false;

            foreach (Button button in m_NavigableButtons)
            {
                if (button.gameObject == selected) return true;
            }
            return false;
        }

        private void Focus()
        {
            Button first = GetFirstSelectableButton();
            if (null == first || null == EventSystem.current) return;

            m_IsFocused = true;
            EventSystem.current.SetSelectedGameObject(first.gameObject);
            EventHandler.RaiseOnMenuFocus(true);
        }

        private void Unfocus()
        {
            if (!m_IsFocused) return;

            m_IsFocused = false;
            if (IsMenuButtonSelected())
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            EventHandler.RaiseOnMenuFocus(false);
        }

        private void EnableActions()
        {
            if (m_AreActionsEnabled) return;
            m_AreActionsEnabled = true;

            if (m_IsInGameMenu)
            {
                m_FocusAction.Enable();
                m_FocusAction.performed += OnFocusPerformed;

                m_BackAction.Enable();
                m_BackAction.performed += OnBackPerformed;

                m_ToStartScreenAction.Enable();
                m_ToStartScreenAction.performed += OnToStartScreenPerformed;

                m_ResetRoomAction.Enable();
                m_ResetRoomAction.performed += OnResetRoomPerformed;
            }
            else
            {
                m_StartGameAction.Enable();
                m_StartGameAction.performed += OnStartGamePerformed;
            }
        }

        private void DisableActions()
        {
            if (!m_AreActionsEnabled) return;
            m_AreActionsEnabled = false;

            if (m_IsInGameMenu)
            {
                m_FocusAction.performed -= OnFocusPerformed;
                m_FocusAction.Disable();

                m_BackAction.performed -= OnBackPerformed;
                m_BackAction.Disable();

                m_ToStartScreenAction.performed -= OnToStartScreenPerformed;
                m_ToStartScreenAction.Disable();

                m_ResetRoomAction.performed -= OnResetRoomPerformed;
                m_ResetRoomAction.Disable();
            }
            else
            {
                m_StartGameAction.performed -= OnStartGamePerformed;
                m_StartGameAction.Disable();
            }
        }

        // Input Callbacks
        private void OnFocusPerformed(InputAction.CallbackContext context)
        {
            if (m_IsFocused)
            {
                Unfocus();
            }
            else
            {
                Focus();
            }
        }

        private void OnBackPerformed(InputAction.CallbackContext context)
        {
            Unfocus();
        }

        private void OnToStartScreenPerformed(InputAction.CallbackContext context)
        {
            EventHandler.RaiseOnMenuQuit();
        }

        private void OnResetRoomPerformed(InputAction.CallbackContext context)
        {
            // If the menu has the focus, the control returns to the Player
            Unfocus();
            EventHandler.RaiseOnMenuReset();
        }

        private void OnStartGamePerformed(InputAction.CallbackContext context)
        {
            EventHandler.RaiseOnMenuStart();
        }

        private void OnMenuButtonClicked()
        {
            // After pressing Reset / Quit the control returns to the Player
            Unfocus();
        }

        // MonoBehaviour Callbacks
        private void OnEnable()
        {
            EnableActions();
        }

        private void OnDisable()
        {
            Unfocus();
            DisableActions();
        }

        private void OnDestroy()
        {
            DisableActions();

            foreach (Button button in m_NavigableButtons)
            {
                if (null != button)
                {
                    button.onClick.RemoveListener(OnMenuButtonClicked);
                }
            }
        }
    }
}
