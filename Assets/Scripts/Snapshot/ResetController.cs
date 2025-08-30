using UnityEngine;

public class ResetController : MonoBehaviour
{
    public KeyCode resetKey = KeyCode.R;
    private Firebase firebase;

    void Start()
    {
        firebase = FindAnyObjectByType<Firebase>();
    }

    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            Debug.Log("[Reset] R key pressed");
            firebase.IncreaseManualReset(); //method to track manual resets
            SnapshotManager.Instance.RestoreCheckpoint();
        }
    }
}
