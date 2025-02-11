using System.Collections.Generic;
using NUnit.Framework;
using Scripts.Game.Stones;
using Scripts.Game.Stones.StonePool;
using UnityEngine;

[RequireComponent(typeof(PositionManager))]
[RequireComponent(typeof(KeyStoneManager))]
public class StoneManager : MonoBehaviour
{
    [Header("Number of Stones")]
    [SerializeField] int m_KeyStonesNumber = 3;
    [SerializeField] int m_DynamicStonesNumber = 8;
    [SerializeField] int m_StaticStonesNumber = 4;

    [Header("Stone Pools")]
    [SerializeField] private StonePool m_KeyStonesPool;
    [SerializeField] private StonePool m_DynamicStonesPool;
    [SerializeField] private StonePool m_StaticStonesPool;

    private PositionManager m_PositionManager;
    private KeyStoneManager m_KeyStoneManager;
    public KeyStoneManager KeyStoneManager { get => m_KeyStoneManager; }

    private List<PooledStone> m_Stones;
    private List<PooledStone> m_KeyStones;
    private List<PooledStone> m_DynamicStones;
    private List<PooledStone> m_StaticStones;


    private void Awake() {
        m_Stones = new List<PooledStone>();
        m_KeyStones = new List<PooledStone>();
        m_DynamicStones = new List<PooledStone>();
        m_StaticStones = new List<PooledStone>();

        m_PositionManager = GetComponent<PositionManager>();
        m_KeyStoneManager = GetComponent<KeyStoneManager>();
    }

    private void Start() {
        CreateStones();
    }

    private void CreateStones()
    {
        m_KeyStones = CreateStones(m_KeyStonesNumber, m_KeyStonesPool, true);
        m_Stones.AddRange(m_KeyStones);
        KeyStoneManager.SetKeyStones(m_KeyStones);

        m_DynamicStones = CreateStones(m_DynamicStonesNumber, m_DynamicStonesPool, true);
        m_Stones.AddRange(m_DynamicStones);

        m_StaticStones = CreateStones(m_StaticStonesNumber, m_StaticStonesPool, false);
        m_Stones.AddRange(m_StaticStones);
    }

    private List<PooledStone> CreateStones(int stonesNumber, StonePool stonePool, bool isCentered = false)
    {
        List<PooledStone> stoneList = new List<PooledStone>();

        for (int i = 1; i <= stonesNumber; i++)
        {
            if (m_PositionManager.IsAnyFreePosition())
            {
                Vector3 newFreeRandomPosition = m_PositionManager.GetRandomFreePosition(isCentered);
                PooledStone newStone = stonePool.GetPooledStone();
                newStone.transform.position = newFreeRandomPosition;
                newStone.transform.rotation = Quaternion.identity;
                newStone.transform.parent = transform;
                stoneList.Add(newStone);
            }
            else
            {
                Debug.Log("ERROR: No more free positions to create stones");
            }
        }
        return stoneList;
    }

    public void ResetStones()
    {
        foreach (PooledStone stone in m_Stones)
        {
            stone.Release();
        }
        m_Stones.Clear();
        m_PositionManager.ResetFreePositions();
        CreateStones();
    }
}