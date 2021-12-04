using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFlareBehavior : MonoBehaviour
{
    [SerializeField]
    [Tooltip("min Lifetime of a Star")]
    float minLifetime;

    [SerializeField]
    [Tooltip("max Lifetime of a Star")]
    float maxLifetime;

    SpriteRenderer appearance;

    float lerpDuration;
    float timeElapsed;
    float lifeTime;
    float targetSize;

    byte starState;         //State 0: fading in; State 1: flare; State 2: fade out

    Vector3 initScale;
    Vector3 newScale;
    Vector3 currentScale;

    // Start is called before the first frame update
    void Start()
    {
        //Initializing somme variables
        initScale = new Vector3(0, 0, 0);
        currentScale = new Vector3(0, 0, 0);

        starState = 0;
        lerpDuration = 1;

        lifeTime = 0;

        //setting up random size to scale to by fading in
        targetSize = Random.Range(0.6f, 0.7f);
        currentScale.x = targetSize;
        currentScale.y = targetSize;
        currentScale.z = 1;

        //Setting up the random color of the star
        //TODO: Lerp color between yellow and blue/Random value inbetween
        appearance.color = Random.ColorHSV(0, 1,           //Hue min max
                                            0, 0.75f,       //Saturation min max
                                            0.9f, 1);       //Value min max

        //Sprite Renderer Component needed to change the color afterwards
        appearance = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //State Machine, to fade in, flare and then fade out and destroy the star
        switch (starState)
        {
            //Fading in
            case 0:
                fadeIn();
                break;
            
            //Flare
            case 1:
                flare();
                break;
            
            //Fading out & Destroy
            case 2:
                fadeOut();
                break;

            default:
                break;
        }
    }

    /**
     * Scales the star up to make it's appearance loook better.
     * If the star is fully scaled up, the state changes
     */
    void fadeIn()
    {
        //Scales the star up by using the lerp function
        if (timeElapsed < lerpDuration)
        {
            newScale = Vector3.Lerp(initScale, currentScale, timeElapsed / lerpDuration);
            gameObject.transform.localScale = newScale;
            timeElapsed += Time.deltaTime;
        }
        //After the star has it's target scale, reset the timer variable and switch the state
        else
        {
            timeElapsed = 0;
            starState = 1;
        }
    }

    /**
     * the star flares by changiing it's scale a little bit every frame
     */
    void flare()
    {
        //Randomize the new scale every time, to make the star flare a bit. If these values are too far away from each other, the flare will look unnaturally
        targetSize = Random.Range(0.6f, 0.7f);
        currentScale.x = targetSize;
        currentScale.y = targetSize;
        currentScale.z = 1;

        //apply the new scale to the star
        gameObject.transform.localScale = currentScale;

        //Initialize lifetime. Within the lifetime, the star flares
        if (lifeTime == 0)
        {
            lifeTime = Time.time + Random.Range(minLifetime, maxLifetime);
        }

        //If the lifetime is over, the state has to be switched, that the star can fade out and be destroyed
        if (Time.time > lifeTime)
        {
            starState = 2;
        }
    }


    /**
     * The star scales down and disappears. Then it will be destroyed
     */
    void fadeOut()
    {
        //Scale the star down
        if (timeElapsed < lerpDuration)
        {
            newScale = Vector3.Lerp(currentScale, initScale, timeElapsed / lerpDuration);
            gameObject.transform.localScale = newScale;
            timeElapsed += Time.deltaTime;
        }
        //When the star is decreased completely, destroy it
        else
        {
            Destroy(gameObject);
        }
    }
}
