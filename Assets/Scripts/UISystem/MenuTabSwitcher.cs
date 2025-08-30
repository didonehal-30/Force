using UnityEngine;

public class MenuTabSwitcher : MonoBehaviour
{
    public GameObject keyBindingPanel, volumePanel, exitPanel;

    public void ShowKeyBinding()
    {
        keyBindingPanel.SetActive(true);
        volumePanel.SetActive(false);
        exitPanel.SetActive(false);
    }

    public void ShowVolume()
    {
        keyBindingPanel.SetActive(false);
        volumePanel.SetActive(true);
        exitPanel.SetActive(false);
    }

    public void ShowExit()
    {
        keyBindingPanel.SetActive(false);
        volumePanel.SetActive(false);
        exitPanel.SetActive(true);
    }
}
