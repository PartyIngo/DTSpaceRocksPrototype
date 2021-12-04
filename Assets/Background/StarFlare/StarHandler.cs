using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarHandler : MonoBehaviour
{
    [SerializeField]
    GameObject star;

    float nextSpawnTime;
    float spawnDelay;
    Vector3 spawnCoords;


    // Start is called before the first frame update
    void Start()
    {
        spawnDelay = Random.Range(0, 4);
        nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            spawnCoords.x = Random.Range(-12, 12);     //12.4
            spawnCoords.y = Random.Range(-7, 7);     //7
            spawnCoords.z = 0;

            Instantiate(star, spawnCoords, transform.rotation);

            spawnDelay = Random.Range(2, 4);
            nextSpawnTime = Time.time + spawnDelay;
        }   
    }
}
