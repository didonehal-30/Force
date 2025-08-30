using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField] private int maxLevelCount = 50;

    void Awake()
    {
        // 初始化 Level 0 通关状态为未通关
        if (!PlayerPrefs.HasKey("Level 0_Completed"))
        {
            PlayerPrefs.SetInt("Level 0_Completed", 0);
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        LevelButton[] buttons = GetComponentsInChildren<LevelButton>();

        foreach (var lb in buttons)
        {
            if (lb.levelIndex < 0 || lb.levelIndex >= maxLevelCount)
                continue;

            bool unlocked = (lb.levelIndex == 0 ||
                PlayerPrefs.GetInt("Level " + (lb.levelIndex - 1) + "_Completed", 0) == 1);

            lb.SetInteractable(unlocked);
        }
    }
}