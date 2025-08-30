using UnityEngine;

public class LrTesting : MonoBehaviour
{
    [SerializeField] Transform[] points;
    void Start()
    {
        gameObject.GetComponent<LineController>().SetUpLine(points);
    }
}
