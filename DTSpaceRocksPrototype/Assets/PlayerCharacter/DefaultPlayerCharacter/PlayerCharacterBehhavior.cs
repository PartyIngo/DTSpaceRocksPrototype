using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterBehhavior : MonoBehaviour
{
    [Header("Stats")]
    public float rotationSpeed = 100;
    public float accelerationRate = 10;

    [Header("Physics")]
    public Rigidbody2D rb;

    [Header("Gamepad Deadzone Setup")]
    public float inputDeadZone = 0.5f;
    Vector2 leftStickInput;

    private float horizontalInput = 0;
    private float verticalInput = 0;

    private byte turnVariant = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            turnVariant = 1;
            Debug.Log("Turning the Spaceship: Variant 1: Rotation in direction by dragging LS or Pressing Q/E");
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            turnVariant = 2;
            Debug.Log("Turning the Spaceship: Variant 2: Tandem with Joystick");
        }

        switch (turnVariant)
        {
            //Turning the Spaceship: Variant 1: Rotation in direction
            case 1:
                turnSpaceShipV1();
                break;
            //Turning the Spaceship: Variant 2: Tandem with Joystick
            case 2:
                turnSpaceShipV2();
                break;
        }

        //accelerates the Spaceship forwards. 
        if (Input.GetKey(KeyCode.Space) || Input.GetButton("Acceleration"))
        {
            rb.AddForce(transform.up * accelerationRate);
            //print("Accelerate");
        }
    }

    /**
     * Turning the Spaceship by either using the Joystick in horizontal or by Pressing "Q" or "E". The spaceship turns over time in the desired direction
     */
    void turnSpaceShipV1()
    {
        //print(Input.GetAxis("Horizontal"));

        //Turn the Spaceship to the right
        if (Input.GetKey(KeyCode.Q) || (Input.GetAxis("Horizontal") < -0.5f))
        {
            //Rotates the Spaceship to the right
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
            //print("Turn left");
        }

        //Turn the Spaceship to the left
        if (Input.GetKey(KeyCode.E) || (Input.GetAxis("Horizontal") > 0.5f))
        {
            //Rotates the Spaceship to the left
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
            //print("Turn right");
        }
    }

    /**
     * Turning the Spaceship by using the Joystick. The Spaceship faces towards the joystick position
     */
    void turnSpaceShipV2()
    {
        //print("Horizontal: " + Input.GetAxis("Horizontal") + " Vertical: " + Input.GetAxis("Vertical"));

        //Check if Joystick Drag is out of the Deadzone. Therefore a Vector2 Variable is created and its magnitute is compared to the specified inputDeadZone variable 
        leftStickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //When out of Deadzone, input may be checked
        if (leftStickInput.magnitude > inputDeadZone)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = -Input.GetAxis("Vertical");
            //Rotates the Spaceship towards joystick direction
        }

        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        movementDirection.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720 * Time.deltaTime);

        //calculates the rotation
        //float angle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;

        //print("Angle: " + angle);

        //Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //transform.rotation = rotation;
    }
}