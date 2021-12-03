using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxBehavior : MonoBehaviour
{
    [Tooltip("The outmost skybox (not the background Skybox)")]
    [SerializeField]
    GameObject skybox01;
    [Tooltip("The speed and direction of which the skybox should rotate")]
    [SerializeField]
    Vector3 rotationSpeedSkybox01;

    [Tooltip("The skybox 1 more inside than outmost Skybox")]
    [SerializeField]
    GameObject skybox02;
    [Tooltip("The speed and direction of which the skybox should rotate")]
    [SerializeField]
    Vector3 rotationSpeedSkybox02;



    // Start is called before the first frame update
    void Start()
    {
//        rotationSpeedSkybox01.x *= 
    }

    // Update is called once per frame
    void Update()
    {
        skybox01.transform.Rotate(  rotationSpeedSkybox01.x * Time.deltaTime, 
                                    rotationSpeedSkybox01.y * Time.deltaTime, 
                                    rotationSpeedSkybox01.z * Time.deltaTime);

        skybox02.transform.Rotate(  rotationSpeedSkybox02.x * Time.deltaTime,
                                    rotationSpeedSkybox02.y * Time.deltaTime,
                                    rotationSpeedSkybox02.z * Time.deltaTime);
    }
}
