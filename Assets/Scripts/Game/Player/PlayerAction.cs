using UnityEngine;
using DG.Tweening;
using Scripts.Game.Stones;
using UnityEngine.Assertions;

namespace Scripts.Game.Player
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerAction : MonoBehaviour
    {
        // Inspector fields
        [Header("Player Action PULL")]
        [Tooltip("Angle of inclination of the player's body when executing the pushing action")]
        [SerializeField] private float m_PullAngle = 20f;
        
        // Private members
        private Player m_Player;


        // MonoBehaviour
        private void Awake() {
            m_Player = GetComponent<Player>();
            Assert.IsTrue(m_Player, "ERROR: m_Player value not found in PlayerAction class");
        }

        // Public Methods
        public void Push(Vector3 inputPullVector)
        {
            if (m_Player.IsInteractionAllowed && inputPullVector != Vector3.zero)
            {
                m_Player.DontAllowInteraction();
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(transform.DORotate(new Vector3(transform.eulerAngles.x + m_PullAngle, transform.eulerAngles.y, transform.eulerAngles.z), Constants.PLAYER_PUSH_TIME/2));
                mySequence.Append(transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z), Constants.PLAYER_PUSH_TIME/2)
                    .OnComplete(m_Player.AllowInteraction));

                RaycastHit hit;
                if (Physics.Raycast(transform.position + Constants.SELF_RAYCAST_ORIGIN, inputPullVector, out hit, 1f))
                {
                    if (null != hit.transform)
                    {
                        StoneHitManager hittable = hit.transform.GetComponent<StoneHitManager>();
                        if (null != hittable)
                        {
                            hittable.Hit(transform);
                        }
                    }
                }
            }
        }
    }
}
