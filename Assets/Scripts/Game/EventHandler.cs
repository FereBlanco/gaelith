using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private static EventHandler m_Instance;
    public static EventHandler Instance { get => m_Instance; }

    // Menu Events
    public static event Action OnMenuReset;
    public static event Action OnMenuQuit;

    // Stone Events
    public static event Action<Vector3> OnKeyStonesAligned;

    // Collectible Events
    public static event Action<Transform> OnCollectibleCollected;

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

    public static void RaiseOnMenuReset() { OnMenuReset?.Invoke(); }
    public static void RaiseOnMenuQuit() { OnMenuQuit?.Invoke(); }    
    public static void RaiseOnKeyStonesAligned(Vector3 vector) { OnKeyStonesAligned?.Invoke(vector); }    
    public static void RaiseOnCollectibleCollected(Transform transform) { OnCollectibleCollected?.Invoke(transform); }    
}
