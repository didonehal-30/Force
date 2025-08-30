using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI gameEndTtitle;
    [SerializeField] Button restartButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button continueButton;
    [SerializeField] private TextMeshProUGUI StarCounter;
    private int collectedStars = 0;
    private int totalStarsInLevel = 0;
    [SerializeField] private TextMeshProUGUI FinalStars;
    [SerializeField] private TextMeshProUGUI ranktext;
    private Firebase firebase;
    private PlayerHealth playerHealth;

    [SerializeField] Canvas resetPauseCanvas;


    private bool isGameOver = true; //used to keep track of whether the restart UI button was pressed from game over screen or game finish screen
    [SerializeField] bool isProd = true; //used to disable sending data to firebase

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalStarsInLevel = GameObject.FindGameObjectsWithTag("Star").Length;
        UpdateStarUI();
        UnityEngine.Debug.Log("Im here");
        firebase = FindAnyObjectByType<Firebase>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        //if (firebase == null)
          //  UnityEngine.Debug.LogError("Firebase NOT found in Start!");
        //else
          //  UnityEngine.Debug.Log("Firebase found in Start");

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void UpdateStarUI()
    {
        if (StarCounter != null)
            StarCounter.text = collectedStars + " / " + totalStarsInLevel;
    }
    public void CollectStar()
    {
        collectedStars++;
        UpdateStarUI();
    }

    //called when the player loses
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0;

        gameEndTtitle.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
    }

    //called when the player finishes the level
    public void GameFinish()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt(currentScene + "_Completed", 1);
        PlayerPrefs.Save();
        isGameOver = false;
        Time.timeScale = 0;

        //disable resetPause Menu
        resetPauseCanvas.enabled = false;

        gameEndTtitle.text = "Well Done!";
        gameEndTtitle.color = new Color32(0, 255, 10, 255);

        gameEndTtitle.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }
        StarCounter.gameObject.SetActive(false);
        FinalStars.gameObject.SetActive(true);
        ranktext.gameObject.SetActive(true);

        FinalStars.text = "Stars : " + collectedStars + " / " + totalStarsInLevel;
        float percentage = (float)collectedStars / totalStarsInLevel;
        float healthPercentage = playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth();
        string rank;
        if (percentage >= 1f )
        {
            if (healthPercentage>= 1f)
                rank = "S";
            else if (healthPercentage>= 0.8f)
                rank = "A";
            else if (healthPercentage >= 0.5f)
                rank = "B";
            else
                rank = "C";
        }
            
        else if (percentage >= 0.8f)
        {
            if (healthPercentage >= 0.8f)
                rank = "A";
            else if (healthPercentage >= 0.5f)
                rank = "B";
            else
                rank = "C";
        }

     
        else if (percentage >= 0.5f)
        {
            if (healthPercentage >= 0.5f)
                rank = "B";
            else
                rank = "C";
        }
        else
            rank = "C";

        ranktext.text = "Rank: " + rank;
    }

    public void Restart()
    {
        Time.timeScale = 1;

        //update restart counter in firebase analytics
        if (isGameOver)
            firebase.IncreaseGameOverReset();
        else
            firebase.IncreaseGameFinishReset();

        //send analytics data
        if(isProd)
            firebase.SendData();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;

        //send analytics data
        if(isProd)
            firebase.SendData();

        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        Time.timeScale = 1;
       // if (firebase == null)
         //   UnityEngine.Debug.LogError("Firebase NOT found in Continue");
        //else
          //  UnityEngine.Debug.Log("Firebase found in Contine");
        //send analytics data
        if (isProd)
            firebase.SendData();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
