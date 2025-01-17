using System;
using UnityEngine;
using DG.Tweening;

public class Hittable : MonoBehaviour
{
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
                    transform.DOMove(intDistance * hitterTransform.forward + transform.position, intDistance * Constants.STONE_MOVE_TIME);
                }
            }
        }
    }
}
