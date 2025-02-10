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
        Debug.Log("StoneManager Awake");
        m_Stones = new List<PooledStone>();
        m_KeyStones = new List<PooledStone>();
        m_DynamicStones = new List<PooledStone>();
        m_StaticStones = new List<PooledStone>();

        m_PositionManager = GetComponent<PositionManager>();
        m_KeyStoneManager = GetComponent<KeyStoneManager>();
    }

    private void Start() {
        CreateStones();
        m_KeyStoneManager.SetKeyStones(m_KeyStones);
    }

    private void CreateStones()
    {
        Debug.Log("StoneManager CreateStones");

        m_KeyStones = CreateStones(m_KeyStonesNumber, m_KeyStonesPool);
        m_Stones.AddRange(m_KeyStones);
        KeyStoneManager.SetKeyStones(m_KeyStones);

        m_DynamicStones = CreateStones(m_DynamicStonesNumber, m_DynamicStonesPool);
        m_Stones.AddRange(m_DynamicStones);

        m_StaticStones = CreateStones(m_StaticStonesNumber, m_StaticStonesPool);
        m_Stones.AddRange(m_StaticStones);
    }

    private List<PooledStone> CreateStones(int stonesNumber, StonePool stonePool)
    {
        Debug.Log("StoneManager CreateStones: " + stonePool.name);
        List<PooledStone> stoneList = new List<PooledStone>();

        Debug.Log("StoneManager CreateStones 1");
        for (int i = 1; i <= stonesNumber; i++)
        {
            Debug.Log("StoneManager CreateStones 2");
            if (m_PositionManager.IsAnyFreePosition())
            {
                Debug.Log("StoneManager CreateStones 3");
                Vector3 newFreeRandomPosition = m_PositionManager.GetRandomFreePosition();
                Debug.Log("StoneManager CreateStones 4");
                PooledStone newStone = stonePool.GetPooledStone();
                Debug.Log("StoneManager CreateStones 5");
                newStone.transform.position = newFreeRandomPosition;
                Debug.Log("StoneManager CreateStones 6");
                newStone.transform.rotation = Quaternion.identity;
                Debug.Log("StoneManager CreateStones 7");
                newStone.transform.parent = transform;
                Debug.Log("StoneManager CreateStones 8");
                stoneList.Add(newStone);
                Debug.Log("StoneManager CreateStones 9");
            }
            else
            {
                Debug.Log("ERROR: No more free positions to create stones");
            }
        }
        Debug.Log("StoneManager CreateStones 10");

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