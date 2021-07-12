using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCharacterBehhavior : MonoBehaviour
{
    [Header("Stats")]
    public float rotationSpeed = 100;
    public float accelerationRate = 10;
    public float torqueSpeed = 1;

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
            //Turning the Spaceship: Variant 3: Using torque
            case 3:
                turnSpaceShipV3();
                break;
            //DEFAULT SETTINGS: Turning the Spaceship: Variant 1: Rotation in direction
            default:
                turnSpaceShipV1();
                break;
        }
    }

    /**
     * Accelerates the Spaceship with an impulse forward
     */
    void OnSpaceShipAcceleration()
    {
        rb.AddForce(transform.up * accelerationRate);
        print("Accelerate");
    }


    #region Different actions to switch between the movement variants
    void OnSwitchToMovement1()
    {
        turnVariant = 1;
        Debug.Log("Turning the Spaceship: Variant 1: Rotation in direction by dragging LS or Pressing Q/E");
        
    }

    void OnSwitchToMovement2()
    {
        turnVariant = 2;
        Debug.Log("Turning the Spaceship: Variant 2: Tandem with Joystick");
    }

    void OnSwitchToMovement3()
    {
        turnVariant = 3;
        Debug.Log("Turning the Spaceship: Variant 3: Using torque");
    }
    #endregion




    //void OnTurnSpaceShipKeyboard(InputValue value)
    //{
    //    print("Turn Ship");


    //    float temp = value.Get<float>();

    //    //Rotate leftwards
    //    if (temp < 0) 
    //    {
    //        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    //        print("Turn left");
    //    }
    //    //Rotate rightwards
    //    else if (temp > 0)
    //    {
    //        transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
    //        print("Turn right");
    //    }

    //}



    /**
     * Turning the Spaceship leftwards. The spaceship turns over time in the desired direction
     */
    void OnLeftTurnSpaceShipKeyboard()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        print("Turn left");
    }
    /**
     * Turning the Spaceship rightwards. The spaceship turns over time in the desired direction
     */
    void OnRightTurnSpaceShipKeyboard()
    {
        transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        print("Turn right");
    }





    /**
     * Turning the Spaceship by either using the Joystick in horizontal or by Pressing "Q" or "E". The spaceship turns over time in the desired direction
     */
    void turnSpaceShipV1()
    {


        //print(Input.GetAxis("Horizontal"));

        //Turn the Spaceship to the right
        //if (Input.GetKey(KeyCode.Q) || (Input.GetAxis("Horizontal") < -0.5f))
        //{
        //    //Rotates the Spaceship to the right
        //    transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        //    //print("Turn left");
        //}

        ////Turn the Spaceship to the left
        //if (Input.GetKey(KeyCode.E) || (Input.GetAxis("Horizontal") > 0.5f))
        //{
        //    //Rotates the Spaceship to the left
        //    transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        //    //print("Turn right");
        //}
    }

    /**
     * Turning the Spaceship by using the Joystick. The Spaceship faces towards the joystick position
     */
    void turnSpaceShipV2()
    {
        //Check if Joystick Drag is out of the Deadzone. Therefore a Vector2 Variable is created and its magnitute is compared to the specified inputDeadZone variable 
        //leftStickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                
        Vector2 movementDirection = new Vector2();
        
        //Input has to be out of the deadzone
        movementDirection = checkDeadzone(leftStickInput);
        
        //movementDirection.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720 * Time.deltaTime);

        //calculates the rotation
        //float angle = Mathf.Atan2(verticalInput, horizontalInput) * Mathf.Rad2Deg;

        //print("Angle: " + angle);

        //Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //transform.rotation = rotation;
    }

    /**
     * Turning the spaceship using a torque. The torque is applied as an impulse which means, the spaceship will initially rotate fast and then slow down due to a friction
     */
    void turnSpaceShipV3()
    {
        //Checking Input with Deadzone checking in mind
        //When Joystick is aligned to another direction as PC's rotation an static angualr force will be added
        //If Angle between Joystick and PC rotation is over 10°, no friction is there which enables fast rotation
        //If the angle is between 10° and 0° the frictiopn will increase exponentially to ensure a slow down
        //If the angle is 0° the friction is infinite (set to maximum)






    }

    /**
     * Checks if the Input of the Gamepad's analoguous sticks is in the deadzone. 
     * Is the input out of the deadzone, the coordinates will be returned.
     */
    Vector2 checkDeadzone(Vector2 input)
    {
        Vector2 validInput = new Vector2(0, 0);

        //When out of Deadzone, input may be checked
        if (input.magnitude > inputDeadZone)
        {
            //validInput = new Vector2(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));
        }

        return validInput;
    }

}