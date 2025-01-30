using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class KeyStonesManager : MonoBehaviour
{
    [SerializeField] StoneHitManager[] specialStones;
    private Vector3 centralPosition;

    public event Action<Vector3> OnKeyStonesAligned;

    private void Awake() {
        Assert.IsNotNull(specialStones, "ERROR: specialStones not set in RoomManager");

        foreach (StoneHitManager stoneHitManager in specialStones)
        {
            Assert.IsNotNull(stoneHitManager, "ERROR: some stone in dynamicStones not set in RoomManager");
            stoneHitManager.OnStoneStop += HandleOnStoneStop;
        }
    }

    private void HandleOnStoneStop(bool isSpecial)
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
            Debug.Log("Special stones in a row!!!");
            centralPosition = new Vector3(xPositions[1], 0.0f, zPositions[1]);
            foreach (StoneHitManager stoneHitManager in specialStones) Destroy(stoneHitManager.gameObject);
            OnKeyStonesAligned?.Invoke(centralPosition);
        }
    }
}
