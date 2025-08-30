using UnityEditor;
using UnityEngine;

public class PlayerPrefsResetTool
{
    [MenuItem("Tools/Reset All Level Progress")]
    public static void ResetLevelProgress()
    {
        if (EditorUtility.DisplayDialog("Reset Level Progress",
            "Are you sure you want to delete all PlayerPrefs? This cannot be undone.",
            "Yes", "No"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("Level 0_Unlocked", 1);
            PlayerPrefs.SetInt("Level 0_Completed", 0);
            PlayerPrefs.Save();

            Debug.Log("✅ All PlayerPrefs cleared. Level 0 has been unlocked but not completed.");
        }
    }
}