using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Analytics : MonoBehaviour
{
    [SerializeField] string formUrl;
    private long _sessionID;
    private string _levelName;
    private string _killerName;
    void Start()
    {
        _sessionID = DateTime.Now.Ticks;
    }

    public void Send()
    {
        //convert all metrics collected to strings
        StartCoroutine(Post(_sessionID.ToString(), _levelName, _killerName));
    }

    private IEnumerator Post(string sessionID, string levelName, string killerName)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.697622678", sessionID);
        form.AddField("entry.588505381", levelName);
        form.AddField("entry.723241446", killerName);

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    public void SetAnalytic(string analyticName, string analyticValue)
    {
        switch (analyticName)
        {
            case "KillerName": _killerName = analyticValue; break;
            case "LevelName": _levelName = analyticValue; break;
        }
    }
}
