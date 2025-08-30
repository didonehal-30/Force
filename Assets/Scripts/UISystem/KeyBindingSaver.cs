using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KeyBindingSaver : MonoBehaviour
{
    private string savePath => Path.Combine(Application.persistentDataPath, "keybindings.json");

    [System.Serializable]
    public class BindingEntry
    {
        public string action;
        public KeyCode key;
    }

    [System.Serializable]
    public class BindingListWrapper
    {
        public List<BindingEntry> bindings = new();
    }

    public void Save(Dictionary<ActionKey, KeyCode> keyMap)
    {
        BindingListWrapper wrapper = new();

        foreach (var pair in keyMap)
        {
            wrapper.bindings.Add(new BindingEntry
            {
                action = pair.Key.ToString(),
                key = pair.Value
            });
        }

        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"[KeyBindingSaver] Saved to: {savePath}");
    }

    public Dictionary<ActionKey, KeyCode> Load(Dictionary<ActionKey, KeyCode> defaultMap)
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("[KeyBindingSaver] No save file found. Using default config.");
            return new Dictionary<ActionKey, KeyCode>(defaultMap);
        }

        string json = File.ReadAllText(savePath);
        BindingListWrapper wrapper = JsonUtility.FromJson<BindingListWrapper>(json);

        Dictionary<ActionKey, KeyCode> loadedMap = new(defaultMap); // start with default

        foreach (var entry in wrapper.bindings)
        {
            if (System.Enum.TryParse(entry.action, out ActionKey action))
            {
                loadedMap[action] = entry.key;
            }
        }

        Debug.Log("[KeyBindingSaver] Loaded from saved file.");
        return loadedMap;
    }
}
