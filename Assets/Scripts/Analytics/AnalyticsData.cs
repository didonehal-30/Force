using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class AnalyticsData
{
    public long sessionID;
    public string levelName;
    public List<string> deathAgent = new List<string>();

    public int manualResetCount = 0;
    public int gameOverResetCount = 0;
    public int gameFinishResetCount = 0;

    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    public List<PUCoordinates> pUCoords = new List<PUCoordinates>();

    public void AddDeath(string name)
    {
        deathAgent.Add(name);
    }

    public void AddCheckpointTime(string name, float time)
    {
        checkpoints.Add(new Checkpoint(name, time));
    }

    public void AddCoords(string _a, float _x, float _y)
    {
        pUCoords.Add(new PUCoordinates(_a, _x, _y));
    }

    public AnalyticsData(long _sessionID, string _levelName)
    {
        this.sessionID = _sessionID;
        this.levelName = _levelName;
    }

}
