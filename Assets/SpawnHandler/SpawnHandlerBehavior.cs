using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TODO: 
 * - Weight and isLooping as properties in respective Prefaby (Player, Asteroid)
 * - The Spawn Handler should get access to these properties, to store them in a list and check for looping
 */


public class SpawnHandlerBehavior : MonoBehaviour
{
    #region Variables

    [Header("The Gameobjects, the Spawn Handler has to handle")]
    [Tooltip("The Reference to the Player Character")]
    public GameObject playerCharacter;

    [Tooltip("Asteroid Game Object prefabs")]
    public GameObject largeAsteroid;
    public GameObject mediumAsteroid;
    public GameObject smallAsteroid;
    int colorVariant;


    [Tooltip("Array of existing Entities")]
    List<GameObject> existingEntities;



    [Header("General Spawner Settings")]
    [Tooltip("The Delay until the next entity spawns in seconds")]
    public float spawnCooldown;
    [Tooltip("For timer calculation")]
    float nextSpawnTime;

    [Header("Boundaries of playing area")]
    [Tooltip("Maximum X-Coord.")]
    public float Xmax;
    [Tooltip("Maximum Y-Coord.")]
    public float Ymax;






    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //Create the List
        existingEntities = new List<GameObject>();

        //Spawn an instance of the Player Character and add it to the List
        Vector2 spawnCoords = new Vector2(0, 0);
        GameObject tmp = Instantiate(playerCharacter, spawnCoords, transform.rotation);
        //addToList(tmp);


        //Reset timer for next spawn
        nextSpawnTime = Time.time + spawnCooldown;
    }

    /**
     * Check if it's time to spawn a new Entity.
     * Also checks if Entities are moved out of the Playing Field.
     */
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnCooldown;


            //set the Appearance of the Asteroid by a random determination
            colorVariant = Random.Range(1, 4);
            //Spawn a new, random Entity on the determined Coordinates
            spawnEntity(Random.Range(1,4), colorVariant, determineCoordinates());
        }

        checkBoundaries();
    }


    /**
     * Checks every Entity that steps over the boundaries and changes it's position to the opposite border, so that they are in the field again
     */
    void checkBoundaries()
    {
        //Create a List with Playercahracter, Enemy Entities (Future: and Items)
        existingEntities.Add(GameObject.FindGameObjectWithTag("Player"));
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            existingEntities.Add(enemies[i]);
        }
        

        //Iterates through every entity, checks boundaries and moves object back to the playing field.
        foreach (GameObject enemyEntity in existingEntities)
        {
            Vector3 tmp = enemyEntity.transform.position;

            if (enemyEntity.transform.position.x > Xmax)
            {
                //Change it's position to be inside of the left border of the field
                tmp.x = -Xmax + 1;   //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
                enemyEntity.transform.position = tmp;
            }

            //If spaceship has passed the left border...
            if (enemyEntity.transform.position.x < -Xmax)
            {
                //Change it's position to be inside of the left border of the field
                tmp.x = Xmax - 1;   //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
                enemyEntity.transform.position = tmp;
            }

            //If spaceship hahs passed top border...
            if (enemyEntity.transform.position.y > Ymax)
            {
                //Change it's position to be inside of the left border of the field
                tmp.y = -Ymax + 1;    //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
                enemyEntity.transform.position = tmp;
            }

            //If spaceship hahs passed bottom border...
            if (enemyEntity.transform.position.y < -Ymax)
            {
                //Change it's position to be inside of the left border of the field
                tmp.y = Ymax - 1;    //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
                enemyEntity.transform.position = tmp;
            }
        }

        //Clear the List
        existingEntities.Clear();

    }

    Vector3 determineCoordinates()
    {
        //Important Variables
        float axis = Random.Range(1, 5);
        Vector3 spawnCoords = new Vector2();

        //Determine on which border of the screen the entity should be spawned
        switch (axis)
        {
            //North
            case 1:
                spawnCoords.x = Random.Range(-Xmax + 1, Xmax - 1);
                spawnCoords.y = -Ymax + 1;
                break;
            //South
            case 2:
                spawnCoords.x = Random.Range(-Xmax + 1, Xmax - 1);
                spawnCoords.y = Ymax - 1;
                break;
            //East
            case 3:
                spawnCoords.x = Xmax - 1;
                spawnCoords.y = Random.Range(-Ymax + 1, Ymax - 1);
                break;
            //West
            case 4:
                spawnCoords.x = -Xmax + 1;
                spawnCoords.y = Random.Range(-Ymax + 1, Ymax - 1);
                break;
            default:
                break;
        }
        spawnCoords.z = 0;
        
        return spawnCoords;
    }




    /**
     * Spawns a new Entity on respective coordinates, so that they can fly in the field to appear.
     */
    public void spawnEntity(int asteroidSize, int newColorVariant, Vector3 coordinates)
    {
        colorVariant = newColorVariant;


        GameObject newEntity = null;
        
        //Instantiate the new Entity on the respective coordinates
        switch (asteroidSize)
        {
            //Large Asteroid
            case 3:
                newEntity = Instantiate(largeAsteroid, coordinates, transform.rotation);
                break;
            //Medium Asteroid
            case 2:
                newEntity = Instantiate(mediumAsteroid, coordinates, transform.rotation);
                break;
            //Small Asteroid
            case 1:
                newEntity = Instantiate(smallAsteroid, coordinates, transform.rotation);
                break;
            default:
                break;
        }
                

        if (newEntity != null)
        {
            newEntity.SendMessage("ChangeAppearance", colorVariant);
        }
    }


    /**
     *  Destroys an entity
     */
    public void destroyEntity(GameObject target)
    {
        //Destroy(target.GetComponent<SpriteRenderer>());
        Destroy(target);
    }
}
