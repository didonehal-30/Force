using UnityEngine;
using System.Collections;

public class MoveTrap : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveUpSpeed = 10f;
    public float moveDownSpeed = 2f;
    [HideInInspector]
    public Vector3 originPos;
    private bool isMoving = false;

    void Start()
    {
        originPos = transform.position;
    }

    public void ActivateTrap()
    {
        if (!isMoving)
            StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        isMoving = true;
        Vector3 target = originPos + Vector3.up * moveDistance;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveUpSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        while (Vector3.Distance(transform.position, originPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, moveDownSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = originPos;
        isMoving = false;
    }
}
