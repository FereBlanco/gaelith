using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            EventHandler.RaiseOnCollectibleCollected(transform);
        }
    }
}
