using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Asteroid Variables")]
    public int asteroidTier;
    Rigidbody2D rb;

    [Tooltip("Maximum movement force of the Asteroid.")]
    public float forceMax;
    [Tooltip("Actual force of the Asteroid.")]
    Vector2 force;

    [Tooltip("max Health of the Asteroid.")]
    public float maxHealth;
    [Tooltip("current Health of the Asteroid. If it reaches 0, the Asteroid is destroyed.")]
    float currentHealth;


    [Header("Asteroid Health Variants")]
    [Tooltip("The Mass of the Asteroid.")]
    public float mass;
    [Tooltip("�How many points should the score increase, when this Asteroid is destroyed")]
    public int scorePoints;

    [Tooltip("Minimum Amount of spawnable children")]
    public int minChildrenAmount;
    [Tooltip("Maximum Amount of spawnable children")]
    public int maxChildrenAmount;

    [Tooltip("The Asteroid Asset in the next smaller variant")]
    public GameObject asteroidChild;
    public GameObject spawnHandler;

    [Tooltip("The Sprite Renderer Component")]
    SpriteRenderer spriteRenderer;


    [Header("Appearance Stats")]
    public Sprite[] redVariant;
    public Sprite[] brownVariant;
    public Sprite[] purpleVariant;
    public int currentVariant;

    [Header("VFX Stats")]
    [Tooltip("The VFX that plays, when the Asteroid gets destroyed")]
    public GameObject burstVFX;

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

        //Request a new Value got the Order in Layer
        spawnHandler.GetComponent<SceneVariables>().newOrder(asteroidTier);

        //Get the new Value of orderInlayer and assign it to the gameObject to avoid jitter, when asteroids move over each other
        switch (asteroidTier)
        {
            case 3:
                spriteRenderer.sortingOrder = spawnHandler.GetComponent<SceneVariables>().orderInLayerTier3;
                break;
            case 2:
                spriteRenderer.sortingOrder = spawnHandler.GetComponent<SceneVariables>().orderInLayerTier2;
                break;
            case 1:
                spriteRenderer.sortingOrder = spawnHandler.GetComponent<SceneVariables>().orderInLayerTier1;
                break;
            default:
                break;
        }

        //Adds random torque for more juice
        rb.AddTorque(Random.Range(-forceMax/8, forceMax/8), ForceMode2D.Force);

        //Reset the currentHealth to the maximum amount
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //When the Asteroid is destroyed, some things have to occur
        if (currentHealth <= 0)
        {
            //VFX explosion
            Instantiate(burstVFX, transform.position, Quaternion.identity);

            //Spawn child asteroids
            int temp = Random.Range(minChildrenAmount, maxChildrenAmount + 1);
            for (int i = 0; i < temp; i++)
            {
                GameObject newSpawn = Instantiate(asteroidChild, transform.position, transform.rotation);
                newSpawn.SendMessage("ChangeAppearance", currentVariant);
            }

            //Increase Score
            ScoreScript.scoreValue += scorePoints;

            //Destroy this instance of asteroid
            spawnHandler.GetComponent<SpawnHandlerBehavior>().destroyEntity(gameObject);
        }

        //Change the Color of the Asteroid for a short time, when it gets damage
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
    void ChangeAppearance(int variant)
    {
        currentVariant = variant;

        switch (variant)
        {
            //Red variant
            case 1:
                gameObject.GetComponent<AnimationHandler>().sprites[0] = redVariant[0];
                gameObject.GetComponent<AnimationHandler>().sprites[1] = redVariant[1];
                gameObject.GetComponent<AnimationHandler>().sprites[2] = redVariant[2];
                gameObject.GetComponent<AnimationHandler>().sprites[3] = redVariant[3];
                break;
            //Brown Variant
            case 2:
                gameObject.GetComponent<AnimationHandler>().sprites[0] = brownVariant[0];
                gameObject.GetComponent<AnimationHandler>().sprites[1] = brownVariant[1];
                gameObject.GetComponent<AnimationHandler>().sprites[2] = brownVariant[2];
                gameObject.GetComponent<AnimationHandler>().sprites[3] = brownVariant[3];
                break;
            //Purple Variant
            case 3:
                gameObject.GetComponent<AnimationHandler>().sprites[0] = purpleVariant[0];
                gameObject.GetComponent<AnimationHandler>().sprites[1] = purpleVariant[1];
                gameObject.GetComponent<AnimationHandler>().sprites[2] = purpleVariant[2];
                gameObject.GetComponent<AnimationHandler>().sprites[3] = purpleVariant[3];
                break;
        }
    }

    /**
     * Damage & Destroy VFX
     */
    public void Damage(float damage)
    {
        currentHealth -= damage;

        getsDamage = true;
    }
}