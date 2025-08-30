using UnityEngine;

/// <summary>
/// Debug tool to teleport the Player to the Goal position
/// and instantly trigger level completion when the "C" key is pressed.
/// </summary>
public class DebugCheatTool : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;         // Reference to the Player object
    [SerializeField] private Transform goal;           // Reference to the Goal object
    [SerializeField] private GameManager gameManager;  // Reference to the GameManager

    void Update()
    {
        // Press the "C" key to teleport the player and trigger level completion
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (player != null && goal != null)
            {
                // Move player to the goal position, slightly above to avoid collision
                player.position = goal.position + Vector3.up * 1.5f;
                Debug.Log("Cheat activated: Player teleported to goal.");

                // Manually trigger level completion
                if (gameManager != null)
                {
                    gameManager.GameFinish();
                }
                else
                {
                    Debug.LogWarning("GameManager reference is missing.");
                }
            }
            else
            {
                Debug.LogWarning("Player or Goal reference is missing.");
            }
        }
    }
}