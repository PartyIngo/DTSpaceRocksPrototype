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

    [Tooltip("Health of the Asteroid. If it reaches 0, the Asteroid is destroyed.")]
    public float health;

    Rigidbody2D rb;

    [Tooltip("Maximum X-Coord.")]
    public float Xmax;
    [Tooltip("Maximum Y-Coord.")]
    public float Ymax;


    [Header("Asteroid Appearance")]

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
    [Tooltip("The VFX that plays, when the asateroid gets destroyed")]
    public GameObject burstVFX;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
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
    }



    /**
     * Channge appearance of Asteroid, when it's the largest version of it
     */
    void changeAppearance()
    {
        if (asteroidSize == 3)
        {
            //assign random sprite
            int rand = Random.Range(0, asteroidSprite.Length);
            spriteRenderer.sprite = asteroidSprite[rand];
            spriteIndex = rand;
        }
    }



    /**
     * Damage & Destroy VFX
     */
    public void Damage(float damage)
    {
        print("Asteroid Damage:  " + damage);

        health -= damage;

        if (health <= 0)
        {
            GameObject handler = GameObject.Find("SpawnHandler");
            handler.gameObject.SendMessage("decreaseOverallWeight", "Asteroid Large");


            //TODO: VFX explosion
            Instantiate(burstVFX, transform.position, transform.rotation);


            //TODO: Spawn child asteroids


            Destroy(gameObject);
        }
    }
}
