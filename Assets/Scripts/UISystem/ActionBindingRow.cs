using UnityEngine;
using TMPro;

public class ActionBindingRow : MonoBehaviour
{
    public TextMeshProUGUI actionNameText;
    public KeyBindButton bindKeyButton;

    private ActionKey currentAction;

    public void SetAction(ActionKey action)
    {
        currentAction = action;
        actionNameText.text = action.ToString();
        bindKeyButton.Initialize(action);
    }
}