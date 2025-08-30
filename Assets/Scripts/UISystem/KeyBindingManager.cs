using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingManager : MonoBehaviour
{
    public static KeyBindingManager Instance;

    private Dictionary<ActionKey, KeyCode> keyBindings = new();
    private Dictionary<ActionKey, KeyCode> defaultBindings = new();
    private ActionKey? currentBindingAction = null;
    private Action<KeyCode> bindingCallback = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitDefaults();
        }
        else Destroy(gameObject);
    }

    private void InitDefaults()
    {
        defaultBindings[ActionKey.Jump] = KeyCode.W;
        defaultBindings[ActionKey.Left] = KeyCode.A;
        defaultBindings[ActionKey.Right] = KeyCode.D;
        defaultBindings[ActionKey.Pull] = KeyCode.Q;
        defaultBindings[ActionKey.Push] = KeyCode.E;
        defaultBindings[ActionKey.Reset] = KeyCode.R;
        defaultBindings[ActionKey.OpenMenu] = KeyCode.M;

        keyBindings = new Dictionary<ActionKey, KeyCode>(defaultBindings);
    }

    public void StartBinding(ActionKey action, Action<KeyCode> callback)
    {
        currentBindingAction = action;
        bindingCallback = callback;
    }

    void OnGUI()
    {
        if (currentBindingAction.HasValue && Event.current.isKey)
        {
            var newKey = Event.current.keyCode;
            keyBindings[currentBindingAction.Value] = newKey;
            bindingCallback?.Invoke(newKey);
            currentBindingAction = null;
            bindingCallback = null;
        }
    }

    public KeyCode GetKeyForAction(ActionKey action)
    {
        return keyBindings.ContainsKey(action) ? keyBindings[action] : KeyCode.None;
    }

    public void ResetToDefault()
    {
        keyBindings = new Dictionary<ActionKey, KeyCode>(defaultBindings);
    }

    public Dictionary<ActionKey, KeyCode> GetAllBindings()
    {
        return new Dictionary<ActionKey, KeyCode>(keyBindings);
    }
}