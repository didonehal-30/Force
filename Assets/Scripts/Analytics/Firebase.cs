using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Firebase : MonoBehaviour
{
    private string url = "https://force-analytics-default-rtdb.firebaseio.com/";
    private AnalyticsData data; //class to store all the data to be sent on game over/finish

    void Start()
    {
        data = new AnalyticsData(DateTime.Now.Ticks, SceneManager.GetActiveScene().name);
    }

    //method to add the transform where the player has died
    public void AddDeath(string name)
    {
        data.AddDeath(name);
        Debug.Log(data.deathAgent.Count);
    }

    //method to add time taken between each checkpoint
    public void AddCheckpoint(string _n, float _t)
    {
        data.AddCheckpointTime(_n, _t);
    }

    //methods to store different types of reset counts
    public void IncreaseManualReset()
    {
        data.manualResetCount += 1;
    }

    public void IncreaseGameOverReset()
    {
        data.gameOverResetCount += 1;
    }

    public void IncreaseGameFinishReset()
    {
        data.gameFinishResetCount += 1;
    }

    public void AddCoordinates(string _a, float _x, float _y)
    {
        data.AddCoords(_a, _x, _y);
    }

    //more methods to add the analytics data (all the data is stored in AnalyticsData object)

    //method to send the data to firebase - only sent when gameEndUI buttons are pressed
    public void SendData()
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        StartCoroutine(Post(json));
    }

    private IEnumerator Post(string json)
    {

        using (var uwr = new UnityWebRequest(url + ".json", "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            using UploadHandlerRaw uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.uploadHandler = uploadHandler;
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.disposeUploadHandlerOnDispose = true;
            uwr.disposeDownloadHandlerOnDispose = true;
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.timeout = 5;

            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Debug.Log("Firebase upload complete!");
            }
        }
    }

}
