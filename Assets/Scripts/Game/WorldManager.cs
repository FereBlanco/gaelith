using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // MonoBehaviour Methods
    private void Awake()
    {
        EventHandler.OnMenuReset += OnMenuResetCallback;
        EventHandler.OnMenuQuit += OnMenuQuitCallback;
    }

    // Callbacks
    public void OnMenuResetCallback()
    {
        Debug.Log("RESET!!!");
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
