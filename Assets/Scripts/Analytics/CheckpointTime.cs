using UnityEngine;

public class CheckpointTime : MonoBehaviour
{
    private float timeElapsed; //used to track time elapsed since last checkpoint
    void Start()
    {
        timeElapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
    }

    public float GetTime()
    {
        return timeElapsed;
    }

    public void ResetTime()
    {
        timeElapsed = 0;
    }
}
