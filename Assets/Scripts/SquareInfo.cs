using Unity.VisualScripting;
using UnityEngine;

public class SquareInfo : MonoBehaviour
{
    [SerializeField] bool isWalkable;
    public bool IsWalkable {
        get { return isWalkable; }
        set { isWalkable = value; }
    }
    
    [SerializeField] bool isSlideable;
    public bool IsSlideable {
        get { return isSlideable; }
        set { isSlideable = value; }
    }

    [SerializeField] bool isCollectible;
    public bool IsCollectible {
        get { return isCollectible; }
        set { isCollectible = value; }
    }
}
