using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playMobileButton; // Assign this button in the Inspector

    void Start()
    {
        if (playMobileButton != null)
        {
            playMobileButton.onClick.AddListener(PlayGameMobile);
        }
    }

    public void PlayGame()
    {
        GameSettings.UseMobileControls = false; // Set mobile controls to false for the regular play
        Debug.Log("MainMenu: 'Play Game' button clicked. GameSettings.UseMobileControls set to: " + GameSettings.UseMobileControls);
        SceneManager.LoadScene(1);
    }

    public void PlayGameMobile()
    {
        GameSettings.UseMobileControls = true; // Set mobile controls to true for the mobile version
        Debug.Log("MainMenu: 'Play Mobile' button clicked. GameSettings.UseMobileControls set to: " + GameSettings.UseMobileControls);
        SceneManager.LoadScene(1);
    }
}