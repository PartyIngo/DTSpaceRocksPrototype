using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRepresentation : MonoBehaviour
{
    public GameObject livesIndicator;
    public GameObject playerCharacter;
    private Transform healthTransform;
    private int healthAmount;



    // Start is called before the first frame update
    void Start()
    {
        //healthTransform = gameObject.transform;

        //healthAmount = playerCharacter.GetComponent<PlayerCharacterMovement>().lives;

        //Debug.Log("healthAmount: " + healthAmount);
        //Debug.Log("Pos: " + healthTransform.position);

        //for (int i = healthAmount; i > 0; i--)
        //{
        //    Vector3 tmp = transform.position;
        //    tmp.x += (i * 20);
        //    healthTransform.position = tmp;
        //    Debug.Log("HealthTransform: " + healthTransform.position);


        //    Instantiate(livesIndicator, healthTransform);









        //    //float offset = i * 20;

        //    //Vector3 tmp = healthTransform.position;
        //    //tmp.x += offset;

        //    //healthTransform.position = tmp;


        //    //Debug.Log("Iteration: " + i);
        //    //Debug.Log("Offset: " + offset);
        //    //Debug.Log("TMP: " + tmp);
        //    //Debug.Log("Pos: " + healthTransform.position);



        //    //var newLiveIndicator = Instantiate(livesIndicator, healthTransform);
        //    //newLiveIndicator.transform.SetParent(gameObject.transform);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        



    }
}
