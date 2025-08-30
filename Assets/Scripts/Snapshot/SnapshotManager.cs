using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectState
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;

    public ObjectState(Vector3 pos, Quaternion rot, Vector3 vel)
    {
        position = pos;
        rotation = rot;
        velocity = vel;
    }
}

public class SnapshotManager : MonoBehaviour
{
    public static SnapshotManager Instance;

    private Dictionary<string, GameObject> trackedObjects = new();
    private Dictionary<string, string> objectToCategory = new();
    private Dictionary<string, ObjectState> checkpointState = new();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterObject(GameObject obj, string category)
    {
        string id = obj.GetInstanceID().ToString();
        if (!trackedObjects.ContainsKey(id))
        {
            trackedObjects[id] = obj;
            objectToCategory[id] = category;
            Debug.Log($"[SnapshotManager] Registered: {obj.name}");
        }
    }

    public void SaveCheckpoint()
    {
        checkpointState.Clear();

        foreach (var (id, obj) in trackedObjects)
        {
            if (TryCaptureState(obj, out var state))
            {
                checkpointState[id] = state;
            }
        }

        Debug.Log("[SnapshotManager] Full checkpoint saved.");
    }

    public void RestoreCheckpoint()
    {
        foreach (var (id, obj) in trackedObjects)
        {
            if (checkpointState.ContainsKey(id))
            {
                TryRestoreState(obj, checkpointState[id]);
                Debug.Log($"[SnapshotManager] Restored: {obj.name}");
            }
        }
    }

    private bool TryCaptureState(GameObject obj, out ObjectState state)
    {
        var rb2D = obj.GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            state = new ObjectState(obj.transform.position, obj.transform.rotation, rb2D.linearVelocity);
            return true;
        }

        // 可选：保留对 Rigidbody 的兼容支持
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            state = new ObjectState(obj.transform.position, obj.transform.rotation, rb.linearVelocity);
            return true;
        }

        state = null;
        return false;
    }

    private bool TryRestoreState(GameObject obj, ObjectState state)
    {
        var rb2D = obj.GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            rb2D.linearVelocity = state.velocity;
            obj.transform.position = state.position;
            obj.transform.rotation = state.rotation;
            return true;
        }

        // 可选：保留对 Rigidbody 的兼容支持
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = state.velocity;
            obj.transform.position = state.position;
            obj.transform.rotation = state.rotation;
            return true;
        }

        return false;
    }
}
