using UnityEngine;
using System.Collections.Generic;

public class InputManagerProxy : MonoBehaviour
{
    public static InputManagerProxy Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public bool GetKey(ActionKey action)
    {
        return Input.GetKey(KeyBindingManager.Instance.GetKeyForAction(action));
    }

    public bool GetKeyDown(ActionKey action)
    {
        return Input.GetKeyDown(KeyBindingManager.Instance.GetKeyForAction(action));
    }
}
