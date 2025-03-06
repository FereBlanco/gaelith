using UnityEngine.Assertions;
using UnityEngine;
using Scripts.Game.Player;
using Scripts.Game.Stones;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class RoomManager : MonoBehaviour
{
    [Header("Player, Key and Door")]
    [SerializeField] private Player m_Player;
    [SerializeField] private GameObject m_PortalKey;
    [SerializeField] private PortalDoor m_PortalDoor;

    [Header("Number of Stones")]
    [SerializeField] private int m_KeyStonesNumber = 3;
    [SerializeField] private int m_DynamicStonesNumber = 2;
    [SerializeField] private int m_StaticStonesNumber = 1;
    private StoneManager m_StoneManager;

    private List<Transform> m_Skyables;

    bool isKeyStonesAlignSubscribed = false;

    private int m_RoomNumber;
    [SerializeField] private TMP_Text m_RoomNumberText;

    // MonoBehaviour
    private void Awake() {
        Assert.IsNotNull(m_Player, "ERROR: m_Player not set in RoomManager");
        Assert.IsNotNull(m_PortalKey, "ERROR: m_PortalKeyPrefab not set in RoomManager");
        Assert.IsNotNull(m_PortalDoor, "ERROR: m_PortalDoor not set in RoomManager");
        Assert.IsNotNull(m_RoomNumberText, "ERROR: m_RoomNumberText not set in RoomManager");

        m_StoneManager = GetComponentInChildren<StoneManager>();
        Assert.IsNotNull(m_StoneManager, "ERROR: m_StoneManager not found in RoomManager children");

        Collectible portalDoorCollectible = m_PortalDoor.GetComponent<Collectible>();
        Assert.IsNotNull(portalDoorCollectible, "ERROR: portalDoorCollectible not set in RoomManager");

        m_Skyables = new List<Transform>(); 

        m_RoomNumber = 1;
        ShowRoomNumber();

        EventHandler.OnCollectibleCollected += OnCollectibleCollectedCallback;
        Initialize();
    }

    // Initialize & Reset
    private void Initialize()
    {
        if (false == isKeyStonesAlignSubscribed)
        {
            EventHandler.OnKeyStonesAlign += OnKeyStonesAlignedCallback;
            isKeyStonesAlignSubscribed = true;
        }
    }

    // Logic
    public void LoadRoom()
    {
        ShowRoomNumber();
        m_StoneManager.Reset();
        m_PortalDoor.ClosePortalDoor();
        m_PortalKey.SetActive(false);
        m_StoneManager.Initialize(m_KeyStonesNumber, m_DynamicStonesNumber, m_StaticStonesNumber);
        m_Player.Initialize();
        ElementsToGround();
        StartCoroutine(StartRoomInteractions());
    }

    public void NextRoom()
    {
        IncreaseRoomValues();
        StartCoroutine(ResetNextRoomSequence());
    }

    public void NextRoomFromMenu()
    {
        m_Player.Reset();
        IncreaseRoomValues();
        StartCoroutine(ResetRoomSequence());
    }

    private void IncreaseRoomValues()
    {
        m_RoomNumber++;
        m_DynamicStonesNumber += 2;
        m_StaticStonesNumber += 1;
    }

    private void ShowRoomNumber()
    {
        m_RoomNumberText.text = $"{Constants.TEXT_ROOM_NUMBER} {m_RoomNumber}";
    }

    IEnumerator ResetNextRoomSequence()
    {
        yield return new WaitForSeconds(Constants.CELEBRATION_TIME);
        StartCoroutine(ResetRoomSequence());
    }

    public void ResetRoom()
    {
        m_Player.Reset();
        StartCoroutine(ResetRoomSequence());
    }

    IEnumerator ResetRoomSequence()
    {
        ElementsToSky();
        yield return new WaitForSeconds(Constants.FLY_TIME);
        LoadRoom();
    }

    IEnumerator StartRoomInteractions()
    {
        yield return new WaitForSeconds(Constants.FALL_TIME);
        m_Player.AllowInteraction();
    }

    private void ElementsToSky()
    {
        m_Skyables.Clear();

        m_Skyables.Add(m_Player.transform);
        if (m_PortalKey.gameObject.activeSelf) m_Skyables.Add(m_PortalKey.transform);

        m_Skyables.AddRange(m_StoneManager.GetActiveStones());

        foreach (Transform skyableTransform in m_Skyables)
        {
            Skyable skyable = skyableTransform.GetComponent<Skyable>();
            Assert.IsNotNull(skyable, $"ERROR: Skyable component not found in {skyableTransform.name} stone");
            skyable.FlyToSky();
        }
    }

    private void ElementsToGround()
    {
        m_Skyables.Clear();

        m_Skyables.Add(m_Player.transform);
        if (m_PortalKey.gameObject.activeSelf) m_Skyables.Add(m_PortalKey.transform);

        m_Skyables.AddRange(m_StoneManager.GetActiveStones());

        foreach (Transform skyableTransform in m_Skyables)
        {
            Skyable skyable = skyableTransform.GetComponent<Skyable>();
            Assert.IsNotNull(skyable, $"ERROR: Skyable component not found in {skyableTransform.name} stone");
            skyable.KeepInitialPosition();
            skyable.ResetToSky();
            skyable.FallToGround();
        }
    }

    // Callbacks
    private void OnKeyStonesAlignedCallback(Vector3 centralPosition)
    {
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