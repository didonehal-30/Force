using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyLoggerUI : MonoBehaviour
{
    public GameObject keyLogEntryPrefab;
    public Transform contentParent;
    public int maxEntries = 20;

    private Queue<GameObject> entryQueue = new();

    void Update()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                AddKeyEntry(key.ToString());
            }
        }
    }

    void AddKeyEntry(string keyName)
    {
        GameObject entry = Instantiate(keyLogEntryPrefab, contentParent);
        entry.GetComponent<TMP_Text>().text = keyName;

        entryQueue.Enqueue(entry);

        if (entryQueue.Count > maxEntries)
        {
            Destroy(entryQueue.Dequeue());
        }
    }
}