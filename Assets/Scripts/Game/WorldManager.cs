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
        EventHandler.OnMenuQuit += OnMenuQuitCallback;
    }
    
    public void Reset()
    {
        EventHandler.OnMenuReset -= OnMenuResetCallback;
        EventHandler.OnMenuQuit -= OnMenuQuitCallback;
    }

    // Callbacks
    public void OnMenuResetCallback()
    {
        m_RoomManager.LoadRoom();
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
