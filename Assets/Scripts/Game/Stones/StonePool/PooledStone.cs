using UnityEngine;

namespace Scripts.Game.Stones.StonePool
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
