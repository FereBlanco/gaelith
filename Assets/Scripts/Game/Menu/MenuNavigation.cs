using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Scripts.Game.Menu
{
    /// <summary>
    /// Gamepad navigation through the in game menu buttons (Reset, Quit).
    /// Home focuses the menu (the Player can't move meanwhile), the D-Pad moves between the buttons,
    /// A presses the selected one, B or Home return to the game.
    /// The menu shortcuts (Reset, Quit...) are handled by MenuInput.
    /// </summary>
    public class MenuNavigation : MonoBehaviour
    {
        // Inspector fields
        [SerializeField] private InputActionAsset InputActions;

        [Header("Menu Buttons")]
        [Tooltip("Menu buttons in navigation order. The first active one is selected when the menu gets the focus")]
        [SerializeField] private List<Button> m_Buttons = new List<Button>();

        // Private members
        private InputAction m_FocusAction;
        private InputAction m_BackAction;

        private readonly List<Button> m_NavigableButtons = new List<Button>();
        private bool m_IsFocused = false;
        private bool m_AreActionsEnabled = false;

        // MonoBehaviour
        private void Awake()
        {
            Assert.IsNotNull(InputActions, "ERROR: InputActions not assigned in MenuNavigation class");

            m_FocusAction = InputActions.FindAction("Room/Focus");
            Assert.IsNotNull(m_FocusAction, "ERROR: m_FocusAction value not found in MenuNavigation class");

            m_BackAction = InputActions.FindAction("Room/Back");
            Assert.IsNotNull(m_BackAction, "ERROR: m_BackAction value not found in MenuNavigation class");

            SetUpNavigation();
        }

        private void LateUpdate()
        {
            // A mouse click (or an on-screen button) selects the button: while the menu hasn't the focus,
            // the selection is removed so the D-Pad (that moves the Player) or A don't act on it
            if (!m_IsFocused && IsMenuButtonSelected())
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

            m_FocusAction.Enable();
            m_FocusAction.performed += OnFocusPerformed;

            m_BackAction.Enable();
            m_BackAction.performed += OnBackPerformed;
        }

        private void DisableActions()
        {
            if (!m_AreActionsEnabled) return;
            m_AreActionsEnabled = false;

            m_FocusAction.performed -= OnFocusPerformed;
            m_FocusAction.Disable();

            m_BackAction.performed -= OnBackPerformed;
            m_BackAction.Disable();
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

        // Event Callbacks
        private void OnMenuActionCallback()
        {
            // After Reset / Next Room (button or shortcut) the control returns to the Player
            Unfocus();
        }

        // MonoBehaviour Callbacks
        private void OnEnable()
        {
            EnableActions();
            EventHandler.OnMenuReset += OnMenuActionCallback;
            EventHandler.OnMenuNextRoom += OnMenuActionCallback;
        }

        private void OnDisable()
        {
            EventHandler.OnMenuReset -= OnMenuActionCallback;
            EventHandler.OnMenuNextRoom -= OnMenuActionCallback;
            Unfocus();
            DisableActions();
        }

        private void OnDestroy()
        {
            EventHandler.OnMenuReset -= OnMenuActionCallback;
            EventHandler.OnMenuNextRoom -= OnMenuActionCallback;
            DisableActions();
        }
    }
}
