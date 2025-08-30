using System;
using System.Collections;
using UnityEngine;

public class SpikeDropper : MonoBehaviour
{
    public float fallSpeed = 5f;
    public float dropDistance = 5f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    private Vector3 topPosition;
    private Vector3 bottomPosition;
    private bool isFalling = false;

    void Start()
    {
        topPosition = transform.position;
        bottomPosition = new Vector3(topPosition.x, topPosition.y - dropDistance, topPosition.z);
        StartCoroutine(SpikeRoutine());
    }

    IEnumerator SpikeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minWaitTime, maxWaitTime));

            isFalling = true;
            yield return new WaitUntil(() => !isFalling);
            transform.position = topPosition; 
        }
    }

    void Update()
    {
        if (isFalling)
        {
            transform.position = Vector3.MoveTowards(transform.position, bottomPosition, fallSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, bottomPosition) < 0.01f)
            {
                isFalling = false;
            }
        }
    }
}
