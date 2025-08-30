using UnityEngine;
using System.Collections; // Still good practice for coroutines, though less critical now

public class HiddenPlatform : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    private bool isRevealed = false; // Track if the platform has been revealed

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();

        if (spriteRenderer == null || platformCollider == null)
        {
            Debug.LogError("HiddenPlatform requires a SpriteRenderer and Collider2D!", this);
            enabled = false;
            return;
        }

        // Initially hide the platform
        spriteRenderer.enabled = false;
        platformCollider.enabled = true; // Disable collider so player falls through
    }

    // This method is called by Echo Sense to reveal the platform
    public void Reveal(float duration) // Duration parameter is now ignored, but kept for compatibility
    {
        // Only reveal if not already revealed to prevent redundant calls
        if (!isRevealed)
        {
            spriteRenderer.enabled = true;
            platformCollider.enabled = true; // Enable collider so player can stand on it
            isRevealed = true; // Mark as revealed

            // Optional: Add a subtle fade-in effect here if desired
            // For example: StartCoroutine(FadeInEffect());
            Debug.Log($"{gameObject.name} has been revealed and will stay visible.");
        }
    }

    // If you had a fade-in effect, you might implement it like this:
    /*
    IEnumerator FadeInEffect()
    {
        Color startColor = spriteRenderer.color;
        startColor.a = 0f; // Start fully transparent
        spriteRenderer.color = startColor;

        float timer = 0f;
        float fadeDuration = 0.5f; // Adjust as needed

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            Color currentColor = spriteRenderer.color;
            currentColor.a = alpha;
            spriteRenderer.color = currentColor;
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f); // Ensure fully opaque
    }
    */
}
