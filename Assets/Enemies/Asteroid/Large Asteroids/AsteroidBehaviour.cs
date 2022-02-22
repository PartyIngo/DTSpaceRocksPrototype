using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Asteroid Variables")]
    Rigidbody2D rb;


    [Tooltip("Maximum movement force of the Asteroid.")]
    public float forceMax;
    [Tooltip("Actual force of the Asteroid.")]
    Vector2 force;

    //[Header("Asteroid Force Variants")]
    //[Tooltip("max Spawning Force of the small Asteroid.")]
    //public float spawnForceSmall;
    //[Tooltip("max Spawning Force of the medium Asteroid.")]
    //public float spawnForceMedium;
    //[Tooltip("max Spawning Force of the large Asteroid.")]
    //public float spawnForceLarge;

    [Tooltip("max Health of the Asteroid.")]
    public float maxHealth;
    [Tooltip("current Health of the Asteroid. If it reaches 0, the Asteroid is destroyed.")]
    float currentHealth;

    //[Header("Asteroid Health Variants")]
    //[Tooltip("max Health of the small Asteroid.")]
    //public float healthSmall;
    //[Tooltip("max Health of the medium Asteroid.")]
    //public float healthMedium;
    //[Tooltip("max Health of the large Asteroid.")]
    //public float healthLarge;

    [Header("Asteroid Health Variants")]
    [Tooltip("The Mass of the Asteroid.")]
    public float mass;
    //[Tooltip("The Mass of the medium Asteroid.")]
    //public float massMedium;
    //[Tooltip("The Mass of the large Asteroid.")]
    //public float massLarge;

    [Tooltip("Maximum X-Coord.")]
    float Xmax;
    [Tooltip("Maximum Y-Coord.")]
    float Ymax;

    [Tooltip("Minimum Amount of spawnable children")]
    public int minChildrenAmount;
    [Tooltip("Maximum Amount of spawnable children")]
    public int maxChildrenAmount;

    [Tooltip("The Asteroid Asset in the next smaller variant")]
    public GameObject asteroidChild;
    public GameObject spawnHandler;

    //[Header("Asteroid' General Appearance")]
    //[Tooltip("The scale of the asteroid")]
    //public float asteroidScale;             //Can be removed bc its nnever used in Code
    //[Tooltip("The Size of the Asteroid. The larger the value, the larger the size.")]
    //[Range(1, 3)]
    //public int asteroidSize;
    [Tooltip("The Sprite Renderer Component")]
    SpriteRenderer spriteRenderer;
    //[Tooltip("The Animation Component")]
    //Animation animation;
    //[Tooltip("The Anomator Component")]
    //Animator animator;

    //[Tooltip("Every Asteroid Sprite that may be assigned")]
    //public Sprite[] asteroidSprite;
    //[Tooltip("The assigned sprite.")]
    //[HideInInspector]
    //public int spriteIndex;

    //[Header("Asteroid Scale")]
    //[Tooltip("The Scale of the small asteroid variant")]
    //public float asteroidScaleSmall;
    //[Tooltip("The Scale of the medium asteroid variant")]
    //public float asteroidScaleMedium;
    //[Tooltip("The Scale of the large asteroid variant")]
    //public float asteroidScaleLarge;




    //[Tooltip("The current variant of the asteroid's general appearance")]
    //int currentVariant;
    //bool isChild = false;


    [Header("VFX Stats")]

    [Tooltip("The VFX that plays, when the Asteroid gets destroyed")]
    public GameObject burstVFX;

    [Tooltip("threshold in percent when the asteroid should change the appearance when health falls below this value")]
    [Range(1, 99)]
    public float visibleDamageThreshold;
    [Tooltip("Color tint when damaged")]
    public Color damageTint;

    [Tooltip("Duration of Color tint when damaged in seconds")]
    public float damageDuration;
    [Tooltip("If the asteroids gets damage just now")]
    bool getsDamage;
    float nextTime;






    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.mass = mass;
        force = new Vector2(Random.Range(-forceMax, forceMax), Random.Range(-forceMax, forceMax));
        rb.AddForce(force);

        //Get access to some important conponents
        spriteRenderer = GetComponent<SpriteRenderer>();

        //Adds random torque for more juice
        rb.AddTorque(Random.Range(-forceMax/8, forceMax/8), ForceMode2D.Force);

        currentHealth = maxHealth;
        visibleDamageThreshold /= 100;



        //Randomize the color of the Asteroid
        //spriteRenderer.material.color = Random.ColorHSV(0, 1, 0, 1, 1, 1);

    }

    // Update is called once per frame
    void Update()
    {
        //When the Asteroid is destroyed, some things have to occur
        if (currentHealth <= 0)
        {
            //VFX explosion
            Instantiate(burstVFX, transform.position, transform.rotation);

            //Spawn child asteroids
            int temp = Random.Range(minChildrenAmount, maxChildrenAmount + 1);
            for (int i = 0; i < temp; i++)
            {

                GameObject newSpawn = Instantiate(asteroidChild, transform.position, transform.rotation);
                //Call Method of child to change the appaerance (brown purple or red)
                //changeAppearance("red"); --> this statement is working but not yet implemented...
            }

            //Increase Score
            ScoreScript.scoreValue += 1;

            //Destroy this instance of asteroid
            spawnHandler.GetComponent<SpawnHandlerBehavior>().destroyEntity(gameObject);
        }


        if (getsDamage)
        {
            spriteRenderer.color = damageTint;
            nextTime = Time.time + damageDuration;
            getsDamage = false;
        }

        //TODO: optimizing that the color changes not every frame
        if (Time.time > nextTime)
        {
            spriteRenderer.color = Color.white;
        }
    }



    /**
     * TBD: implementation && has to be called by instantiating child asteroids.
     * Sets the appearance of Asteroid
     */
    void changeAppearance(string variant)
    {
        ////Child asteroids should not get textures that are colorized different than their parent asteroids
        //switch (variant)
        //{
        //    case "brown":
        //        //Choose brown set of Sprites
        //        break;
        //    case "purple":
        //        //Choose purple set of Sprites

        //        break;
        //    case "red":
        //        //Choose red set of Sprites
        //        break;
        //    default:
        //        break;
        //}
    }



/**
 * Damage & Destroy VFX
 */
public void Damage(float damage)
    {
        currentHealth -= damage;

        getsDamage = true;

        //when current Health falls below a specific threshold (or less than X% of health are remaining), viisible damage is shown as a sprite.
        if (currentHealth < (maxHealth * visibleDamageThreshold))
        {
            //TODO: change Sprite of Asteroid to cracked variant
            //spriteRenderer.sprite = crackedAsteroids[currentVariant]; //should be working, but has to be tested when other sprites are available
        }
    }


    /**
     * Sets Variable Xmax to parameter value
    // */
    //public void setXmax(float newX)
    //{
    //    Xmax = newX;
    //}

    ///**
    //* Sets Variable Ymax to parameter value
    //*/
    //public void setYmax(float newY)
    //{
    //    Ymax = newY;
    //}
}
