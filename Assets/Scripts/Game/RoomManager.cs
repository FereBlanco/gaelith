using UnityEngine.Assertions;
using UnityEngine;
using Scripts.Game.Player;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] StoneHitManager[] dynamicStones;
    [SerializeField] KeyStonesManager keyStonesManager;
    [SerializeField] GameObject portalKeyPrefab;
    [SerializeField] GameObject portalDoorGO;

    private void Awake() {
        Assert.IsNotNull(player, "ERROR: player not set in RoomManager");
        Assert.IsNotNull(dynamicStones, "ERROR: dynamicStones not set in RoomManager");
        Assert.IsNotNull(keyStonesManager, "ERROR: keyStonesManager not set in RoomManager");
        Assert.IsNotNull(portalKeyPrefab, "ERROR: portalKey not set in RoomManager");
        Assert.IsNotNull(portalDoorGO, "ERROR: portalDoor not set in RoomManager");

        keyStonesManager.OnKeyStonesAligned += OnKeyStonesAlignedCallback;

        Collectible portalDoorCollectible = portalDoorGO.GetComponent<Collectible>();
        portalDoorCollectible.OnCollectibleCollected += OnCollectibleCollectedCallback;        
    }

    private void OnKeyStonesAlignedCallback(Vector3 centralPosition)
    {
        GameObject portalKey = Instantiate(portalKeyPrefab, centralPosition, Quaternion.identity);
        
        Collectible portalKeyCollectible = portalKey.AddComponent<Collectible>();
        portalKeyCollectible.OnCollectibleCollected += OnCollectibleCollectedCallback;
    }

    private void OnCollectibleCollectedCallback(Transform transform)
    {
        Collectible collectible = transform.GetComponent<Collectible>();

        if (transform.CompareTag(Constants.TAG_PORTAL_KEY))
        {
            collectible.OnCollectibleCollected -= OnCollectibleCollectedCallback;
            Destroy(transform.gameObject);

            PortalDoor portalDoor = portalDoorGO.GetComponent<PortalDoor>();
            portalDoor.OpenPortalDoor();
        }

        if (transform.CompareTag(Constants.TAG_PORTAL_DOOR))
        {
            collectible.OnCollectibleCollected -= OnCollectibleCollectedCallback;
            player.m_GameCompleted = true;
        }
    }
}
