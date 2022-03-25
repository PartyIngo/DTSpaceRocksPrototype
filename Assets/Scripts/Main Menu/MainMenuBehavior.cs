using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    [Header("Screen Assets")]
    [SerializeField]
    [Tooltip("The asset for the Title Screen")]
    GameObject titleScreen;

    [SerializeField]
    [Tooltip("The asset for the Main Menu Screen")]
    GameObject mainMenuScreen;

    [SerializeField]
    [Tooltip("The asset for the Credits Screen")]
    GameObject creditsScreen;

    [SerializeField]
    [Tooltip("The asset for the Options Menu Screen")]
    GameObject optionsMenuScreen;

    [SerializeField]
    [Tooltip("The asset for the Highscore Screen")]
    GameObject highscroreScreen;

    [Header("Default Buttons")]
    [SerializeField]
    [Tooltip("The first selected button for the Highscore Screen")]
    Button primaryButtonMM;


    byte currentScreen;

    // Start is called before the first frame update
    void Start()
    {
        currentScreen = 0;

        titleScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        creditsScreen.SetActive(false);
        optionsMenuScreen.SetActive(false);
        highscroreScreen.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        switch (currentScreen)
        {
            //No Switch needed
            case 0:
                break;

            //Title Screen
            case 1:

                break;

            //Main Menu Screen
            case 2:

                break;

            //Credits Screen
            case 3:

                break;

            //Options Menu Screen
            case 4:

                break;

            //Highscore Screen
            case 5:

                break;
            default:
                break;
        }
    }


    //Starts the Game Scene
    public void playGame()
    {
        SceneManager.LoadScene("PSRGame");
    }

    //Quits the Application
    public void quit()
    {
        Application.Quit();
    }



    //public void OnSubmit(InputAction.CallbackContext context)
    //{
    //    titleScreen.SetActive(false);
    //    mainMenuScreen.SetActive(true);

    //    primaryButtonMM.Select();

    //}





}
