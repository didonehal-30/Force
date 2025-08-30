using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Checkpoint] Triggered by: {other.name}");

        if (!activated && other.CompareTag("Player"))
        {
            SnapshotManager.Instance.SaveCheckpoint();
            Debug.Log("[Checkpoint] Checkpoint Saved!");
            activated = true;
        }
    }
}
