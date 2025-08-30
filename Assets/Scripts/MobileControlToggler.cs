using UnityEngine;

public class MobileControlToggler : MonoBehaviour
{
    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("MobileControlToggler: No Canvas component found on this GameObject.", this);
            return;
        }

        Debug.Log("MobileControlToggler Awake: GameSettings.UseMobileControls is currently: " + GameSettings.UseMobileControls);

        if (GameSettings.UseMobileControls)
        {
            canvas.enabled = true;
            Debug.Log("MobileControlToggler: Enabling Canvas (Mobile Controls).");
        }
        else
        {
            canvas.enabled = false;
            Debug.Log("MobileControlToggler: Disabling Canvas (Mobile Controls).");
        }
    }
}