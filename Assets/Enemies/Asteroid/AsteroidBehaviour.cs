using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Asteroid Variables")]

    [Tooltip("Maximum movement force of the Asteroid.")]
    public float forceMax;
    [Tooltip("Actual force of the Asteroid.")]
    Vector2 force;

    [Tooltip("max Health of the Asteroid.")]
    public float maxHealth;
    [Tooltip("current Health of the Asteroid. If it reaches 0, the Asteroid is destroyed.")]
    float currentHealth;

    Rigidbody2D rb;

    [Tooltip("Maximum X-Coord.")]
    float Xmax;
    [Tooltip("Maximum Y-Coord.")]
    float Ymax;

    [Tooltip("Minimum Amount of spawnable children")]
    public int minChildrenAmount;
    [Tooltip("Maximum Amount of spawnable children")]
    public int maxChildrenAmount;

    public GameObject asteroidChild;

    [Header("Asteroid' General Appearance")]
    [Tooltip("The scale of the asteroid")]
    public float asteroidScale;             //Can be removed bc its nnever used in Code
    [Tooltip("The Size of the Asteroid. The larger the value, the larger the size.")]
    [Range(1, 3)]
    public int asteroidSize;
    [Tooltip("The Sprite Renderer Component")]
    SpriteRenderer spriteRenderer;
    [Tooltip("Every Asteroid Sprite that may be assigned")]
    public Sprite[] asteroidSprite;
    [Tooltip("The assigned sprite.")]
    [HideInInspector]
    public int spriteIndex;

    [Tooltip("The Scale of the small asteroid variant")]
    public float asteroidScaleSmall;
    [Tooltip("The Scale of the medium asteroid variant")]
    public float asteroidScaleMedium;
    [Tooltip("The Scale of the large asteroid variant")]
    public float asteroidScaleLarge;

    [Tooltip("The current variant of the asteroid's general appearance")]
    int currentVariant;
    bool isChild = false;


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
        //Initializing some parameters
        currentHealth = maxHealth;
        visibleDamageThreshold /= 100;

        print("Force Max    " + forceMax);

        force = new Vector2(Random.Range(-forceMax, forceMax), Random.Range(-forceMax, forceMax));
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(force);

        spriteRenderer = GetComponent<SpriteRenderer>();

        //Choose and assign the sprite for the asteroid
        changeAppearance();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 tmp = transform.position;

        if (transform.position.x > Xmax)
        {
            tmp.x = -Xmax + 1;   
            transform.position = tmp;
        }

        if (transform.position.x < -Xmax)
        {
            tmp.x = Xmax - 1; 
            transform.position = tmp;
        }

        if (transform.position.y > Ymax)
        {
            tmp.y = -Ymax + 1;
            transform.position = tmp;
        }

        if (transform.position.y < -Ymax)
        {
            tmp.y = Ymax - 1;
            transform.position = tmp;
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
     * Sets the appearance of Asteroid
     */
    void changeAppearance()
    {
        //Child asteroids should not get textures that are colorized different than their parent asteroids
        if (!isChild)
        {
            //assign random sprite
            currentVariant = Random.Range(0, asteroidSprite.Length);
        }

        spriteRenderer.sprite = asteroidSprite[currentVariant];
       
        Vector3 newScale;
        float tempScale;

        switch (asteroidSize)
        {
            case 1:
                tempScale = asteroidScaleSmall;
                break;
            case 2:
                tempScale = asteroidScaleMedium;
                break;
            case 3:
                tempScale = asteroidScaleLarge;
                break;
            default:
                tempScale = 1;
                break;
        }
        
        //change scale of asteroid based on asteroidSize
        newScale.x = tempScale; // asteroidScale * (asteroidSize / 3);
        newScale.y = tempScale; //asteroidScale * (asteroidSize / 3);
        newScale.z = 1;
        gameObject.transform.localScale = newScale;
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

        if (currentHealth <= 0)
        {
            //GameObject handler = GameObject.Find("SpawnHandler");
            //handler.gameObject.SendMessage("decreaseOverallWeight", "Asteroid Large");

            //VFX explosion
            Instantiate(burstVFX, transform.position, transform.rotation);



            //Spawn child asteroids
            if (asteroidSize > 1)
            {
                int temp = Random.Range(minChildrenAmount, maxChildrenAmount);
                for (int i = 0; i < temp; i++)
                {

                    GameObject newSpawn = Instantiate(asteroidChild, transform.position, transform.rotation);
                    newSpawn.gameObject.SendMessage("decreaseSize", asteroidSize - 1);
                    newSpawn.gameObject.SendMessage("setVariant", currentVariant);
                    newSpawn.gameObject.SendMessage("setXmax", Xmax);
                    newSpawn.gameObject.SendMessage("setYmax", Ymax);
                }
            }

            //child: spriteIndex = rand;
            //Destroy this instance of asteroid
            Destroy(gameObject);
        }
    }

    /**
     * Sets the Size to paramenter value
     */
    public void decreaseSize(int newSize)
    {
        asteroidSize = newSize;
    }

    /**
     * Sets the current variant of child asteroids
     */
    public void setVariant(int newVariant)
    {
        currentVariant = newVariant;
        isChild = true;
    }

    /**
     * Sets Variable Xmax to parameter value
     */
    public void setXmax(float newX)
    {
        Xmax = newX;
    }

    /**
    * Sets Variable Ymax to parameter value
    */
    public void setYmax(float newY)
    {
        Ymax = newY;
    }
}
