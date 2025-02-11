using System;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    [Header("Board Size")]
    [SerializeField] int m_xMax = 7;
    [SerializeField] int m_zMax = 7;

    [Header("Ignored Game Objects Position")]
    [SerializeField] GameObject[] m_IgnoreGameObjectsPosition;
    List<Vector3> m_InitialFreePositions;
    List<Vector3> m_CurrentFreePositions;

    private void Awake()
    {
        m_InitialFreePositions = new List<Vector3>();
        m_CurrentFreePositions = new List<Vector3>();

        InitialiazeFreePositions();
        ResetFreePositions();
    }

    private void InitialiazeFreePositions()
    {
        for (int x = 1; x <= m_xMax; x++)
        {
            for (int z = 1; z <= m_zMax; z++)
            {
                bool ignore = false;

                foreach (GameObject ignoreGameObjectPosition in m_IgnoreGameObjectsPosition)
                {
                    if (x == Mathf.RoundToInt(ignoreGameObjectPosition.transform.position.x) &&
                        z == Mathf.RoundToInt(ignoreGameObjectPosition.transform.position.z))
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
    }

    public void ResetFreePositions()
    {
        m_CurrentFreePositions.Clear();
        foreach (Vector3 initialFreePosition in m_InitialFreePositions)
        {
            m_CurrentFreePositions.Add(initialFreePosition);
        }
    }

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