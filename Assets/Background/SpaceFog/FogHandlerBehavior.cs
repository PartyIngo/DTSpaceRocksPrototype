using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogHandlerBehavior : MonoBehaviour
{
    [Tooltip("The gameObject for the Fog")]
    [SerializeField]
    GameObject fogPlane;

    [Tooltip("The position in the distance, where the fog should spawn")]
    [SerializeField]
    Vector3 fogSpawnPoint;

    [Tooltip("The distance between the individual fog layers")]
    [SerializeField]
    float fogDistance;

    [Tooltip("The speed of the zoom effect")]
    [SerializeField]
    float zoomSpeed;

    [Tooltip("The speed in which the fog should rotate")]
    [SerializeField]
    Vector3 rotationSpeedFog;

    //The array of fog planes
    GameObject[] fog = new GameObject[3];
    


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < fog.Length; i++)
        {
            fog[i] = Instantiate(fogPlane, fogSpawnPoint, transform.rotation);
            fogSpawnPoint.z += fogDistance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        zoomFog();
        rotateFog();

    }


    /**
     * Moves the Fog towards the camera, to get a zoom illusion
     * Once the Fog is out of Field of View, it' position will be resetted
     * 
     * TODO: distance between layers has to  be increased
     * 
     * 
     */
    void zoomFog()
    {
        for (int i = 0; i < fog.Length; i++)
        {
            fog[i].transform.position += Vector3.back * Time.deltaTime * zoomSpeed;

            if (fog[i].transform.position.z <= -10)
            {
                fog[i].transform.position = fogSpawnPoint;
                fog[i].GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            }
        }
    }


    /**
     * Rotates the Fog in a specific direction with a specific speed
     */
    void rotateFog()
    {
        for (int i = 0; i < fog.Length; i++)
        {
            fog[i].transform.Rotate(    rotationSpeedFog.x * Time.deltaTime,
                                        rotationSpeedFog.y * Time.deltaTime,
                                        rotationSpeedFog.z * Time.deltaTime);
        }
    }
}
