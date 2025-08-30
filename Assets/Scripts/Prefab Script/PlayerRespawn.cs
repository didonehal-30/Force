using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] Transform currentRespawnPoint;
    [SerializeField] private float damageTaken = 34.0f;
    [SerializeField] GameObject shatterPrefab;
    [SerializeField] float shatterDuration = 1.5f;

    private GameObject currentShatterInstance;
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private bool isRespawning = false; // Prevent multiple shatters

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void DieAndRespawn()
    {
        if (!isRespawning)
            StartCoroutine(DieAndRespawnRoutine());
    }

    private IEnumerator DieAndRespawnRoutine()
    {
        isRespawning = true;

        if (playerMovement != null)
            playerMovement.externalLockMovement = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.simulated = false;

        if (shatterPrefab != null)
        {
            currentShatterInstance = Instantiate(shatterPrefab, transform.position, transform.rotation);
        }

        SetPlayerVisualsActive(false);

        yield return new WaitForSeconds(shatterDuration);

        if (currentRespawnPoint != null)
        {
            playerHealth.TakeDamage(damageTaken);

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            transform.position = currentRespawnPoint.position;
        }

        if (currentShatterInstance != null)
        {
            Destroy(currentShatterInstance);
        }

        SetPlayerVisualsActive(true);

        if (rb != null)
            rb.simulated = true;

        if (playerMovement != null)
            playerMovement.externalLockMovement = false;

        isRespawning = false;
    }

    private void SetPlayerVisualsActive(bool isActive)
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var r in renderers)
        {
            r.enabled = isActive;
        }

        if (playerHealth != null)
        {
            playerHealth.SetHealthUIActive(isActive);
        }
    }

    public void SetRespawnPoint(Transform point)
    {
        currentRespawnPoint = point;
    }



void Start()
    {

    }

    void Update()
    {

    }
}
