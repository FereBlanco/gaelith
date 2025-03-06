using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    // MonoBehaviour
    private void Awake()
    {
        Initialize();
    }

    // Initialize & Reset
    private void Initialize()
    {
        EventHandler.OnMenuStart += OnMenuStartCallback;
        EventHandler.OnMenuQuit += OnMenuQuitCallback;
    }
    
    public void Reset()
    {
        EventHandler.OnMenuStart -= OnMenuStartCallback;
        EventHandler.OnMenuQuit -= OnMenuQuitCallback;
    }

    public void OnMenuStartCallback()
    {
        SceneManager.LoadScene(Constants.SCENE_ROOM);
    }

    public void OnMenuQuitCallback()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }    
}
