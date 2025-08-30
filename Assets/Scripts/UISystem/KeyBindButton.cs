using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindButton : MonoBehaviour
{
    public TextMeshProUGUI buttonLabel;
    private Button button;
    private ActionKey boundAction;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void Initialize(ActionKey action)
    {
        boundAction = action;
        UpdateLabel();
    }

    void UpdateLabel()
    {
        if (KeyBindingManager.Instance != null)
        {
            var key = KeyBindingManager.Instance.GetKeyForAction(boundAction);
            buttonLabel.text = key.ToString();
        }
    }

    void OnClick()
    {
        buttonLabel.text = "...";
        KeyBindingManager.Instance.StartBinding(boundAction, (key) =>
        {
            UpdateLabel();
        });
    }
}
