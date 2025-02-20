using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Scripts.Game.Stones.StonePool;

namespace Scripts.Game.Stones
{
    public class KeyStoneManager : MonoBehaviour
    {
        private List<StoneHitManager> m_KeyStones;
        private Vector3 m_CentralPosition;
        private List<int> m_XPositions;
        private List<int> m_ZPositions;

        private float m_TimeToStonesTogetherFX = 0.5f;
        private float m_StonesTogetherHeight = 2.0f;

        private void Awake() {
            m_XPositions = new List<int>();
            m_ZPositions = new List<int>();

            m_KeyStones = new List<StoneHitManager>();
        }

        private void HandleOnStoneStop(bool isSpecial)
        {
            if (AreStonesInARow())
            {
                StartCoroutine(StonesComeTogetherFX());
            }
        }

        private bool AreStonesInARow()
        {
            m_XPositions.Clear();
            m_ZPositions.Clear();

            foreach (StoneHitManager stoneHitManager in m_KeyStones)
            {
                m_XPositions.Add(Mathf.RoundToInt(stoneHitManager.transform.position.x));
                m_ZPositions.Add(Mathf.RoundToInt(stoneHitManager.transform.position.z));
            }

            m_XPositions.Sort();
            m_ZPositions.Sort();

            bool areStonesInARowInXAxis = (m_XPositions[0] == m_XPositions[2] && m_ZPositions[1] == m_ZPositions[0] + 1 && m_ZPositions[2] == m_ZPositions[1] + 1);
            bool areStonesInARowInZAxis = (m_ZPositions[0] == m_ZPositions[2] && m_XPositions[1] == m_XPositions[0] + 1 && m_XPositions[2] == m_XPositions[1] + 1);

            return (areStonesInARowInXAxis || areStonesInARowInZAxis);
        }

        IEnumerator StonesComeTogetherFX()
        {
            m_CentralPosition = new Vector3(m_XPositions[1], m_StonesTogetherHeight, m_ZPositions[1]);

            foreach (StoneHitManager stoneHitManager in m_KeyStones)
            {
                stoneHitManager.GetComponent<Collider>().enabled = false;
                stoneHitManager.transform.DOMove(stoneHitManager.transform.position + m_StonesTogetherHeight * Vector3.up,
                            m_TimeToStonesTogetherFX / 2)
                            .SetEase(Ease.OutCirc);
            }
            yield return new WaitForSeconds(m_TimeToStonesTogetherFX / 2);

            foreach (StoneHitManager stoneHitManager in m_KeyStones)
            {
                stoneHitManager.transform.DOMove(m_CentralPosition,
                            m_TimeToStonesTogetherFX / 2)
                            .SetEase(Ease.OutCirc);
            }
            yield return new WaitForSeconds(m_TimeToStonesTogetherFX / 2);

            EventHandler.RaiseOnKeyStonesAligned(m_CentralPosition);
            HideStones();
        }

        private void HideStones()
        {
            foreach (StoneHitManager keyStone in m_KeyStones)
            {
                PooledStone pooledStone = keyStone.gameObject.GetComponent<PooledStone>();
                Assert.IsNotNull(keyStone, "ERROR: some keyStone has no PooledStone");
                pooledStone.Release();
            }
        }

        public void SetKeyStones(List<PooledStone> keyStones)
        {
            foreach (PooledStone keyStone in keyStones)
            {
                StoneHitManager stoneHit = keyStone.GetComponent<StoneHitManager>();
                m_KeyStones.Add(stoneHit);
                Assert.IsNotNull(keyStone, "ERROR: some keyStone has no StoneHitManager");
                stoneHit.OnStoneStop += HandleOnStoneStop;
            }
        }
    }
}