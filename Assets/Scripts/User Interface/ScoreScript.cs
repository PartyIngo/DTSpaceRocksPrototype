using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreScript : MonoBehaviour
{
    public GameObject gameOverOverlay;

    public static int scoreValue = 0;
    public static string scoreName = "NULL";
    Text score;

    public static bool isOverlayActive = false;



    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score:    " + scoreValue;


        if (isOverlayActive)
        {
            showOverlay();
        }
    }





    //TBD: Remove control from game and use controls on Overlay. When we can go to Main Menu and Restart the game
    //      by using the gampead, everything is fine
    //      After that, we add a input field, where the player can insert their name
    public void showOverlay()
    {
        gameOverOverlay.gameObject.SetActive(true);
        isOverlayActive = false;
    }







    public void loadMenu()
    {
        SceneManager.LoadScene("MainMenu");


    }

    public void restartScene()
    {
        //Reload Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

}
