using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Scripts.Game.Stones.ObjectPool;

namespace Scripts.Game.Stones
{
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

        // MonoBehaviour
        private void Awake()
        {
            m_PositionManager = GetComponent<PositionManager>();
            m_KeyStoneManager = GetComponent<KeyStoneManager>();
            Assert.IsNotNull(m_PositionManager, "ERROR: m_PositionManager not set in StoneManager");
            Assert.IsNotNull(m_KeyStoneManager, "ERROR: m_KeyStoneManager not set in StoneManager");

            Assert.IsNotNull(m_KeyStonesPool, "ERROR: m_KeyStonesPool not set in StoneManager");
            Assert.IsNotNull(m_DynamicStonesPool, "ERROR: m_DynamicStonesPool not set in StoneManager");
            Assert.IsNotNull(m_StaticStonesPool, "ERROR: m_StaticStonesPool not set in StoneManager");

            m_Stones = new List<PooledStone>();
            m_KeyStones = new List<PooledStone>();
            m_DynamicStones = new List<PooledStone>();
            m_StaticStones = new List<PooledStone>();
        }

        // Initialize & Reset
        internal void Initialize()
        {
            m_KeyStones = CreateStones(m_KeyStonesNumber, m_KeyStonesPool, true);
            m_Stones.AddRange(m_KeyStones);
            KeyStoneManager.Initialize(m_KeyStones);

            m_DynamicStones = CreateStones(m_DynamicStonesNumber, m_DynamicStonesPool, true);
            m_Stones.AddRange(m_DynamicStones);

            m_StaticStones = CreateStones(m_StaticStonesNumber, m_StaticStonesPool, false);
            m_Stones.AddRange(m_StaticStones);
        }

        public void Reset()
        {
            foreach (PooledStone stone in m_Stones)
            {
                stone.Release();
            }
            m_Stones.Clear();
            m_KeyStones.Clear();
            m_DynamicStones.Clear();
            m_StaticStones.Clear();

            m_PositionManager.Reset();
            KeyStoneManager.Reset();
        }

        // Logic
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
    }
}