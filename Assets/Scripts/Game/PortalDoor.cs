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
    private bool isDoorOpen = false;

    private void Awake() {
        Assert.IsNotNull(door, "ERROR: door not set in PortalDoor");

        portalSquareInfo = GetComponent<SquareInfo>();
    }

    public void Reset()
    {
        ClosePortalDoor();
    }

    public void OpenPortalDoor()
    {
        if (!isDoorOpen)
        {
            isDoorOpen = true;
            door.transform.DOScale(new Vector3(1.0f, 0.0f, 1.0f), timeToOpenDoor)
                .OnComplete(PortalDoorOpened);
        }
    }

    private void PortalDoorOpened()
    {
        portalSquareInfo.IsWalkable = true;
        portalSquareInfo.IsCollectible = true;
    }

    public void ClosePortalDoor()
    {
        if (isDoorOpen)
        {
            isDoorOpen = false;
            portalSquareInfo.IsWalkable = false;
            portalSquareInfo.IsCollectible = false;
            door.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), timeToOpenDoor);
        }
    }
}
