using UnityEngine;

[System.Serializable]
public class Checkpoint
{
    public string checkpointName;
    public float time;

    public Checkpoint(string _cn, float _t)
    {
        this.checkpointName = _cn;
        this.time = _t;
    }
}
