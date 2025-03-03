using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private static EventHandler m_Instance;
    public static EventHandler Instance { get => m_Instance; }
    public static Action OnMenuNext { get; internal set; }

    // Menu Events
    public static event Action OnMenuReset;
    public static event Action OnMenuNextRoom;
    public static event Action OnMenuQuit;

    // Stone Events
    public static event Action<bool> OnStoneStop;
    public static event Action<Vector3> OnKeyStonesAlign;

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

    // Raise Menu Events
    public static void RaiseOnMenuReset() { OnMenuReset?.Invoke(); }
    public static void RaiseOnMenuNextRoom() { OnMenuNextRoom?.Invoke(); }
    public static void RaiseOnMenuQuit() { OnMenuQuit?.Invoke(); }    

    // Stone Events
    public static void RaiseOnStoneStop(bool isSpecial = false) { OnStoneStop?.Invoke(isSpecial); }    
    public static void RaiseOnKeyStonesAlign(Vector3 vector) { OnKeyStonesAlign?.Invoke(vector); }   

    // Collectible Events
    public static void RaiseOnCollectibleCollected(Transform transform) { OnCollectibleCollected?.Invoke(transform); }    
}
