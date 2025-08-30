// PlayerPowerUps.cs
using UnityEngine;
using System.Collections;

public class PlayerPowerUps : MonoBehaviour
{
    // --- References ---
    private PlayerMovement playerMovement; // Reference to your existing PlayerMovement script
    private Rigidbody2D rb; // Player's Rigidbody2D

    // --- Kinetic Impulse Settings ---
    [Header("Kinetic Impulse Settings")]
    [SerializeField] private float impulseForce = 15f; // How strong the push/pull is
    [SerializeField] private float impulseRange = 3f; // Max distance to affect objects
    [SerializeField] private float impulseCooldown = 0.5f; // Cooldown for the ability
    [SerializeField] private float playerRecoilForce = 5f; // Force applied to player as recoil
    private bool canUseImpulse = true;

    // --- Echo Sense Settings ---
    [Header("Echo Sense Settings")]
    [SerializeField] private float echoSenseRadius = 10f; // Radius of the echo pulse
    [SerializeField] private float echoRevealDuration = 2f; // How long hidden objects are revealed
    [SerializeField] private float echoCooldown = 1f; // Cooldown for Echo Sense
    [SerializeField] private float sensoryOverloadDuration = 0.5f; // How long player is disoriented
    [SerializeField] private float sensoryOverloadSpeedPenalty = 0.5f; // Multiplier for player speed during overload
    private bool canUseEcho = true;
    private bool isSensoryOverloaded = false;

    // --- Visual/Audio Feedback Prefabs (Assign in Inspector) ---
    [Header("Feedback Prefabs")]
    [SerializeField] private GameObject impulseEffectPrefab; // Visual effect for impulse (e.g., a burst, wave)
    [SerializeField] private GameObject echoEffectPrefab; // Visual effect for echo (e.g., expanding circle)
    [SerializeField] private AudioClip impulseSound; // Sound for impulse
    [SerializeField] private AudioClip echoSound; // Sound for echo
    private AudioSource audioSource;

    // --- Thicker Raycast using BoxCast ---
    Vector2 boxSize = new Vector2(1.0f, 1.0f);

    private Firebase firebase;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>(); // Get reference to existing movement script
        audioSource = GetComponent<AudioSource>(); // Add an AudioSource component to your player if you don't have one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //get firebase analytics object
        firebase = FindAnyObjectByType<Firebase>();
    }

    void Update()
    {
        // --- Keyboard Input for Push/Pull/Echo Sense (Active in Editor/Standalone Builds) ---
        if (canUseImpulse)
        {
            if (Input.GetKeyDown(KeyCode.E)) // 'E' for Push
            {
                firebase.AddCoordinates("push", gameObject.transform.position.x, gameObject.transform.position.y);
                UseKineticImpulse(true); // True for Push
            }
            else if (Input.GetKeyDown(KeyCode.Q)) // 'Q' for Pull
            {
                firebase.AddCoordinates("pull", gameObject.transform.position.x, gameObject.transform.position.y);
                UseKineticImpulse(false); // False for Pull
            }
        }

        if (canUseEcho && !isSensoryOverloaded)
        {
            if (Input.GetKeyDown(KeyCode.X)) // 'X' for Echo Sense
            {
                UseEchoSense();
            }
        }

        // Apply sensory overload penalty if active
        if (isSensoryOverloaded && playerMovement != null)
        {
            // Assuming PlayerMovement has a public method or property for speed
            // You might need to modify PlayerMovement to allow this.
            // For now, we'll just slow down horizontal movement directly if possible.
            // A better way would be to have a public method in PlayerMovement to apply/remove debuffs.
            // Example: playerMovement.ApplySpeedPenalty(sensoryOverloadSpeedPenalty);
        }
    }

    // --- Kinetic Impulse Logic ---
    void UseKineticImpulse(bool isPush)
    {
        canUseImpulse = false;
        StartCoroutine(KineticImpulseCooldown());

        // Play visual and audio effects
        if (impulseEffectPrefab != null)
        {
            GameObject effect = Instantiate(impulseEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }
        if (impulseSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(impulseSound);
        }

        Vector2 playerFacingDirection = (Vector2)transform.right * Mathf.Sign(transform.localScale.x);

        rb.AddForce(-playerFacingDirection * playerRecoilForce, ForceMode2D.Impulse);

        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            boxSize,
            0f, // No rotation
            playerFacingDirection,
            impulseRange,
            LayerMask.GetMask("KineticObjects")
        );

        if (hit.collider != null)
        {
            KineticObject kineticObject = hit.collider.GetComponent<KineticObject>();
            if (kineticObject != null)
            {
                Vector2 forceToApply = isPush ? playerFacingDirection * impulseForce : -playerFacingDirection * impulseForce;
                kineticObject.ApplyImpulse(forceToApply, isPush);
                Debug.Log($"Applied {(isPush ? "push" : "pull")} force of {forceToApply} to {hit.collider.name}");
            }

            EchoSensitiveEnemy enemy = hit.collider.GetComponent<EchoSensitiveEnemy>();
            if (enemy != null)
            {
                enemy.OnKineticImpulseHit();
                Debug.Log($"Hit enemy {hit.collider.name} with impulse.");
            }
        }
        else
        {
            Debug.Log("No KineticObject or EchoSensitiveEnemy found within impulse range.");
        }
    }

    IEnumerator KineticImpulseCooldown()
    {
        yield return new WaitForSeconds(impulseCooldown);
        canUseImpulse = true;
    }

    // --- Echo Sense Logic ---
    void UseEchoSense()
    {
        canUseEcho = false;
        StartCoroutine(EchoSenseCooldown());
        StartCoroutine(SensoryOverload()); // Trigger sensory overload immediately

        // Play visual and audio effects
        if (echoEffectPrefab != null)
        {
            GameObject effect = Instantiate(echoEffectPrefab, transform.position, Quaternion.identity);
            // The echo effect might need to scale up over time
            Destroy(effect, echoRevealDuration + 0.1f);
        }
        if (echoSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(echoSound);
        }

        // Find all colliders within the Echo Sense radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, echoSenseRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            // Reveal hidden platforms
            HiddenPlatform hiddenPlatform = hitCollider.GetComponent<HiddenPlatform>();
            if (hiddenPlatform != null)
            {
                hiddenPlatform.Reveal(echoRevealDuration);
            }

            // Alert EchoSensitiveEnemies
            EchoSensitiveEnemy enemy = hitCollider.GetComponent<EchoSensitiveEnemy>();
            if (enemy != null)
            {
                enemy.OnEchoSenseDetected(transform.position); // Pass player position for enemy to react
            }
        }
    }

    IEnumerator EchoSenseCooldown()
    {
        yield return new WaitForSeconds(echoCooldown);
        canUseEcho = true;
    }

    IEnumerator SensoryOverload()
    {
        isSensoryOverloaded = true;
        // Visual feedback for sensory overload (e.g., screen tint, blur)
        // You might need a Post-Processing Stack or a UI Canvas for this.
        // For now, we'll just affect movement if PlayerMovement allows.
        if (playerMovement != null)
        {
            // Temporarily reduce player speed. You'll need to add a public method
            // to PlayerMovement like SetSpeedMultiplier(float multiplier)
            // playerMovement.SetSpeedMultiplier(sensoryOverloadSpeedPenalty);
        }

        yield return new WaitForSeconds(sensoryOverloadDuration);

        isSensoryOverloaded = false;
        if (playerMovement != null)
        {
            // Reset player speed
            // playerMovement.SetSpeedMultiplier(1f);
        }
        // Remove visual feedback
    }

    // Optional: Draw gizmos in editor for easier debugging of ranges
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, echoSenseRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.right * transform.localScale.x * impulseRange, boxSize);
    }

    // --- Public Methods for UI Buttons ---

    public void OnPushButtonPressed()
    {
        if (canUseImpulse)
        {
            UseKineticImpulse(true); // True for Push
        }
    }

    public void OnPullButtonPressed()
    {
        if (canUseImpulse)
        {
            UseKineticImpulse(false); // False for Pull
        }
    }

    public void OnEchoSenseButtonPressed()
    {
        if (canUseEcho && !isSensoryOverloaded)
        {
            UseEchoSense();
        }
    }
}