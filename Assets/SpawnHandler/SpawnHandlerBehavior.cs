using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * TODO: 
 * - Weight and isLooping as properties in respective Prefaby (Player, Asteroid)
 * - The Spawn Handler should get access to these properties, to store them in a list and check for looping
 * 
 */


public class SpawnHandlerBehavior : MonoBehaviour
{
    List<GameObject> currentlyActiveList;
    //List<ItemCatalogue> currentlyActiveList;  //NOTE: Currently Not useful because the Inspector has to be customized first

    #region Variables

    [Header("The Gameobjects, the Spawn Handler has to handle")]
    [Tooltip("The Reference to the Player Character")]
    public GameObject playerCharacter;
    
    [Tooltip("Array of Game Object prefabs")]
    public GameObject[] entityReferences;

    //[Header("Parameters for gameplay weight of Assets")]
    //[Tooltip("The weight of the asteroid asset")]
    //public float weightAsteroid;
    //[Tooltip("The current total weight of all assets that are hostile against the player")]
    //float currentWeight;
    //[Space(10)]
    //[Tooltip("The maximum weight of all assets that are hostile against the player")]
    //public float maximumWeight;


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
        //catalogue = new List<ItemCatalogue>();

        currentlyActiveList = new List<GameObject>();

        //print("New List Count:    " + currentlyActiveList.Count);

        //Spawn an instance of the Player Character and add it to the List
        Vector2 spawnCoords = new Vector2(0, 0);
        GameObject tmp = Instantiate(playerCharacter, spawnCoords, transform.rotation);
        currentlyActiveList.Add(tmp);


        //Reset timer for next spawn
        nextSpawnTime = Time.time + spawnCooldown;

        //Debug Log
        //print("New List Count:    " + currentlyActiveList.Count);
    }

    /**
     * Call functions to handle spawning: 
     * 1. check if objects can be spawned depending on current weight and maximum Weight
     * 2. if objects can be spawned: choose an object (randomly), that doesn't exceed the maximum Weight
     * 3. spawn this asset
     */
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnCooldown;

            spawnAsteroid();
        }

        checkBoundaries();

    }
    /**
     * Spawns asteroids on a random border
     * 
     */



    /**
     * Checks every Entity that steps over the boundaries and changes it's position to the opposite or destroys them
     */
    void checkBoundaries()
    {
        //Boundaries X +/- Xmax
        //Boundaries Y +/- Ymax

        foreach (var item in currentlyActiveList)
        {
            GameObject itemGO = item.gameObject;

            Vector3 tmp = itemGO.transform.position;

            if (itemGO.transform.position.x > Xmax)
            {
                //Change it's position to be inside of the left border of the field
                tmp.x = -Xmax + 1;   //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
                itemGO.transform.position = tmp;
            }

            //If spaceship has passed the left border...
            if (itemGO.transform.position.x < -Xmax)
            {
                //Change it's position to be inside of the left border of the field
                tmp.x = Xmax - 1;   //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
                itemGO.transform.position = tmp;
            }

            //If spaceship hahs passed top border...
            if (itemGO.transform.position.y > Ymax)
            {
                //Change it's position to be inside of the left border of the field
                tmp.y = -Ymax + 1;    //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
                itemGO.transform.position = tmp;
            }

            //If spaceship hahs passed bottom border...
            if (itemGO.transform.position.y < -Ymax)
            {
                //Change it's position to be inside of the left border of the field
                tmp.y = Ymax - 1;    //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
                itemGO.transform.position = tmp;
            }

            //print(item);
        }
    }


    void spawnAsteroid()
    {
        float axis = Random.Range(1, 5);
        Vector3 spawnCoords = new Vector2();

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
        //print("SpawnCoords: " + spawnCoords);

        //Spawns new Astreroid on respective axis


        GameObject newAsteroid = Instantiate(entityReferences[0], spawnCoords, transform.rotation);
        currentlyActiveList.Add(newAsteroid);
        //print("New List Count:    " + currentlyActiveList.Count);

        newAsteroid.gameObject.SendMessage("setXmax", Xmax);
        newAsteroid.gameObject.SendMessage("setYmax", Ymax);
        newAsteroid.gameObject.SendMessage("setSize", Random.Range(1, 4));
    }


    /**
     * 
     */
    public void destroyEntity(GameObject target)
    {
        Destroy(target.GetComponent<SpriteRenderer>());
        Destroy(target);
    }


    ///**
    // * Spawns the asset and increases the current weight
    // */
    //void spawnEntity(GameObject asset)
    //{
    //    print("Name of Game Object:    " + asset.name);
        
    //    //Check for the name and increase the current weight, dependinng on the next asset
    //    switch (asset.name)
    //    {
    //        case "Asteroid Large":
    //            currentWeight += weightAsteroid;
    //            break;
    //        default:
    //            break;
    //    }

    //    print("Current Weight on create:    " + currentWeight);


    //    //Instantiate the asset
    //    //TOTO: position and rotation may be adjusted and randomized to improve gameplay
    //    Instantiate(asset, transform.position, transform.rotation);
    //}

    ///**
    // * This is called from hostile assets like Asteroids and Items, when they are destroyed.
    // * The total sum of weight is decreased by the weight value of the destroyed asset
    // */
    //public void decreaseOverallWeight(string temp)
    //{
    //    print("Destroy name:    " + temp);
        
    //    switch (temp)
    //    {
    //        case "Asteroid Large":
    //            currentWeight -= weightAsteroid;
    //            break;
    //        default:
    //            break;
    //    }

    //    print("Current Weight on destroy:    " + currentWeight);

    //}

}
