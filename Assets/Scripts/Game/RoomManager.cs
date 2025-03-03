using UnityEngine.Assertions;
using UnityEngine;
using Scripts.Game.Player;
using Scripts.Game.Stones;
using System.Collections;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Player m_Player;
    [SerializeField] private GameObject m_PortalKey;
    [SerializeField] private PortalDoor m_PortalDoor;
    private StoneManager m_StoneManager;
    bool isKeyStonesAlignSubscribed = false;

    // MonoBehaviour
    private void Awake() {
        Assert.IsNotNull(m_Player, "ERROR: m_Player not set in RoomManager");
        Assert.IsNotNull(m_PortalKey, "ERROR: m_PortalKeyPrefab not set in RoomManager");
        Assert.IsNotNull(m_PortalDoor, "ERROR: m_PortalDoor not set in RoomManager");

        m_StoneManager = GetComponentInChildren<StoneManager>();
        Assert.IsNotNull(m_StoneManager, "ERROR: m_StoneManager not found in RoomManager children");

        Collectible portalDoorCollectible = m_PortalDoor.GetComponent<Collectible>();
        Assert.IsNotNull(portalDoorCollectible, "ERROR: portalDoorCollectible not set in RoomManager");

        EventHandler.OnCollectibleCollected += OnCollectibleCollectedCallback;
        Initialize();
    }

    // Initialize & Reset
    private void Initialize()
    {
        if (false == isKeyStonesAlignSubscribed)
        {
            Debug.Log("RoomManager.Initialize >> EventHandler.OnKeyStonesAlign += OnKeyStonesAlignedCallback");
            EventHandler.OnKeyStonesAlign += OnKeyStonesAlignedCallback;
            isKeyStonesAlignSubscribed = true;
        }
    }

    // Logic
    internal void LoadRoom()
    {
        m_PortalDoor.ClosePortalDoor();
        m_PortalKey.SetActive(false);
        m_StoneManager.Initialize();
        m_Player.Initialize();
    }

    // Logic
    public void NextRoom()
    {
        StartCoroutine(NextRoomSequence());
    }

    IEnumerator NextRoomSequence()
    {
        // Reset();
        yield return new WaitForSeconds(3.0f);
        LoadRoom();
    }

    // Callbacks
    private void OnKeyStonesAlignedCallback(Vector3 centralPosition)
    {
        Debug.Log("RoomManager.OnKeyStonesAlignedCallback");
        // EventHandler.OnKeyStonesAlign -= OnKeyStonesAlignedCallback;
        isKeyStonesAlignSubscribed = false;

        m_PortalKey.transform.position = centralPosition;
        m_PortalKey.transform.rotation = Quaternion.identity;
        m_PortalKey.SetActive(true);
    }

    private void OnCollectibleCollectedCallback(Transform transform)
    {
        if (transform.CompareTag(Constants.TAG_PORTAL_KEY))
        {
            m_PortalKey.SetActive(false);
            m_PortalDoor.OpenPortalDoor();
        }

        if (transform.CompareTag(Constants.TAG_PORTAL_DOOR))
        {
            m_Player.Celebrate();
            NextRoom();
        }
    }
}