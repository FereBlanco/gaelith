using System.Collections;
using UnityEngine;
using DG.Tweening;
using NUnit.Framework;

[RequireComponent(typeof(SquareInfo))]
public class PortalDoor : MonoBehaviour
{
    [SerializeField] float timeToOpenDoor = 2.0f;
    [SerializeField] GameObject door;
    private SquareInfo portalSquareInfo;

    private void Awake() {
        Assert.IsNotNull(door, "ERROR: door not set in PortalDoor");

        portalSquareInfo = GetComponent<SquareInfo>();
    }

    public void OpenPortalDoor()
    {
        door.transform.DOScale(new Vector3(1.0f, 0.0f, 1.0f), timeToOpenDoor)
            .OnComplete(PortalDoorOpened);
    }

    private void PortalDoorOpened()
    {
        portalSquareInfo.IsWalkable = true;
        portalSquareInfo.IsCollectible = true;
    }
}
