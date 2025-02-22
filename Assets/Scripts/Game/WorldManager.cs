using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private void Awake()
    {
        EventHandler.OnMenuReset += OnMenuResetCallback;
        EventHandler.OnMenuQuit += OnMenuQuitCallback;
    }

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
