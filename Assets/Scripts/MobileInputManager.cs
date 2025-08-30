using UnityEngine;
using UnityEngine.EventSystems; // Required for IPointerDownHandler, IPointerUpHandler

public class MobileInputManager : MonoBehaviour
{
    // Public properties to track button states
    public bool moveLeftPressed { get; private set; }
    public bool moveRightPressed { get; private set; }

    // Methods to be called by UI Event Triggers
    public void OnLeftButtonDown()
    {
        moveLeftPressed = true;
    }

    public void OnLeftButtonUp()
    {
        moveLeftPressed = false;
    }

    public void OnRightButtonDown()
    {
        moveRightPressed = true;
    }

    public void OnRightButtonUp()
    {
        moveRightPressed = false;
    }
}