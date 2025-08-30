using UnityEngine;
using System.Collections;

public class EchoSensitiveEnemy : MonoBehaviour
{
    [Header("Echo Sense Reactions")]
    [SerializeField] private bool reactsToEchoSense = true;
    [SerializeField] private float alertedDuration = 3f; // How long enemy is alerted after echo
    [SerializeField] private float alertSpeedMultiplier = 1.5f; // How much faster it moves when alerted
    [SerializeField] private float vulnerableDuration = 1f; // How long weak point is revealed

    [Header("Kinetic Impulse Reactions (Reactive Armor)")]
    [SerializeField] private bool hasReactiveArmor = false;
    [SerializeField] private float reactiveArmorInvulnDuration = 1f; // How long invulnerable after impulse hit
    [SerializeField] private GameObject reactiveArmorEffectPrefab; // Visual effect for reactive armor
    private bool isInvulnerableFromImpulse = false;

    private Vector2 originalSpeed; // Store original speed if enemy has a movement script
    private bool isAlerted = false;
    private SpriteRenderer spriteRenderer; // For visual feedback

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("EchoSensitiveEnemy needs a SpriteRenderer for visual feedback.", this);
        }

        // If your enemy has its own movement script, get a reference to it
        // Example: EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        // if (enemyMovement != null) originalSpeed = enemyMovement.speed;
    }

    // --- Echo Sense Detection ---
    public void OnEchoSenseDetected(Vector3 playerPosition)
    {
        if (!reactsToEchoSense) return;

        if (!isAlerted)
        {
            StartCoroutine(AlertedState(playerPosition));
        }
        else
        {
            // Reset alert timer if already alerted and new echo detected
            StopCoroutine("AlertedState");
            StartCoroutine(AlertedState(playerPosition));
        }
    }

    IEnumerator AlertedState(Vector3 playerPosition)
    {
        isAlerted = true;
        Debug.Log($"{gameObject.name} detected Echo Sense! Alerted and moving towards player.");
        // Visual feedback: e.g., change enemy color, add exclamation mark above head
        if (spriteRenderer != null) spriteRenderer.color = Color.red;

        // Example: If enemy has a movement script, increase its speed
        // EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        // if (enemyMovement != null) enemyMovement.speed *= alertSpeedMultiplier;

        // Example: Make enemy move towards player's last known position
        // This would require a simple AI movement script on the enemy
        // For now, it just knows the position.

        // Simulate revealing a weak point (for boss/specific enemies)
        StartCoroutine(RevealWeakPoint());

        yield return new WaitForSeconds(alertedDuration);

        isAlerted = false;
        Debug.Log($"{gameObject.name} is no longer alerted.");
        if (spriteRenderer != null) spriteRenderer.color = Color.white;
        // Reset enemy speed
        // if (enemyMovement != null) enemyMovement.speed = originalSpeed;
    }

    IEnumerator RevealWeakPoint()
    {
        // This is for enemies that become vulnerable after Echo Sense
        // You'd typically have a separate component for enemy health/damage
        // For demonstration, we'll just log it.
        Debug.Log($"{gameObject.name}'s weak point revealed! Vulnerable for {vulnerableDuration} seconds.");
        // Visual feedback: e.g., glowing weak spot on enemy model
        yield return new WaitForSeconds(vulnerableDuration);
        Debug.Log($"{gameObject.name}'s weak point is no longer revealed.");
    }

    // --- Kinetic Impulse Reaction ---
    public void OnKineticImpulseHit()
    {
        if (!hasReactiveArmor) return;

        if (!isInvulnerableFromImpulse)
        {
            StartCoroutine(ActivateReactiveArmor());
            Debug.Log($"{gameObject.name} activated Reactive Armor!");
            // Trigger reactive armor visual/audio feedback here (e.g., shield glow)
            if (reactiveArmorEffectPrefab != null)
            {
                GameObject effect = Instantiate(reactiveArmorEffectPrefab, transform.position, Quaternion.identity);
                effect.transform.parent = transform; // Attach to enemy
                Destroy(effect, reactiveArmorInvulnDuration);
            }
        }
        else
        {
            Debug.Log($"{gameObject.name} is already invulnerable from Reactive Armor.");
        }
    }

    IEnumerator ActivateReactiveArmor()
    {
        isInvulnerableFromImpulse = true;
        // Optionally, make enemy invulnerable to ALL damage during this time
        // (You'd need to integrate this with your enemy's health/damage system)
        yield return new WaitForSeconds(reactiveArmorInvulnDuration);
        isInvulnerableFromImpulse = false;
        Debug.Log($"{gameObject.name}'s Reactive Armor deactivated.");
    }

    // You would add your enemy's normal movement, attack, and health logic here.
    // Make sure your enemy's damage-taking logic checks `isInvulnerableFromImpulse`.
}
