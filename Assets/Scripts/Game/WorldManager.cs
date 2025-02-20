using Scripts.Game.Menu;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

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
        Debug.Log("QUIT!!!");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
