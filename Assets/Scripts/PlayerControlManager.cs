using UnityEngine;
using DG.Tweening;
using NUnit.Framework;

public class PlayerControlManager : MonoBehaviour
{

    private bool canMove = true;

    [SerializeField] private DG.Tweening.Ease ease;

    private RaycastHit hit;

    private float rotateAngle = 90f;
    private float pushAngle = 20f;


    private void Awake() {
        Assert.IsNotNull(ease, "ERROR: ease value is empty!!!");
    }

    private void Update()
    {
        if (canMove && Input.GetKeyUp(KeyCode.UpArrow)) MoveForward();
        if (canMove && Input.GetKeyUp(KeyCode.DownArrow)) MoveBackward();
        if (canMove && Input.GetKeyUp(KeyCode.LeftArrow)) RotateLeft();
        if (canMove && Input.GetKeyUp(KeyCode.RightArrow)) RotateRight();

        if (Input.GetKeyUp(KeyCode.Space)) Hit();
    }

    private void DontAllowMovement()
    {
        canMove = false;
    }

    public void AllowMovement()
    {
        canMove = true;
    }

    private bool IsFrontTileWalkable()
    {
        RaycastHit hit;
        bool frontTileNotWalkable = Physics.Raycast(transform.position + Constants.SELF_RAYCAST_ORIGIN, transform.TransformDirection(Vector3.forward), out hit, 1f);
        return !frontTileNotWalkable;
    }

    private bool IsBackTileWalkable()
    {
        RaycastHit hit;
        bool backTileNotWalkable = Physics.Raycast(transform.position + Constants.SELF_RAYCAST_ORIGIN, transform.TransformDirection(-1f * Vector3.forward), out hit, 1f);
        return !backTileNotWalkable;
    }

    private void MoveForward()
    {
        if (IsFrontTileWalkable())
        {
            DontAllowMovement();
            transform.DOMove(transform.position + transform.forward, Constants.PLAYER_MOVE_TIME)
                .SetEase(ease)
                .OnComplete(AllowMovement);
        }
    }
    private void MoveBackward()
    {
        if (IsBackTileWalkable())
        {
            DontAllowMovement();
            transform.DOMove(transform.position - transform.forward, Constants.PLAYER_MOVE_TIME)
                .SetEase(ease)
                .OnComplete(AllowMovement);
        }
    }

    private void RotateLeft()
    {
        DontAllowMovement();
        transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - rotateAngle, transform.eulerAngles.z), Constants.PLAYER_ROTATE_TIME)
            .SetEase(ease)
            .OnComplete(AllowMovement);
    }
    private void RotateRight()
    {
        DontAllowMovement();
        transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + rotateAngle, transform.eulerAngles.z), Constants.PLAYER_ROTATE_TIME)
            .SetEase(ease)
            .OnComplete(AllowMovement);
    }

    private void Hit()
    {
        DontAllowMovement();
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DORotate(new Vector3(transform.eulerAngles.x + pushAngle, transform.eulerAngles.y, transform.eulerAngles.z), Constants.PLAYER_PUSH_TIME/2));
        mySequence.Append(transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z), Constants.PLAYER_PUSH_TIME/2));

        RaycastHit hit;
        bool hitMade = false;
        if (Physics.Raycast(transform.position + Constants.SELF_RAYCAST_ORIGIN, transform.TransformDirection(Vector3.forward), out hit, 1f))
        {
            if (hit.transform != null)
            {
                StoneHitManager hittable = hit.transform.GetComponent<StoneHitManager>();
                if (hittable != null)
                {
                    hittable.Hit(transform);
                    hitMade = true;
                }
            }
        }
        if (!hitMade)
        {
            AllowMovement();
        }
    }
}