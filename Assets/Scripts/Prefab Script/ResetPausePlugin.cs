using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetPausePlugin : MonoBehaviour
{
    public Button resetButton;
    public Button pauseButton;
    public Button mainMenuButton; // ← 添加主菜单按钮引用
    [SerializeField] TextMeshProUGUI gamePausedText;
    [SerializeField] bool isProd = true;

    private bool isPaused = false;
    private Firebase firebase;

    void Start()
    {
        firebase = FindAnyObjectByType<Firebase>();

        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetClicked);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseClicked);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked); // ← 添加监听
    }

    void OnResetClicked()
    {
        Time.timeScale = 1f;

        //update restart counter in firebase analytics
        firebase.IncreaseGameOverReset();

        //send analytics data
        if(isProd)
            firebase.SendData();
        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    void OnPauseClicked()
    {
        isPaused = !isPaused;
        gamePausedText.gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        Text label = pauseButton.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = isPaused ? "Resume" : "Pause";
        }
    }

    void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        
        //send analytics data
        if (isProd)
            firebase.SendData();
        
        SceneManager.LoadScene("MainMenu"); // ← 加载主菜单场景
    }
}
