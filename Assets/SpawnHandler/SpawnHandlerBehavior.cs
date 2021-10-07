using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandlerBehavior : MonoBehaviour
{
    #region Variables
    [Tooltip("The Gameobjects, the Spawn Handler has to handle")]
    public GameObject[] spawnableAssets;
    
    //TODO: For elegance, this values may be stored in the respective asset-scripts and commited to the spawner handler in methods for spawning and destroying
    
    
    
    [Header("Parameters for gameplay weight of Assets")]
    [Tooltip("The weight of the asteroid asset")]
    public float weightAsteroid;
    [Tooltip("The current total weight of all assets that are hostile against the player")]
    float currentWeight;
    [Space(10)]
    [Tooltip("The maximum weight of all assets that are hostile against the player")]
    public float maximumWeight;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

    /**
     * Call functions to handle spawning: 
     * 1. check if objects can be spawned depending on current weight and maximum Weight
     * 2. if objects can be spawned: choose an object (randomly), that doesn't exceed the maximum Weight
     * 3. spawn this asset
     */
    void Update()
    {
        //Check if objects can be spawned, depending on current weight and maximum weight
        if (currentWeight < maximumWeight)
        {
            //Choose object that will be spawned
            chooseAsset();  
        }


        //for (int i = 0; i < spawnableAssets.Length; i++)
        //{
        //    //When necessary, calculate next time for spawning a specific asset
        //    calculateNextSpawnTime(spawnableAssets[i]);
        //    //Check if time has come to spawn a specific asset
        //    checkNextSpawnTime(spawnableAssets[i]);
        //    //finally spawns this specific asset
        //    spawnEntity(spawnableAssets[i]);
        //}

    }

    /**
     * Choose an Object that will be spawned.
     * When a potential asset is found, spawn it
     */
    void chooseAsset()
    {
        //TODO: Choose objects, when more assets are implemented

        //Finally spawn this entity
        spawnEntity(spawnableAssets[0]); 
    }

    /**
     * Spawns the asset and increases the current weight
     */
    void spawnEntity(GameObject asset)
    {
        print("Name of Game Object:    " + asset.name);
        
        //Check for the name and increase the current weight, dependinng on the next asset
        switch (asset.name)
        {
            case "Asteroid Large":
                currentWeight += weightAsteroid;
                break;
            default:
                break;
        }

        print("Current Weight on create:    " + currentWeight);


        //Instantiate the asset
        //TOTO: position and rotation may be adjusted and randomized to improve gameplay
        Instantiate(asset, transform.position, transform.rotation);
    }

    /**
     * This is called from hostile assets like Asteroids and Items, when they are destroyed.
     * The total sum of weight is decreased by the weight value of the destroyed asset
     */
    public void decreaseOverallWeight(string temp)
    {
        print("DEstroy nname:    " + temp);
        
        switch (temp)
        {
            case "Asteroid Large":
                currentWeight -= weightAsteroid;
                break;
            default:
                break;
        }

        print("Current Weight on destroy:    " + currentWeight);

    }

}
