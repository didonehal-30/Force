using UnityEngine;
using System.Collections;

public class DisappearPlatform : MonoBehaviour
{
    public float shakeDuration = 3f;    // How long the platform shakes
    public float respawnDelay = 1f;     // How long before it reappears
    public float shakeAmount = 0.1f;    // How much the platform shakes

    private Vector3 originalPos;
    private SpriteRenderer sr;
    private Collider2D col;

    private void Awake()
    {
        originalPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShakeAndDisappear());
        }
    }

    private IEnumerator ShakeAndDisappear()
    {
        // Prevent multiple triggers
        col.enabled = false;
        col.enabled = true;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            // Simple shaking effect
            Vector3 offset = Random.insideUnitCircle * shakeAmount;
            transform.position = originalPos + offset;
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Reset position and make platform disappear
        transform.position = originalPos;
        sr.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // Reappear
        sr.enabled = true;
        col.enabled = true;
    }
}
