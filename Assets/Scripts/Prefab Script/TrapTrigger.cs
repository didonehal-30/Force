using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public MoveTrap trap;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && trap != null)
        {
            trap.ActivateTrap();
        }
    }
}