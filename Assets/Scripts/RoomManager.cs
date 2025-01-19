using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] PlayerControlManager player;
    [SerializeField] StoneHitManager[] dynamicStones;
    [SerializeField] StoneHitManager[] specialStones;
    [SerializeField] GameObject portalKey;

    private void Awake() {
        Assert.IsNotNull(player, "ERROR: player not set in RoomManager");
        Assert.IsNotNull(dynamicStones, "ERROR: dynamicStones not set in RoomManager");
        Assert.IsNotNull(specialStones, "ERROR: specialStones not set in RoomManager");
        Assert.IsNotNull(portalKey, "ERROR: portalKey not set in RoomManager");

        foreach (StoneHitManager stoneHitManager in dynamicStones)
        {
            Assert.IsNotNull(stoneHitManager, "ERROR: some stone in dynamicStones not set in RoomManager");
            stoneHitManager.OnStoneStop += HandleOnStoneStop;
        }

        foreach (StoneHitManager stoneHitManager in specialStones)
        {
            Assert.IsNotNull(stoneHitManager, "ERROR: some stone in dynamicStones not set in RoomManager");
            stoneHitManager.OnStoneStop += HandleOnStoneStop;
        }
    }

    private void HandleOnStoneStop(bool isSpecial)
    {
        if (isSpecial)
        {
            CheckSpecialStonesInARow();
        }
        else
        {
            player.AllowMovement();
        }
    }

    private void CheckSpecialStonesInARow()
    {
        List<int> xPositions = new List<int>();
        List<int> zPositions = new List<int>();

        foreach (StoneHitManager stoneHitManager in specialStones)
        {
            xPositions.Add(Mathf.RoundToInt(stoneHitManager.transform.position.x));
            zPositions.Add(Mathf.RoundToInt(stoneHitManager.transform.position.z));
        }
        xPositions.Sort();
        zPositions.Sort();

        if ((xPositions[0] == xPositions[2] && zPositions[1] == zPositions[0] + 1 && zPositions[2] == zPositions[1] + 1) ||
            (zPositions[0] == zPositions[2] && xPositions[1] == xPositions[0] + 1 && xPositions[2] == xPositions[1] + 1))
        {
            foreach (StoneHitManager stoneHitManager in specialStones) Destroy(stoneHitManager.gameObject);
            Instantiate(portalKey, new Vector3(xPositions[1], 0f, zPositions[1]), Quaternion.identity);
        }

        player.AllowMovement();
    }
}
