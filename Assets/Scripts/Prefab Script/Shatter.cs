using UnityEngine;

public class Shatter : MonoBehaviour
{
    public float explosionForce = 5f;

    void Start()
    {
        foreach (Rigidbody2D rb in GetComponentsInChildren<Rigidbody2D>())
        {
            Vector2 forceDirection = (rb.transform.position - transform.position).normalized;
            rb.AddForce(forceDirection * explosionForce, ForceMode2D.Impulse);
        }
    }
}
