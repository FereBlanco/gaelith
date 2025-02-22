using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Scripts.Game.Stones
{
    public class PositionManager : MonoBehaviour
    {
        [Header("Board Size")]
        [SerializeField] int m_xMax = 7;
        [SerializeField] int m_zMax = 7;

        [Header("Ignored Game Objects Position")]
        [SerializeField] private Transform[] m_IgnoreGameObjectsPosition;
        List<Vector3> m_InitialFreePositions;
        List<Vector3> m_CurrentFreePositions;

        // MonoBehaviour
        private void Awake()
        {
            Assert.IsNotNull(m_IgnoreGameObjectsPosition, "ERROR: m_IgnoreGameObjectsPosition not set in PositionManager");

            foreach (Transform ignoreGameObjectPosition in m_IgnoreGameObjectsPosition)
            {
                if (null == ignoreGameObjectPosition)
                {
                    throw new Exception("ERROR: m_IgnoreGameObjectsPosition contains null elements in PositionManager");
                }
            }

            m_InitialFreePositions = new List<Vector3>();
            m_CurrentFreePositions = new List<Vector3>();

            Initialiaze();
        }

        // Initialize & Reset
        private void Initialiaze()
        {
            Reset();

            for (int x = 1; x <= m_xMax; x++)
            {
                for (int z = 1; z <= m_zMax; z++)
                {
                    bool ignore = false;

                    foreach (Transform ignoreGameObjectPosition in m_IgnoreGameObjectsPosition)
                    {
                        if (x == Mathf.RoundToInt(ignoreGameObjectPosition.position.x) &&
                            z == Mathf.RoundToInt(ignoreGameObjectPosition.position.z))
                        {
                            ignore = true;
                            break;
                        }
                    }

                    if (!ignore)
                    {
                        Vector3 newFreePosition = new Vector3(x, 0f, z);
                        m_InitialFreePositions.Add(newFreePosition);
                    }
                }
            }

            foreach (Vector3 initialFreePosition in m_InitialFreePositions)
            {
                m_CurrentFreePositions.Add(initialFreePosition);
            }
        }

        public void Reset()
        {
            m_InitialFreePositions.Clear();
            m_CurrentFreePositions.Clear();
        }

        // Logic
        public bool IsAnyFreePosition()
        {
            return m_CurrentFreePositions.Count > 0;
        }

        public Vector3 GetRandomFreePosition(bool isCentered)
        {
            int randomIndex;
            Vector3 randomPosition;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, m_CurrentFreePositions.Count);
                randomPosition = m_CurrentFreePositions[randomIndex];
            } while (isCentered && (randomPosition.x == 1 || randomPosition.x == m_xMax || randomPosition.z == 1 || randomPosition.z == m_zMax));

            m_CurrentFreePositions.RemoveAt(randomIndex);

            return randomPosition;
        }
    }
}
