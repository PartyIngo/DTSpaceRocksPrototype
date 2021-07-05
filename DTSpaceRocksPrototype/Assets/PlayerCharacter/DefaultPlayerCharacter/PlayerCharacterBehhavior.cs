using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterBehhavior : MonoBehaviour
{
    [Header("Stats")]
    public float turnSpeed = 20;
    public float accelerationRate = 10;

    [Header("Physics")]
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        #region Turning the Spaceship

        //TODO: Orienting the Spaceship towards the Stick. 
        print(Input.GetAxis("Horizontal"));

        //Turn the Spaceship to the right
        if (Input.GetKey(KeyCode.Q) || (Input.GetAxis("Horizontal") < -0.5f))
        {
            //Rotates the Spaceship to the right
            transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
            print("Turn left");
        }

        //Turn the Spaceship to the left
        if (Input.GetKey(KeyCode.E) || (Input.GetAxis("Horizontal") > 0.5f))
        {
            //Rotates the Spaceship to the left
            transform.Rotate(Vector3.back * turnSpeed * Time.deltaTime);
            print("Turn right");
        }
        #endregion


        //accelerates the Spaceship forwards. 
        //TODO: Use impulse and check if it feels better
        if (Input.GetKey(KeyCode.Space) || Input.GetButton("Acceleration"))
        {
            rb.AddForce(transform.up * accelerationRate);
            print("Accelerate");
        }
    }
}