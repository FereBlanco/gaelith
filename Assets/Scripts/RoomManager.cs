using UnityEngine.Assertions;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] PlayerControlManager player;
    [SerializeField] StoneHitManager[] dynamicStones;
    [SerializeField] KeyStonesManager keyStonesManager;
    [SerializeField] GameObject portalKeyPrefab;
    GUIStyle style;


    private void Awake() {
        Assert.IsNotNull(player, "ERROR: player not set in RoomManager");
        Assert.IsNotNull(dynamicStones, "ERROR: dynamicStones not set in RoomManager");
        Assert.IsNotNull(keyStonesManager, "ERROR: keyStonesManager not set in RoomManager");
        Assert.IsNotNull(portalKeyPrefab, "ERROR: portalKey not set in RoomManager");

        style = new GUIStyle();
        style.richText = true;

        keyStonesManager.OnKeyStonesAligned += OnKeyStonesAlignedCallback;
    }

    private void OnKeyStonesAlignedCallback(Vector3 centralPosition)
    {
        GameObject portalKey = Instantiate(portalKeyPrefab, centralPosition, Quaternion.identity);
        portalKey.transform.tag = Constants.TAG_PORTAL_KEY;
        
        Collectible collectible = portalKey.AddComponent<Collectible>();
        collectible.OnCollectibleCollected += OnCollectibleCollectedCallback;
    }

    private void OnCollectibleCollectedCallback(Transform transform)
    {
        if (transform.CompareTag(Constants.TAG_PORTAL_KEY))
        {
            Debug.Log("Portal Key collected >> Open the PortalDoor");
        }
        Destroy(transform.gameObject);
    }

    private void OnGUI()
        {
            // GUI.Label(new Rect(10, 0, 1000, 100), $"<color=white><size=32>Distance between Stone01 and Stone02: {(dynamicStones[1].transform.position.x - dynamicStones[0].transform.position.x).ToString()} </size></color>", style);
            // GUI.Label(new Rect(10, 32, 1000, 100), $"<color=white><size=32>Distance between Stone03 and Stone04: {(dynamicStones[3].transform.position.x - dynamicStones[2].transform.position.x).ToString()} </size></color>", style);
            // GUI.Label(new Rect(10, 64, 1000, 100), $"<color=white><size=32>Distance between Stone05 and Stone06: {(dynamicStones[5].transform.position.x - dynamicStones[4].transform.position.x).ToString()} </size></color>", style);
        }
}
