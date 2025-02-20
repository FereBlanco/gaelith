using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private static EventHandler m_Instance;
    public static EventHandler Instance { get => m_Instance; }

    // Events
    public static event Action OnMenuReset;
    public static event Action OnMenuQuit;

    private void Awake()
    {
        // Singleton pattern
        if (null == m_Instance)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RaiseEvent(Action action)
    {
        action?.Invoke();
    }

    public static void RaiseOnMenuReset()
    {
        OnMenuReset?.Invoke();
    }

    public static void RaiseOnMenuQuit()
    {
        OnMenuQuit?.Invoke();
    }    
}
