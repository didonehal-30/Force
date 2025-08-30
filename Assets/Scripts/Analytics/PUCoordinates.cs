using UnityEngine;

[System.Serializable]
public class PUCoordinates
{
    public string action;
    public float x;
    public float y;

    public PUCoordinates(string _a, float _x, float _y)
    {
        this.action = _a;
        this.x = _x;
        this.y = _y;
    }
}
