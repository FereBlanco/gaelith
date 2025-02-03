using UnityEngine;

public class Collectible : MonoBehaviour
{
    public event System.Action<Transform> OnCollectibleCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            OnCollectibleCollected?.Invoke(transform);
        }
    }
}
