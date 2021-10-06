using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandlerBehavior : MonoBehaviour
{
    #region Variables
    [Tooltip("The Gameobjects, the Spawn Handler has to handle")]
    public GameObject[] spawnableAssets;
    


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < spawnableAssets.Length; i++)
        {
            //When necessary, calculate next time for spawning a specific asset
            calculateNextSpawnTime(spawnableAssets[i]);
            //Check if time has come to spawn a specific asset
            checkNextSpawnTime(spawnableAssets[i]);
            //finally spawns this specific asset
            spawnEntity(spawnableAssets[i]);
        }

    }


    void calculateNextSpawnTime(GameObject asset)
    {

        //TODO Get access to minTime, maxTime, nnextSpawnTime, isWaiting, and Weight of scripts of other game objects
        //WITHOUT using the name of the scripts. That's because each script of every new asset will be named different.


        //ScriptableObject temp = asset.GetComponent<ScriptableObject>();

        //if (!asset.isQueuedForSpawn)
        //{
        //    print("BOOOOOOOOL:    " + asset.isQueuedForSpawn);
        //}

    }

    void checkNextSpawnTime(GameObject asset)
    {


    }

    void spawnEntity(GameObject asset)
    {


    }


}
