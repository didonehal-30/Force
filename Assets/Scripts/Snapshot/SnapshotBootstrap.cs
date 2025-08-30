using UnityEngine;

public class SnapshotBootstrap : MonoBehaviour
{
    void Start()
    {
        foreach (var box in GameObject.FindGameObjectsWithTag("Ground"))
        {
            if (box.name.Contains("KineticBox") || box.GetComponent<Rigidbody>() != null)
            {
                SnapshotManager.Instance.RegisterObject(box, "SnapshotTarget");
                Debug.Log($"[Bootstrap] Registered Box: {box.name}");
            }
        }

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            SnapshotManager.Instance.RegisterObject(enemy, "SnapshotTarget");
            Debug.Log($"[Bootstrap] Registered Enemy: {enemy.name}");
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SnapshotManager.Instance.RegisterObject(player, "Player");
            Debug.Log($"[Bootstrap] Registered Player: {player.name}");
        }
    }
}
