using UnityEngine;
using UnityEngine.Assertions;

public class WorldManager : MonoBehaviour
{
    private RoomManager m_RoomManager;

    // MonoBehaviour
    private void Awake()
    {
        m_RoomManager = GetComponentInChildren<RoomManager>();
        Assert.IsNotNull(m_RoomManager, "ERROR: m_RoomManager not found in WorldManager children");

        Initialize();
    }

    public void Start()
    {
        m_RoomManager.LoadRoom();
    }

    // Initialize & Reset
    private void Initialize()
    {
        EventHandler.OnMenuReset += OnMenuResetCallback;
        EventHandler.OnMenuNextRoom += OnMenuNextRoomCallback;
        EventHandler.OnMenuQuit += OnMenuQuitCallback;
    }
    
    public void Reset()
    {
        EventHandler.OnMenuReset -= OnMenuResetCallback;
        EventHandler.OnMenuNextRoom -= OnMenuNextRoomCallback;
        EventHandler.OnMenuQuit -= OnMenuQuitCallback;
    }

    // Callbacks
    public void OnMenuResetCallback()
    {
        m_RoomManager.ResetRoom();
    }
    public void OnMenuNextRoomCallback()
    {
        m_RoomManager.NextRoom();
    }

    private void OnMenuQuitCallback()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
