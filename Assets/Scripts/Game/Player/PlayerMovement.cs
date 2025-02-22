using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;

namespace Scripts.Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {

        // Inspector fields
        [Tooltip("Ease type for the movement tween")]
        [SerializeField] private DG.Tweening.Ease ease;

        [Tooltip("Rotation angle in degrees")]
        [SerializeField] private float rotateAngle = 90f;

        // Private members
        private bool m_MovementIsAllowed = false;
        public bool MovementIsAllowed { get => m_MovementIsAllowed; }

        // MonoBehaviour
        private void Awake() {
            Assert.IsTrue(System.Enum.IsDefined(typeof(DG.Tweening.Ease), ease), "ERROR: ease value is invalid!!!");
        }

        // Logic
        public void Move(Vector3 inputMovementVector)
        {
            if (m_MovementIsAllowed && inputMovementVector != Vector3.zero)
            {
                Vector3 nextTilePosition = transform.position + inputMovementVector;

                if (IsNextTileWalkable(inputMovementVector))
                {
                    DontAllowMovement();
                    transform.DOMove(nextTilePosition, Constants.PLAYER_MOVE_TIME)
                        .SetEase(ease)
                        .OnComplete(AllowMovement);
                }
            }
        }

        public void Rotate(Vector3 inputRotationVector)
        {
            if (m_MovementIsAllowed && inputRotationVector != Vector3.zero)
            {
                DontAllowMovement();
                Vector3 newRotation = transform.rotation.eulerAngles + rotateAngle * inputRotationVector;
                transform.DOLocalRotate(newRotation, Constants.PLAYER_ROTATE_TIME)
                    .SetEase(ease)
                    .OnComplete(AllowMovement);
            }
        }

        public void AllowMovement()
        {
            m_MovementIsAllowed = true;
        }

        public void DontAllowMovement()
        {
            m_MovementIsAllowed = false;
        }

        private bool IsNextTileWalkable(Vector3 nextTileDirection)
        {
            SquareInfo squareInfo = GetSquareInfo(nextTileDirection, 1.0f);
            return (null == squareInfo || squareInfo.IsWalkable);
        }

        private SquareInfo GetSquareInfo(Vector3 vectorDirection, float distance)
        {
            SquareInfo squareInfo = null;
            RaycastHit hit;
            Physics.Raycast(transform.position + Constants.SELF_RAYCAST_ORIGIN, vectorDirection, out hit, distance);
            if (null != hit.transform)
            {
                squareInfo = hit.transform.GetComponent<SquareInfo>();
            }
            return squareInfo;
        }
    }
}