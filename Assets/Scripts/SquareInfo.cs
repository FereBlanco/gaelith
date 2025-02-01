using UnityEngine;

public class SquareInfo : MonoBehaviour
{
    [SerializeField] bool isWalkable;
    public bool IsWalkable { get { return isWalkable; } }
    
    [SerializeField] bool isSlideable;
    public bool IsSlideable { get { return isSlideable; } }

    [SerializeField] bool isCollectible;
    public bool IsCollectible { get { return isCollectible; } }

}
