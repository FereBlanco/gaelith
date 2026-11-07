using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    private RoomManager m_RoomManager;

    // MonoBehaviour
    private void Awake()
    {
        m_RoomManager = GetComponentInChildren<RoomManager>();
        Assert.IsNotNull(m_RoomManager, "ERROR: m_RoomManager not found in WorldManager children");
    }

    public void Start()
    {
        m_RoomManager.LoadRoom();
    }

    // Logic
    private void Initialize()
    {
        EventHandler.OnMenuReset += OnMenuResetCallback;
        EventHandler.OnMenuNextRoom += OnMenuNextRoomCallback;
        EventHandler.OnMenuQuit += OnMenuQuitCallback;
    }

    private void Reset()
    {
        EventHandler.OnMenuReset -= OnMenuResetCallback;
        EventHandler.OnMenuNextRoom -= OnMenuNextRoomCallback;
        EventHandler.OnMenuQuit -= OnMenuQuitCallback;
    }

    // MonoBehaviour Callbacks

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        Reset();
    }

    private void OnDestroy()
    {
        Reset();
    }

    // Callbacks
    private void OnMenuResetCallback()
    {
        m_RoomManager.ResetRoom();
    }
    private void OnMenuNextRoomCallback()
    {
        m_RoomManager.NextRoomFromMenu();
    }

    private void OnMenuQuitCallback()
    {
        SceneManager.LoadScene(Constants.SCENE_START);
    }
}
