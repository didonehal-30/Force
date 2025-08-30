using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResetManager : MonoBehaviour
{
    [Header("Reset Bottom")]
    public KeyCode resetKey = KeyCode.R;

    [Header("UI")]
    public TextMeshProUGUI hintText;

    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            ReloadCurrentScene();
        }
    }

    void Start()
    {
        if (hintText != null)
        {
            hintText.text = $"Press [{resetKey}] to Restart";
        }
    }

    public void ReloadCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}
