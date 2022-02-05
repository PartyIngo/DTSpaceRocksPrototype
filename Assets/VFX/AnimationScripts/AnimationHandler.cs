using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    [Tooltip("The Sprites for animating the asset. Animation will loop")]
    public Sprite[] sprites;
    [Tooltip("the interval between the frames in seconds")]
    public float interval;

    //Timer Variables
    float nextTime;

    //Animation variables
    byte currentIndex;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        nextTime = Time.time + interval;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        updateAnimation();
    }

    /**
     * Check if interval is over. Then change the sprite and reset the Timer
     */
    void updateAnimation()
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

            spriteRenderer.sprite = sprites[currentIndex];
        }
    }
}
