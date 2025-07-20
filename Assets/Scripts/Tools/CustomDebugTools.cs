using UnityEngine;

namespace Scripts.Tools
{
    static public class CustomDebugTools
    {
        static public void CustomLog(string message)
        {
            #if UNITY_EDITOR
            Debug.Log(message);
            #endif
        }

        static public void CustomWarning(string message)
        {
            #if UNITY_EDITOR
            Debug.LogWarning(message);
            #endif
        }
    }
}
