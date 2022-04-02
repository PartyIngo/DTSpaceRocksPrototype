using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuBehavior : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Starts the Game Scene
    public void startGame()
    {
        SceneManager.LoadScene("PSRGame");

    }

    //Quits the Application
    public void closeApplication()
    {
        print("CLOOOOOSE");
        Application.Quit();
    }
}
