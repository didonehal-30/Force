// MobileUILinker.cs
using UnityEngine;
using UnityEngine.UI;
using System; // For Action
using UnityEngine.EventSystems; // For PointerDown/Up

public class MobileUILinker : MonoBehaviour
{
    [Header("Button References (Assign in Inspector)")]
    public Button jumpButton;
    public Button pushButton;
    public Button pullButton;
    public Button echoButton;

    [Header("Mobile Movement Buttons (Assign in Inspector)")]
    public GameObject leftButtonObj; // Use GameObject to get EventTrigger
    public GameObject rightButtonObj; // Use GameObject to get EventTrigger

    private PlayerMovement playerMovement;
    private PlayerPowerUps playerPowerUps;
    private MobileInputManager mobileInputManager;

    void Start()
    {
        // Find the player in the scene (assuming it's tagged "Player" or has PlayerMovement component)
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerPowerUps = FindObjectOfType<PlayerPowerUps>();
        mobileInputManager = FindObjectOfType<MobileInputManager>();

        if (playerMovement == null)
        {
            Debug.LogError("MobileUILinker: PlayerMovement script not found in scene! Cannot link UI controls.");
            return; // Exit if player is not found
        }
        if (playerPowerUps == null)
        {
            Debug.LogError("MobileUILinker: PlayerPowerUps script not found in scene! Cannot link UI controls.");
            // Continue as movement might still work
        }
        if (mobileInputManager == null)
        {
            Debug.LogError("MobileUILinker: MobileInputManager script not found in scene! Cannot link mobile movement.");
            // Continue as other buttons might still work
        }

        // Link Jump Button
        if (jumpButton != null)
        {
            jumpButton.onClick.AddListener(playerMovement.OnJumpButtonPressed);
        }
        else { Debug.LogWarning("Jump Button not assigned to MobileUILinker!"); }


        // Link Power-Up Buttons
        if (pushButton != null)
        {
            pushButton.onClick.AddListener(playerPowerUps.OnPushButtonPressed);
        }
        else { Debug.LogWarning("Push Button not assigned to MobileUILinker!"); }

        if (pullButton != null)
        {
            pullButton.onClick.AddListener(playerPowerUps.OnPullButtonPressed);
        }
        else { Debug.LogWarning("Pull Button not assigned to MobileUILinker!"); }

        if (echoButton != null)
        {
            echoButton.onClick.AddListener(playerPowerUps.OnEchoSenseButtonPressed);
        }
        else { Debug.LogWarning("Echo Button not assigned to MobileUILinker!"); }

        // Link Movement Buttons (Left/Right) using EventTrigger
        if (mobileInputManager != null)
        {
            LinkMovementButton(leftButtonObj, mobileInputManager.OnLeftButtonDown, mobileInputManager.OnLeftButtonUp);
            LinkMovementButton(rightButtonObj, mobileInputManager.OnRightButtonDown, mobileInputManager.OnRightButtonUp);
        }
    }

    private void LinkMovementButton(GameObject buttonObj, Action pointerDownAction, Action pointerUpAction)
    {
        if (buttonObj == null)
        {
            Debug.LogWarning("Movement button GameObject not assigned to MobileUILinker!");
            return;
        }

        EventTrigger trigger = buttonObj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = buttonObj.AddComponent<EventTrigger>();
        }

        // Clear existing listeners to prevent duplicates
        trigger.triggers.Clear();

        // Add PointerDown event
        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((data) => { pointerDownAction?.Invoke(); });
        trigger.triggers.Add(entryDown);

        // Add PointerUp event
        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((data) => { pointerUpAction?.Invoke(); });
        trigger.triggers.Add(entryUp);
    }

    // Optional: OnDestroy to remove listeners if needed (good practice for dynamic scenes)
    void OnDestroy()
    {
        if (jumpButton != null) jumpButton.onClick.RemoveAllListeners();
        if (pushButton != null) pushButton.onClick.RemoveAllListeners();
        if (pullButton != null) pullButton.onClick.RemoveAllListeners();
        if (echoButton != null) echoButton.onClick.RemoveAllListeners();

        // For EventTrigger, it's safer to clear and re-add or manage through the Trigger component itself.
        // Since LinkMovementButton clears, this might not be strictly necessary here if the Canvas is destroyed/reloaded.
    }
}