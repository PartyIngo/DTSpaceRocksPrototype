using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeAsteroidBehavior : MonoBehaviour
{

    [Header("Appearance Variants")]
    [Tooltip("The first variant of the large Asteroid")]
    public Sprite[] appearanceVariant1;
    [Tooltip("The second variant of the large Asteroid")]
    public Sprite[] appearanceVariant2;
    [Tooltip("The third variant of the large Asteroid")]
    public Sprite[] appearanceVariant3;

    [Tooltip("The current variant of the large Asteroid")]
    int currentVariant;

    [Header("Animation Parameters")]
    [Tooltip("Interval")]
    public float animationInterval;
    float nextTime;

    [Tooltip("The Sprite Renderer Component")]
    SpriteRenderer spriteRenderer;

    [Tooltip("Animation Index")]
    byte animationIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //determine the current variant for this Asteroid
        currentVariant = Random.Range(1, 4);
        spriteRenderer.sprite = appearanceVariant1[0];

        //Initialize Timer for Animation
        nextTime = Time.time + animationInterval;


        //Get access to important Components
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Animate();    
    }


    void Animate()
    {

        if (Time.time > nextTime)
        {
            //Reset Timer for Animation
            nextTime = Time.time + animationInterval;

            animationIndex++;

            if (animationIndex > (appearanceVariant1.Length - 1))
            {
                animationIndex = 0;
            }

            print("Index: " + animationIndex);

            //Change appearance
            spriteRenderer.sprite = appearanceVariant1[animationIndex];



        }
    }

}
