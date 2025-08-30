using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{

    [SerializeField] float movementDistance;
    [SerializeField] float speed;
    private float leftEdge;
    private float rightEdge;
    private bool isMovingLeft;
    private float enemyScale = 2.0f;
    //private Analytics analytics;
    private Firebase firebase;
    // private PlayerAttack playerAttack;       
    void Start()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
        // playerAttack = gameObject.GetComponent<PlayerAttack>();

        //analytics = FindAnyObjectByType<Analytics>(); 
        firebase = FindAnyObjectByType<Firebase>();
    }

    void Update()
    {
        if (isMovingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                isMovingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                isMovingLeft = true;
        }

        //flip enemy
        if (isMovingLeft)
            transform.localScale = new Vector3(-1, 1, 1) * enemyScale;
        else
            transform.localScale = Vector3.one * enemyScale;
    }

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
    }
}