using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] int checkpointID;
    [SerializeField] Color activatedColor = Color.green;
    [SerializeField] Color defaultColor = Color.yellow;
    [SerializeField] GameObject sparklePrefab;

    private SpriteRenderer sr;
    private bool snapshotSaved = false;
    private CheckpointTime checkpointTime;
    private Firebase firebase;
    private bool hasNotPassedCheckpoint = true;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr) sr.color = defaultColor;
        checkpointTime = FindAnyObjectByType<CheckpointTime>();
        firebase = FindAnyObjectByType<Firebase>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerRespawn pr = other.GetComponent<PlayerRespawn>();
        if (pr != null) pr.SetRespawnPoint(this.transform);

        foreach (var point in FindObjectsByType<RespawnPoint>(FindObjectsSortMode.None))
        {
            point.ResetColor();
        }

        if (sr) sr.color = activatedColor;

        if (hasNotPassedCheckpoint)
        {
            firebase.AddCheckpoint("Checkpoint " + checkpointID, checkpointTime.GetTime());
            checkpointTime.ResetTime();

            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null && ph.GetCurrentHealth() < ph.GetMaxHealth())
            {
                float newHealth = Mathf.Min(ph.GetCurrentHealth() + 30f, ph.GetMaxHealth());
                typeof(PlayerHealth)
                    .GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .SetValue(ph, newHealth);

                UnityEngine.UI.Image healthImage = typeof(PlayerHealth)
                    .GetField("health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .GetValue(ph) as UnityEngine.UI.Image;
                if (healthImage != null) healthImage.fillAmount = newHealth / ph.GetMaxHealth();

               
            }
            if (sparklePrefab != null)
            {
                Instantiate(sparklePrefab, other.transform.position, Quaternion.identity);
            }

            hasNotPassedCheckpoint = false;
        }

        if (!snapshotSaved && SnapshotManager.Instance != null)
        {
            SnapshotManager.Instance.SaveCheckpoint();
            snapshotSaved = true;
        }
    }

    public void ResetColor()
    {
        if (sr) sr.color = defaultColor;
    }
}
