using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Spike : MonoBehaviour
{
    //private Analytics analytics;
    private Firebase firebase;

    [SerializeField] private GameObject enemyShatterPrefab; 
    [SerializeField] private float enemyShatterDuration = 1.5f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //analytics = FindAnyObjectByType<Analytics>();
        firebase = FindAnyObjectByType<Firebase>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn pr = other.GetComponent<PlayerRespawn>();
            if (pr != null)
            {
                firebase.AddDeath(gameObject.name);

                //analytics.SetAnalytic("LevelName", SceneManager.GetActiveScene().name);
                //analytics.SetAnalytic("KillerName", gameObject.name);
                //analytics.Send();
                pr.DieAndRespawn();
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            StartCoroutine(EnemyShatterRoutine(other.gameObject)); // spikes kill the enemies with shatter
        }
    }

    //Replicate OnTriggerEnter2D for spikes that don't have isTrigger Enabled
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerRespawn pr = collision.gameObject.GetComponent<PlayerRespawn>();
            if (pr != null)
            {
                firebase.AddDeath(gameObject.name);

                //analytics.SetAnalytic("LevelName", SceneManager.GetActiveScene().name);
                //analytics.SetAnalytic("KillerName", gameObject.name);
                //analytics.Send();
                pr.DieAndRespawn();
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(EnemyShatterRoutine(collision.gameObject)); // spikes kill the enemies with shatter
        }
    }

    private IEnumerator EnemyShatterRoutine(GameObject enemy)
    {
        GameObject shatterInstance = null;

        // Spawn shatter effect
        if (enemyShatterPrefab != null)
        {
            shatterInstance = Instantiate(enemyShatterPrefab, enemy.transform.position, enemy.transform.rotation);
        }

        // Hide enemy immediately
        enemy.SetActive(false);

        // Wait before cleaning up shatter pieces
        yield return new WaitForSeconds(enemyShatterDuration);

        // Remove shatter effect
        if (shatterInstance != null)
        {
            Destroy(shatterInstance);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
