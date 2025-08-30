using System.Collections.Generic;
using UnityEngine;

public class HoldButtonDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    private HashSet<GameObject> currentColliders = new HashSet<GameObject>();

    [SerializeField] float moveDistance = 10f;
    [SerializeField] float openSpeed = 2f; 
    [SerializeField] float closeSpeed = 8f; 

    private Coroutine moveCoroutine;
    private bool doorIsOpen = false;
    private Vector3 doorClosedPosition;
    private Vector3 doorOpenPosition;

    void Start()
    {
        doorClosedPosition = door.transform.position;
        doorOpenPosition = doorClosedPosition + door.transform.up * moveDistance;
    }

    void Update()
    {
        if (!isNotColliding())
        {
            if (!doorIsOpen)
            {
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(MoveDoor(doorOpenPosition, openSpeed));
                doorIsOpen = true;
            }
        }
        else
        {
            if (doorIsOpen)
            {
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                moveCoroutine = StartCoroutine(MoveDoor(doorClosedPosition, closeSpeed));
                doorIsOpen = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentColliders.Add(collision.gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        currentColliders.Remove(collision.gameObject);
    }

    private bool isNotColliding()
    {
        return currentColliders.Count == 0;
    }

    private System.Collections.IEnumerator MoveDoor(Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(door.transform.position, targetPosition) > 0.01f)
        {
            door.transform.position = Vector3.MoveTowards(
                door.transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
            yield return null;
        }

        door.transform.position = targetPosition;
    }
}
