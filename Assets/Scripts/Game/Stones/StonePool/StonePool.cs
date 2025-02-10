using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Scripts.Game.Stones.StonePool
{
    public class StonePool : MonoBehaviour
    {
        [SerializeField] private int m_InitPoolSize = 0;
        [SerializeField] private PooledStone m_StoneToPool;

        private Stack<PooledStone> m_StoneStack;

        private void Awake()
        {
            Debug.Log("Awake");
            Assert.IsNotNull(m_StoneToPool, "ERROR: m_StoneStack not assigned in StonePool class");

            SetupPool();
        }

        public void SetupPool()
        {
            m_StoneStack = new Stack<PooledStone>();
            PooledStone instance = null;

            for (int i = 0; i < m_InitPoolSize; i++)
            {
                instance = Instantiate(m_StoneToPool);
                instance.Pool = this;
                instance.gameObject.SetActive(false);
                m_StoneStack.Push(instance);
            }
        }

        public PooledStone GetPooledStone()
        {
            Debug.Log("GetPooledStone 1");
            // if the pool is not large enough, instantiate a new object
            if (m_StoneStack.Count == 0)
            {
                Debug.Log("GetPooledStone 2");
                PooledStone newInstance = Instantiate(m_StoneToPool);
                Debug.Log("GetPooledStone 3");
                newInstance.Pool = this;
                Debug.Log("GetPooledStone 4");
                return newInstance;
            }

            Debug.Log("GetPooledStone 5");
            // otherwise, just grab the next one from the list
            PooledStone nextInstance = m_StoneStack.Pop();
            Debug.Log("GetPooledStone 6");
            nextInstance.gameObject.SetActive(true);
            Debug.Log("GetPooledStone 7");
            return nextInstance;
        }

        public void ReturnToPool(PooledStone pooledStone)
        {
            m_StoneStack.Push(pooledStone);
            pooledStone.gameObject.SetActive(false);
        }
    }
}