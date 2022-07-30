using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreScript : MonoBehaviour
{
    public GameObject gameOverOverlay;
    public GameObject nameEntry;
    public GameObject nameEntryText;
    Text nameText;

    public static int scoreValue = 0;
    public static string scoreName = "NULL";
    Text score;
    public GameObject gameOverScore;
    Text gameOverScoreText;

    public static bool isOverlayActive = false;

    public GameObject pauseOverlay;
    public bool isPaused = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
        gameOverScoreText = gameOverScore.GetComponent<Text>();
        nameText = nameEntryText.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        pauseGame();





        score.text = "Score:    " + scoreValue;


        if (isOverlayActive)
        {
            showOverlay();
        }
    }

    //TBD: Remove control from game and use controls on Overlay. When we can go to Main Menu and Restart the game
    //      by using the gampead, everything is fine
    //      After that, we add an input field, where the player can insert their name
    public void showOverlay()
    {
        gameOverOverlay.gameObject.SetActive(true);
        isOverlayActive = false;

        Debug.Log("NEW SCORE" + scoreValue);

        gameOverScoreText.text = scoreValue.ToString();

        if (PlayerPrefs.GetInt("lastPlacement") < scoreValue)
        {
            Debug.Log("CONGRATZ");
            nameEntry.SetActive(true);

            //TODO: other Overlay with String input.
        }
        else
        {
            nameEntry.SetActive(false);
            //TODO: normal Overlay
            Debug.Log("YOU SUCK");
        }
    }

    public void saveScore()
    {
        Debug.Log("SAVEDDDD" + nameText.text + " SCORE " + scoreValue);

        PlayerPrefs.SetInt("NewScore", scoreValue);
        PlayerPrefs.SetString("NewName", nameText.text);
        PlayerPrefs.Save();
    }

    public void loadMenu()
    {
        ////Reset Score
        //ScoreScript.scoreValue = 0;
        SceneManager.LoadScene("MainMenu");
    }

    public void restartScene()
    {
        ////Reset Score
        //ScoreScript.scoreValue = 0;
        //Reload Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void pauseGame()
    {
        bool tmp = Pause.isPaused;

        if (tmp)
        {
            pauseOverlay.SetActive(true);
            //pause the game
            Time.timeScale = 0f;
        }
        else
        {
            pauseOverlay.SetActive(false);
            //continue the game
            Time.timeScale = 1f;
        }



    }
}
