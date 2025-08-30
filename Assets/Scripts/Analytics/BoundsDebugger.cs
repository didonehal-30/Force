using UnityEngine;

public class BoundsDebugger : MonoBehaviour
{
    [SerializeField] Transform topLeft;
    [SerializeField] Transform bottomRight;

    void Start()
    {
        Debug.Log("Min X: " + bottomRight.position.x + ",Max X: " + topLeft.position.x + ",Min Y: " + bottomRight.position.y + ",Max Y: " + topLeft.position.y);
    }
}
