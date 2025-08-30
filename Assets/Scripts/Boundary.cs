using UnityEngine;

public class Boundary : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.GameOver();
        }
    }
}
