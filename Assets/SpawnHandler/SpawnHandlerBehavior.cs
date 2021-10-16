using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandlerBehavior : MonoBehaviour
{
    #region Variables

    [Header("The Gameobjects, the Spawn Handler has to handle")]
    [Tooltip("List of Game Objects")]
    public GameObject[] spawnableAssets;

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
        nextSpawnTime = Time.time + spawnCooldown;
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
    }
    /**
     * Spawns asteroids on a random border
     * 
     */
    void spawnAsteroid()
    {
        byte axis = (byte)Random.Range(1, 4);
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
        print("SpawnCoords: " + spawnCoords);

        //Spawns new Astreroid on respective axis
        GameObject newAsteroid = Instantiate(spawnableAssets[0], spawnCoords, transform.rotation);
        newAsteroid.gameObject.SendMessage("setXmax", Xmax);
        newAsteroid.gameObject.SendMessage("setYmax", Ymax);
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
    //    print("DEstroy nname:    " + temp);
        
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
