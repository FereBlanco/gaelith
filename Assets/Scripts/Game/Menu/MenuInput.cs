using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Scripts.Game.Menu
{
    /// <summary>
    /// Menu shortcuts (keyboard and gamepad) with the Input System.
    /// The keys and buttons are defined in the GaelithInputs action map of each screen:
    /// - StartScreen: Start (Enter / Y), Quit (Q)
    /// - Room: Reset (R / Y), NextRoom (N), Quit (Q / Start)
    /// </summary>
    public class MenuInput : MonoBehaviour
    {
        // Inspector fields
        [SerializeField] private InputActionAsset InputActions;

        [Tooltip("Action map with the menu shortcuts of this screen (StartScreen or Room)")]
        [SerializeField] private string m_ActionMapName = "Room";

        // Private members
        private readonly Dictionary<InputAction, Action> m_MenuActions = new Dictionary<InputAction, Action>();
        private bool m_AreActionsEnabled = false;

        // MonoBehaviour
        private void Awake()
        {
            Assert.IsNotNull(InputActions, "ERROR: InputActions not assigned in MenuInput class");

            InputActionMap actionMap = InputActions.FindActionMap(m_ActionMapName);
            Assert.IsNotNull(actionMap, $"ERROR: action map {m_ActionMapName} not found in MenuInput class");

            // Each screen only has some of these actions
            AddMenuAction(actionMap, "Start", EventHandler.RaiseOnMenuStart);
            AddMenuAction(actionMap, "Reset", EventHandler.RaiseOnMenuReset);
            AddMenuAction(actionMap, "NextRoom", EventHandler.RaiseOnMenuNextRoom);
            AddMenuAction(actionMap, "Quit", EventHandler.RaiseOnMenuQuit);
        }

        // Logic
        private void AddMenuAction(InputActionMap actionMap, string actionName, Action raiseEvent)
        {
            InputAction action = actionMap.FindAction(actionName);
            if (null != action)
            {
                m_MenuActions.Add(action, raiseEvent);
            }
        }

        private void OnMenuActionPerformed(InputAction.CallbackContext context)
        {
            if (m_MenuActions.TryGetValue(context.action, out Action raiseEvent))
            {
                raiseEvent();
            }
        }

        private void EnableActions()
        {
            if (m_AreActionsEnabled) return;
            m_AreActionsEnabled = true;

            foreach (InputAction action in m_MenuActions.Keys)
            {
                action.Enable();
                action.performed += OnMenuActionPerformed;
            }
        }

        private void DisableActions()
        {
            if (!m_AreActionsEnabled) return;
            m_AreActionsEnabled = false;

            foreach (InputAction action in m_MenuActions.Keys)
            {
                action.performed -= OnMenuActionPerformed;
                action.Disable();
            }
        }

        // MonoBehaviour Callbacks
        private void OnEnable()
        {
            EnableActions();
        }

        private void OnDisable()
        {
            DisableActions();
        }

        private void OnDestroy()
        {
            DisableActions();
        }
    }
}
