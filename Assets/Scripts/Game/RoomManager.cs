using UnityEngine.Assertions;
using UnityEngine;
using Scripts.Game.Player;
using Scripts.Game.Stones;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Player m_Player;
    [SerializeField] GameObject m_PortalKeyPrefab;
    [SerializeField] GameObject m_PortalDoorGO;
    [SerializeField] StoneManager m_StoneManager;

    private void Awake() {
        Assert.IsNotNull(m_Player, "ERROR: m_Player not set in RoomManager");
        Assert.IsNotNull(m_PortalKeyPrefab, "ERROR: m_PortalKeyPrefab not set in RoomManager");
        Assert.IsNotNull(m_PortalDoorGO, "ERROR: m_PortalDoor not set in RoomManager");
        Assert.IsNotNull(m_StoneManager, "ERROR: m_StoneManager not set in RoomManager");

        m_StoneManager.KeyStoneManager.OnKeyStonesAligned += OnKeyStonesAlignedCallback;

        Collectible portalDoorCollectible = m_PortalDoorGO.GetComponent<Collectible>();
        portalDoorCollectible.OnCollectibleCollected += OnCollectibleCollectedCallback;        
    }

    private void OnKeyStonesAlignedCallback(Vector3 centralPosition)
    {
        GameObject portalKey = Instantiate(m_PortalKeyPrefab, centralPosition, Quaternion.identity);
        
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

            PortalDoor portalDoor = m_PortalDoorGO.GetComponent<PortalDoor>();
            portalDoor.OpenPortalDoor();
        }

        if (transform.CompareTag(Constants.TAG_PORTAL_DOOR))
        {
            collectible.OnCollectibleCollected -= OnCollectibleCollectedCallback;
            m_Player.m_GameCompleted = true;
        }
    }
}