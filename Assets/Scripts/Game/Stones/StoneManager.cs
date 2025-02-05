using System.Collections.Generic;
using NUnit.Framework;
using Scripts.Game.Stones;
using UnityEngine;

[RequireComponent(typeof(PositionManager))]
[RequireComponent(typeof(KeyStoneManager))]
public class StoneManager : MonoBehaviour
{
    [Header("Number of Stones")]
    [SerializeField] int m_StoneSpecialNumber = 3;
    [SerializeField] int m_StoneDynamicNumber = 8;
    [SerializeField] int m_StoneStaticNumber = 4;

    [Header("Stone Prefabs")]
    [SerializeField] GameObject m_StoneSpecialPrefab;
    [SerializeField] GameObject m_StoneDynamicPrefab;
    [SerializeField] GameObject m_StoneStaticPrefab;

    private PositionManager m_PositionManager;
    private KeyStoneManager m_KeyStoneManager;
    public KeyStoneManager KeyStoneManager { get => m_KeyStoneManager; }

    private List<GameObject> m_Stones;
    private List<GameObject> m_KeyStones;
    private List<GameObject> m_DynamicStones;
    private List<GameObject> m_StaticStones;


    private void Awake() {
        Assert.IsNotNull(m_StoneSpecialPrefab, "ERROR: m_StoneSpecialPrefab not set in StonesManager");
        Assert.IsNotNull(m_StoneDynamicPrefab, "ERROR: m_StoneDynamic not set in StonesManager");
        Assert.IsNotNull(m_StoneStaticPrefab, "ERROR: m_StoneStatic not set in StonesManager");

        m_Stones = new List<GameObject>();
        m_KeyStones = new List<GameObject>();
        m_DynamicStones = new List<GameObject>();
        m_StaticStones = new List<GameObject>();

        m_PositionManager = GetComponent<PositionManager>();
        m_KeyStoneManager = GetComponent<KeyStoneManager>();
    }

    private void Start() {
        CreateStones();
        m_KeyStoneManager.SetKeyStones(m_KeyStones);
    }

    private void CreateStones()
    {
       m_KeyStones = CreateSingleStone(m_StoneSpecialNumber, m_StoneSpecialPrefab);
       m_Stones.AddRange(m_KeyStones);

       m_DynamicStones = CreateSingleStone(m_StoneDynamicNumber, m_StoneDynamicPrefab);
       m_Stones.AddRange(m_DynamicStones);

       m_StaticStones = CreateSingleStone(m_StoneStaticNumber, m_StoneStaticPrefab);
       m_Stones.AddRange(m_StaticStones);
    }

    private List<GameObject> CreateSingleStone(int stonesNumber, GameObject stonePrefab)
    {
        List<GameObject> stoneList = new List<GameObject>();

        for (int i = 1; i <= stonesNumber; i++)
        {
            if (m_PositionManager.IsAnyFreePosition())
            {
                Vector3 newFreeRandomPosition = m_PositionManager.GetRandomFreePosition();
                GameObject newStone = Instantiate(stonePrefab, newFreeRandomPosition, Quaternion.identity);
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
        foreach (GameObject stone in m_Stones)
        {
            Destroy(stone);
        }
        m_Stones.Clear();
        m_PositionManager.ResetFreePositions();
        CreateStones();
    }
}