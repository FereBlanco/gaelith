using UnityEngine;
using DG.Tweening;

namespace Scripts.Game.Stones
{
    public class StoneHitManager : MonoBehaviour
    {
        [SerializeField] private bool isSpecial = false;

        public DG.Tweening.Ease ease;

        public void Hit(Transform hitterTransform)
        {
            RaycastHit limitHit;
            if (Physics.Raycast(transform.position + Constants.SELF_RAYCAST_ORIGIN, transform.TransformDirection(hitterTransform.forward), out limitHit, Mathf.Infinity))
            {
                if (limitHit.transform != null)
                {
                    int intDistance = Mathf.FloorToInt(limitHit.distance);
                    if (intDistance > 0)
                    {
                        transform.DOMove(intDistance * hitterTransform.forward + transform.position,
                            intDistance * Constants.STONE_MOVE_TIME)
                            .SetEase(Ease.OutCirc)
                            .OnComplete(LaunchOnStoneStopEvent);
                    }
                }
            }
        }

        private void LaunchOnStoneStopEvent()
        {
            EventHandler.RaiseOnStoneStop(isSpecial);
        }
    }
}