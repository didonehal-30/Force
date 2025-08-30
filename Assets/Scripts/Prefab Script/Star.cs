using System.Diagnostics;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public float floatAmplitude = 0.3f;
    public float floatSpeed = 3f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = startPos + new Vector3(0, newY, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Star Collected");
            FindObjectOfType<GameManager>().CollectStar();
            Destroy(gameObject);
        }
    }
}
