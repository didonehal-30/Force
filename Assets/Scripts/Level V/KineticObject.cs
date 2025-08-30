using UnityEngine;

public class KineticObject : MonoBehaviour
{
    [SerializeField] private float minImpulseForce = 5f; // Minimum force to move this object
    [SerializeField] private bool isReactiveArmor = false; // Is this object an enemy with reactive armor?
    [SerializeField] private float reactiveArmorDuration = 1f; // How long invulnerable after hit
    private bool isInvulnerable = false; // For reactive armor

    private Rigidbody2D rb;

    [Header("Respawnable Crate Settings")]
    [SerializeField] private bool disableOnGround = false;
    [SerializeField] private bool hasLifeTime = false;
    [SerializeField] private float maxLifeTime;
    private float lifeTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("KineticObject requires a Rigidbody2D component!", this);
            enabled = false; // Disable script if no Rigidbody2D
        }
        // Ensure the Rigidbody2D is set to Dynamic and has a Collider2D
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1; // Or whatever gravity you want for it
    }

    public void ApplyImpulse(Vector2 force, bool isPush)
    {
        if (isReactiveArmor && isInvulnerable)
        {
            Debug.Log("KineticObject with Reactive Armor ignored impulse (currently invulnerable).");
            return; // Don't apply force if invulnerable
        }

        if (force.magnitude >= minImpulseForce)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
            Debug.Log($"KineticObject received impulse: {force}");

            if (isReactiveArmor)
            {
                StartCoroutine(ActivateReactiveArmor());
                // Trigger reactive armor visual/audio feedback here (e.g., shield glow)
                Debug.Log("Reactive Armor activated!");
            }
        }
        else
        {
            Debug.Log("Impulse too weak to move KineticObject.");
        }
    }

    System.Collections.IEnumerator ActivateReactiveArmor()
    {
        isInvulnerable = true;
        // Optionally, trigger a visual effect for invulnerability (e.g., a shield sprite, color change)
        yield return new WaitForSeconds(reactiveArmorDuration);
        isInvulnerable = false;
        // Optionally, remove the visual effect
    }

    // You can add methods here for other interactions, e.g.,
    // public bool IsInvulnerable() { return isInvulnerable; }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (disableOnGround && collision.gameObject.CompareTag("Ground"))
            gameObject.SetActive(false);
        if (collision.gameObject.CompareTag("Button"))
        {
            hasLifeTime = false;
        }
    }

    void Update()
    {
        //setting lifeTime for respawnable crates
        if (hasLifeTime)
        {
            lifeTime += Time.deltaTime;
            if (lifeTime > maxLifeTime)
                gameObject.SetActive(false);
        }
    }

    //method to enable respawnable Crates
    public void enableRespawnCrate()
    {
        gameObject.SetActive(true);
        lifeTime = 0;
    }
}
