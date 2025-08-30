using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("关卡编号（从 0 开始）")]
    public int levelIndex;

    public string levelName => "Level " + levelIndex;

    private Button button;
    private LevelStatusIcon statusIcon;

    void Awake()
    {
        button = GetComponent<Button>();
        statusIcon = GetComponentInChildren<LevelStatusIcon>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    public void SetInteractable(bool unlocked)
    {
        if (button != null)
            button.interactable = unlocked;

        bool completed = PlayerPrefs.GetInt(levelName + "_Completed", 0) == 1;

        if (statusIcon != null)
        {
            if (!unlocked)
                statusIcon.SetState(LevelStatusIcon.IconState.Locked);
            else if (completed)
                statusIcon.SetState(LevelStatusIcon.IconState.Checkmark);
            else
                statusIcon.SetState(LevelStatusIcon.IconState.Circle);
        }
    }

    public void OnClick()
    {
        if (button != null && button.interactable && !string.IsNullOrEmpty(levelName))
        {
            Debug.Log("Loading scene: " + levelName);
            SceneManager.LoadScene(levelName);
        }
    }
}