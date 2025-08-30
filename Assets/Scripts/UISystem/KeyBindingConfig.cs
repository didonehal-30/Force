using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyMapping
{
    public ActionKey action;
    public KeyCode key;
}

[CreateAssetMenu(fileName = "KeyBindingConfig", menuName = "Input/KeyBinding Config")]
public class KeyBindingConfig : ScriptableObject
{
    public List<KeyMapping> keyMappings = new();
}
