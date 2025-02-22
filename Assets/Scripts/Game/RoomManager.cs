using UnityEngine.Assertions;
using UnityEngine;
using Scripts.Game.Player;
using Scripts.Game.Stones;
using System.Collections;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Player m_Player;
    [SerializeField] private GameObject m_PortalKey;
    [SerializeField] private GameObject m_PortalDoorGO;
    private StoneManager m_StoneManager;

    // MonoBehaviour
    private void Awake() {
        Assert.IsNotNull(m_Player, "ERROR: m_Player not set in RoomManager");
        Assert.IsNotNull(m_PortalKey, "ERROR: m_PortalKeyPrefab not set in RoomManager");
        Assert.IsNotNull(m_PortalDoorGO, "ERROR: m_PortalDoor not set in RoomManager");

        m_StoneManager = GetComponentInChildren<StoneManager>();
        Assert.IsNotNull(m_StoneManager, "ERROR: m_StoneManager not found in RoomManager children");

        Collectible portalDoorCollectible = m_PortalDoorGO.GetComponent<Collectible>();
        Assert.IsNotNull(portalDoorCollectible, "ERROR: portalDoorCollectible not set in RoomManager");

        Initialiaze();
    }

    // Initialize & Reset
    internal void Initialiaze()
    {
        m_PortalKey.SetActive(false);
        EventHandler.OnCollectibleCollected += OnCollectibleCollectedCallback;        
        EventHandler.OnKeyStonesAlign += OnKeyStonesAlignedCallback;
    }

    private void Reset() {
        m_PortalKey.SetActive(false);
        EventHandler.OnCollectibleCollected -= OnCollectibleCollectedCallback;        
        EventHandler.OnKeyStonesAlign -= OnKeyStonesAlignedCallback;
    }

    // Logic
    internal void StartRoom()
    {
        m_StoneManager.Initialize();
        m_Player.Initialize();
    }

    IEnumerator NextRoom()
    {
        yield return new WaitForSeconds(3.0f);
        m_Player.Reset();
        StartRoom();
    }

    // Callbacks
    private void OnKeyStonesAlignedCallback(Vector3 centralPosition)
    {
        EventHandler.OnKeyStonesAlign -= OnKeyStonesAlignedCallback;

        m_PortalKey.transform.position = centralPosition;
        m_PortalKey.transform.rotation = Quaternion.identity;
        m_PortalKey.SetActive(true);
    }

    private void OnCollectibleCollectedCallback(Transform transform)
    {
        Collectible collectible = transform.GetComponent<Collectible>();

        if (transform.CompareTag(Constants.TAG_PORTAL_KEY))
        {
            m_PortalKey.SetActive(false);

            PortalDoor portalDoor = m_PortalDoorGO.GetComponent<PortalDoor>();
            portalDoor.OpenPortalDoor();
        }

        if (transform.CompareTag(Constants.TAG_PORTAL_DOOR))
        {
            m_Player.Celebrate();
            StartCoroutine(NextRoom());
        }
    }
}