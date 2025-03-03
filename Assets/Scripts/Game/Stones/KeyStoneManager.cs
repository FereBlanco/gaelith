using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Scripts.Game.Stones.ObjectPool;

namespace Scripts.Game.Stones
{
    public class KeyStoneManager : MonoBehaviour
    {
        private List<PooledStone> m_KeyStones;
        private bool m_AreStonesInARow;
        private Vector3 m_CentralPosition;
        private List<int> m_XPositions;
        private List<int> m_ZPositions;
        private float m_TimeToStonesTogetherFX = 0.5f;
        private float m_StonesTogetherHeight = 2.0f;

        // MonoBehaviour
        private void Awake() {
            m_XPositions = new List<int>();
            m_ZPositions = new List<int>();

            m_KeyStones = new List<PooledStone>();

            m_AreStonesInARow = false;

            EventHandler.OnStoneStop += HandleOnStoneStop;
        }

        // Initialize & Reset
        public void Initialize(List<PooledStone> keyStones)
        {
            foreach (PooledStone keyStone in keyStones)
            {
                keyStone.GetComponent<Collider>().enabled = true;
                m_KeyStones.Add(keyStone);
                StoneHitManager stoneHit = keyStone.GetComponent<StoneHitManager>();
                Assert.IsNotNull(stoneHit, "ERROR: some keyStone has no StoneHitManager in class KeyStoneManager");
            }
        }

        public void Reset()
        {
            foreach (PooledStone keyStone in m_KeyStones)
            {
                // StoneHitManager stoneHit = keyStone.GetComponent<StoneHitManager>();
                // if (null != stoneHit)
                // {
                //     EventHandler.OnStoneStop -= HandleOnStoneStop;
                // }
                keyStone.Release();
            }

            m_XPositions.Clear();
            m_ZPositions.Clear();
            m_KeyStones.Clear();      
        }

        // Logic
        private void HandleOnStoneStop(bool isSpecial)
        {
            CheckStonesInARow();
            if (m_AreStonesInARow)
            {
                StartCoroutine(StonesComeTogetherFX());
            }
        }

        private void CheckStonesInARow()
        {
            m_XPositions.Clear();
            m_ZPositions.Clear();

            foreach (PooledStone keyStone in m_KeyStones)
            {
                m_XPositions.Add(Mathf.RoundToInt(keyStone.transform.position.x));
                m_ZPositions.Add(Mathf.RoundToInt(keyStone.transform.position.z));
            }

            m_XPositions.Sort();
            m_ZPositions.Sort();

            bool areStonesInARowInXAxis = (m_XPositions[0] == m_XPositions[2] && m_ZPositions[1] == m_ZPositions[0] + 1 && m_ZPositions[2] == m_ZPositions[1] + 1);
            bool areStonesInARowInZAxis = (m_ZPositions[0] == m_ZPositions[2] && m_XPositions[1] == m_XPositions[0] + 1 && m_XPositions[2] == m_XPositions[1] + 1);

            m_AreStonesInARow = areStonesInARowInXAxis || areStonesInARowInZAxis;
        }

        IEnumerator StonesComeTogetherFX()
        {
            m_CentralPosition = new Vector3(m_XPositions[1], m_StonesTogetherHeight, m_ZPositions[1]);

            foreach (PooledStone keyStone in m_KeyStones)
            {
                keyStone.GetComponent<Collider>().enabled = false;
                keyStone.transform.DOMove(keyStone.transform.position + m_StonesTogetherHeight * Vector3.up,
                            m_TimeToStonesTogetherFX / 2)
                            .SetEase(Ease.OutCirc);
            }
            yield return new WaitForSeconds(m_TimeToStonesTogetherFX / 2);

            foreach (PooledStone keyStone in m_KeyStones)
            {
                keyStone.transform.DOMove(m_CentralPosition,
                            m_TimeToStonesTogetherFX / 2)
                            .SetEase(Ease.OutCirc);
            }
            yield return new WaitForSeconds((m_TimeToStonesTogetherFX / 2));

            EventHandler.RaiseOnKeyStonesAlign(m_CentralPosition);
            Reset();
        }
    }
}