using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace Scripts.Game.Stones
{
    public class KeyStoneManager : MonoBehaviour
    {
        [SerializeField] List<StoneHitManager> specialStones;
        private Vector3 centralPosition;
        private List<int> xPositions;
        private List<int> zPositions;

        private float timeToStonesTogetherFX = 0.5f;
        private float stonesTogetherHeight = 2.0f;

        public event Action<Vector3> OnKeyStonesAligned;

        private void Awake() {
            xPositions = new List<int>();
            zPositions = new List<int>();
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
            xPositions.Clear();
            zPositions.Clear();

            foreach (StoneHitManager stoneHitManager in specialStones)
            {
                xPositions.Add(Mathf.RoundToInt(stoneHitManager.transform.position.x));
                zPositions.Add(Mathf.RoundToInt(stoneHitManager.transform.position.z));
            }

            xPositions.Sort();
            zPositions.Sort();

            bool areStonesInARowInXAxis = (xPositions[0] == xPositions[2] && zPositions[1] == zPositions[0] + 1 && zPositions[2] == zPositions[1] + 1);
            bool areStonesInARowInZAxis = (zPositions[0] == zPositions[2] && xPositions[1] == xPositions[0] + 1 && xPositions[2] == xPositions[1] + 1);

            return (areStonesInARowInXAxis || areStonesInARowInZAxis);
        }

        IEnumerator StonesComeTogetherFX()
        {
            centralPosition = new Vector3(xPositions[1], stonesTogetherHeight, zPositions[1]);

            foreach (StoneHitManager stoneHitManager in specialStones)
            {
                stoneHitManager.GetComponent<Collider>().enabled = false;
                stoneHitManager.transform.DOMove(stoneHitManager.transform.position + stonesTogetherHeight * Vector3.up,
                            timeToStonesTogetherFX / 2)
                            .SetEase(Ease.OutCirc);
            }
            yield return new WaitForSeconds(timeToStonesTogetherFX / 2);

            foreach (StoneHitManager stoneHitManager in specialStones)
            {
                stoneHitManager.transform.DOMove(centralPosition,
                            timeToStonesTogetherFX / 2)
                            .SetEase(Ease.OutCirc);
            }
            yield return new WaitForSeconds(timeToStonesTogetherFX / 2);

            OnKeyStonesAligned?.Invoke(centralPosition);
            DestroyStones();
        }

        private void DestroyStones()
        {
            foreach (StoneHitManager stoneHitManager in specialStones)
            {
                Destroy(stoneHitManager.gameObject);
            }
        }

        public void SetKeyStones(List<GameObject> keyStones)
        {
            foreach (GameObject keyStone in keyStones)
            {
                StoneHitManager stoneHit = keyStone.GetComponent<StoneHitManager>();
                specialStones.Add(stoneHit);
                Assert.IsNotNull(keyStone, "ERROR: some keyStone has no StoneHitManager");
                stoneHit.OnStoneStop += HandleOnStoneStop;
            }
        }
    }
}