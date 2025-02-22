using UnityEngine.Assertions;
using UnityEngine;
using Scripts.Game.Player;
using Scripts.Game.Stones;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Player m_Player;
    [SerializeField] GameObject m_PortalKey;
    [SerializeField] GameObject m_PortalDoorGO;
    [SerializeField] StoneManager m_StoneManager;

    // MonoBehaviour Methods
    private void Awake() {
        Assert.IsNotNull(m_Player, "ERROR: m_Player not set in RoomManager");
        Assert.IsNotNull(m_PortalKey, "ERROR: m_PortalKeyPrefab not set in RoomManager");
        Assert.IsNotNull(m_PortalDoorGO, "ERROR: m_PortalDoor not set in RoomManager");
        Assert.IsNotNull(m_StoneManager, "ERROR: m_StoneManager not set in RoomManager");

        m_PortalKey.SetActive(false);

        Collectible portalDoorCollectible = m_PortalDoorGO.GetComponent<Collectible>();
        EventHandler.OnCollectibleCollected += OnCollectibleCollectedCallback;        
    }

    private void Start()
    {
        EventHandler.OnKeyStonesAlign += OnKeyStonesAlignedCallback;
    }

    // Callbacks
    private void OnKeyStonesAlignedCallback(Vector3 centralPosition)
    {
        EventHandler.OnKeyStonesAlign -= OnKeyStonesAlignedCallback;

        m_PortalKey.transform.position = centralPosition;
        m_PortalKey.transform.rotation = Quaternion.identity;
        m_PortalKey.SetActive(true);
        
        Collectible portalKeyCollectible = m_PortalKey.AddComponent<Collectible>();
    }

    private void OnCollectibleCollectedCallback(Transform transform)
    {
        Collectible collectible = transform.GetComponent<Collectible>();

        if (transform.CompareTag(Constants.TAG_PORTAL_KEY))
        {
            // EventHandler.OnCollectibleCollected -= OnCollectibleCollectedCallback;
            m_PortalKey.SetActive(false);

            PortalDoor portalDoor = m_PortalDoorGO.GetComponent<PortalDoor>();
            portalDoor.OpenPortalDoor();
        }

        if (transform.CompareTag(Constants.TAG_PORTAL_DOOR))
        {
            // EventHandler.OnCollectibleCollected -= OnCollectibleCollectedCallback;
            m_Player.m_GameCompleted = true;
        }
    }
}