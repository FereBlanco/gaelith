using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    // Logic
    private void Initialize()
    {
        EventHandler.OnMenuStart += OnMenuStartCallback;
        EventHandler.OnMenuQuit += OnMenuQuitCallback;
    }
    
    private void Reset()
    {
        EventHandler.OnMenuStart -= OnMenuStartCallback;
        EventHandler.OnMenuQuit -= OnMenuQuitCallback;
    }


    // MonoBehaviour Callbacks
    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        // Handles normal cases where the object is disabled in the scene
        Reset();
    }

    private void OnDestroy()
    {
        // Catches edge cases (such as Unity destroying the object without enabling it again, or if the GameObject was never re-enabled since being disabled),ensuring the static event doesn't try to call a method on a destroyed instance.
        Reset();
    }

    // Callbacks
    private void OnMenuStartCallback()
    {
        SceneManager.LoadScene(Constants.SCENE_ROOM);
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
