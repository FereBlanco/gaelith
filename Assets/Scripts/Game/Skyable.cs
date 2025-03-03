using UnityEngine;
using DG.Tweening;

public class Skyable : MonoBehaviour
{
    private Vector3 m_InitialPosition;
    private float m_GroundLevel = 0.0f;

    public void KeepInitialPosition()
    {
        m_InitialPosition = new Vector3(transform.position.x, m_GroundLevel, transform.position.z);
    }

    public void ResetToSky()
    {
        transform.DOMove(transform.position + Constants.SKY_LEVEL * Vector3.up, 0f);
    }

    public void FlyToSky()
    {
        transform.DOMove(transform.position + Constants.SKY_LEVEL * Vector3.up, Constants.FLY_TIME).SetEase(Ease.OutCirc);
    }

    public void FallToGround()
    {
        transform.DOMove(m_InitialPosition, Constants.FALL_TIME).SetEase(Ease.OutCirc);
    }
}
