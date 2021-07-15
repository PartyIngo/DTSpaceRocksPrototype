using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterBehavior : MonoBehaviour
{
    [Header("Acceleration Stats")]
    [Tooltip("The inner boundaries for boost. If the Thumbstick's value is greater, a boost will be applied.")]
    public float boostZone;
    [Tooltip("How fast has the acceleration to be")]
    public float accelerationValue;
    [Tooltip("How large has the boost multiplier to be, when dragging the Thumbstick to it's limit")]
    public float accelerationBoostMultiplier;
    float currentAcceleration;

    [Header("Brake Stats")]
    [Tooltip("How strong should the brake be? This value is multiplied with the next Parameter called 'defaultFriction'")]
    public float brakeDragMultiplier;
    [Tooltip("The default value for the friction of the environment. This value is assigned to the Parameter 'Linear Drag' Value of the Rigidbody Component")]
    public float defaultFriction;

    [Header("Turning Stats")]
    [Tooltip("How fast should the spaceship rotate?")]
    public float torque;
    [Tooltip("the default Value for angular drag")]
    public float defaultAngularDrag;
    [Tooltip("Multiplier for angular drag which slows down the rotation")]
    public float maxVariableAngularDrag;
    float variableAngularDrag;
    [Tooltip("angular difference between Spaceship and LTS. Is the Spaceship within this angle, the friction will be increased")]
    public float brakeAngle;
    float lastTargetRotation;

    [Header("Gamepad Deadzone Setup")]
    public float deadZoneRadius;
    Vector2 leftStickInput;

    Rigidbody2D rb;

    
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.drag = defaultFriction;
    }

    void FixedUpdate()
    {
        //Check if the spaceship has to be accelerated forward
        handleAcceleration();
        
        //Check if the Spaceship has to turn
        handleTurning();
    }

    /**
     * Sets the spaceship' s speed when requirements are given
     */
    void handleAcceleration()
    {
        //When the current Value of the Thumbstick is out of the deadzone...
        if (leftStickInput.magnitude > deadZoneRadius)
        {
            //print("Magnitude: " + leftStickInput.normalized.magnitude + " On Coordinates " + leftStickInput);
            //print("On Coordinates " + leftStickInput);
            //When dragging the Left Thumbstick a little bit, the acceleration is normal
            if (leftStickInput.magnitude < boostZone)
            {
                currentAcceleration = (leftStickInput.normalized.magnitude * accelerationValue);
                rb.AddForce(transform.up * currentAcceleration);
                print("NORMAL Speed: " + currentAcceleration);
            }

            //When dragging the Left Thumbstick to it's limit, a boost has to be applied, to increase the acceleration to a maximum
            if (leftStickInput.magnitude >= boostZone)
            {
                currentAcceleration = (leftStickInput.normalized.magnitude * accelerationValue * accelerationBoostMultiplier);
                rb.AddForce(transform.up * currentAcceleration);
                print("BOOST: " + currentAcceleration);
            }
        }
    }

    /**
     * When pressing the left Thumbstick, a multiplier has to be applied to the friction to slow down the spaceship.
     */
    public void OnBrake(InputAction.CallbackContext context)
    {
        //On LTS pressed, the drag is increased to slow down the spaceship
        if (context.started)
        {
            rb.drag = defaultFriction * brakeDragMultiplier;
            print("Pressed");
        }

        //On LTS released, the friction is set to it's default value
        if (context.canceled)
        {
            rb.drag = defaultFriction;
            print("Released");
        }
    }

    /**
     * Here is described how the spaceship has to turn in the desired directions
     */
    void handleTurning()
    {
        //When out of Deadzone, the Spaceship is able to turn around
        if (leftStickInput.magnitude > deadZoneRadius)
        {
            //print("LeftStick Input " + leftStickInput);

            //Calculates LTS input into float value for angle
            float angleLTS = Mathf.Atan2(leftStickInput.y, leftStickInput.x) * Mathf.Rad2Deg;
            lastTargetRotation = angleLTS;

            //Converts LTS Input into desired Spaceship Rotation. The Whole Coordinate System of LTS is rotated by 90° CCW, so North has the Value of 90 while Unitys System hast a Value of 0 in northern Direction
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angleLTS - 90));

            
            //transform.rotation = targetRotation; //Check if conversation of LTS works. Comment Code below, so it won't affect this debug values

            //The only difference that can be calculates is from both z Axis values. The Spaceship as well as the calculated LTS input doen't have any other values than 0 on their X and Y axis
            float angleDifference = Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation.eulerAngles.z);  //If SS is on 358 and LTS is on 2, the difference will be huge (256 instead of 4)

            print("SS:    " + transform.rotation.eulerAngles.z + "    LTS:    " + targetRotation.eulerAngles.z + "    Diff   " + angleDifference);
            
            
            //If the spaceship is rotated in another direction than the LTS is currently dragged to, the Spaceship has to be rotated.
            if (transform.rotation.eulerAngles.z != targetRotation.eulerAngles.z)
            {
                //If the difference is too high, the rotation has to work fast. Therefore a Multiplier for the angularDrag is set to a minimum.
                if (angleDifference > brakeAngle)
                {
                    variableAngularDrag = 0;
                }

                //If the difference between the Spaceship's rotation and the LTS's Direction is small enaough, the multiplier is increased exponentially.
                if (angleDifference <= brakeAngle)
                {
                    variableAngularDrag = (Mathf.Pow(brakeAngle, 2) - Mathf.Pow(angleDifference, 2)) * maxVariableAngularDrag;
                }

                //set the angular Drag Property of the Rigidbody Component
                rb.angularDrag = defaultAngularDrag + variableAngularDrag;

                //
                if (angleDifference < 180)
                {
                    torque = Mathf.Abs(torque);
                }
                if (angleDifference >= 180)
                {
                    //If torque is positive, switch it's sign ONCE
                    if (Mathf.Sign(torque) > 0)
                    {
                        torque = -torque;
                    }
                }

                //Apply Torque to the spaceship to rotate it
                rb.AddTorque(torque, ForceMode2D.Force);
            }

            /**
             *      PSEUDO CODE. DO NOT DELETE!!!
             *      
             *      public float defaultAngularDrag;
             *      float variableAngularDrag;
             *      WHILE Thumbstick Position != SpaceshipRotation
             *          IF angle between SpaceshipRotation  and ThumbstickPosition greater than brakeAngle
             *              variableAngularDrag = 0;
             *              
             *          IF angle between SpaceshipRotation and ThumbstickPosition smallerEqual brakeAngle
             *              variableAngularDrag = (brakeAngle² - diff²) * maxVariableAngularDrag;
             *
             *      rb.angualarDrag = defaultAngularDrag + variableAngularDrag;
             *      Rotate Spaceship with AddTorque on Spaceship
             */
        }
    }

    /**
     * Called, then the left Thumbstick is dragged
     */
    public void OnLeftThumbstickInput(InputAction.CallbackContext context)
    {
        leftStickInput = context.ReadValue<Vector2>();
    }
}