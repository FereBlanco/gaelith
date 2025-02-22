using UnityEngine;

namespace Scripts.Game.Stones.ObjectPool
{
    public class PooledStone : MonoBehaviour
    {
        private StonePool pool;
        public StonePool Pool { get => pool; set => pool = value; }

        public void Release()
        {
            pool.ReturnToPool(this);
        }

    }
}
