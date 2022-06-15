using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIButtonAnimationBehavior : MonoBehaviour
{
    [Tooltip("The Sprites for animating the asset. Animation will loop")]
    public Sprite[] sprites;

    [Tooltip("The default Sprite without Animation")]
    public Sprite[] defaultSprites;
    [Tooltip("the interval between the frames in seconds")]
    public float interval;

    //Timer Variables
    float nextTime;

    //Animation variables
    byte currentIndex;
    Image image;

    public bool startAnimation = false;


    // Start is called before the first frame update
    void Start()
    {
        nextTime = Time.time + interval;
        image = GetComponent<Image>();
        image.sprite = defaultSprites[0]; // sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        //print("Mouse over Start Button");

        if (startAnimation)
        {
            if (Time.time > nextTime)
            {
                //Reset Timer
                nextTime = Time.time + interval;

                currentIndex++;

                if (currentIndex >= sprites.Length)
                {
                    currentIndex = 0;
                }

                image.sprite = sprites[currentIndex];
            }
        }

        //else
        //{
        //    if (Time.time > nextTime)
        //    {
        //        //Reset Timer
        //        nextTime = Time.time + interval;

        //        currentIndex++;

        //        if (currentIndex >= sprites.Length)
        //        {
        //            currentIndex = 0;
        //        }

        //        image.sprite = defaultSprites[currentIndex];
        //    }
        //}

    }

    public void StartAnimate()
    {
        startAnimation = true;
    }
    public void StopAnimate()
    {
        startAnimation = false;

        image.sprite = defaultSprites[0];

    }

}
