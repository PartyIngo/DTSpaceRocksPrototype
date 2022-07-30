using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayInteractions : MonoBehaviour
{
    public GameObject pauseOverlay;

    bool isPaused = false;
    public void pauseGame()
    {
        isPaused = !isPaused;
        //pauseOverlay.gameObject.setActive(isPaused);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
