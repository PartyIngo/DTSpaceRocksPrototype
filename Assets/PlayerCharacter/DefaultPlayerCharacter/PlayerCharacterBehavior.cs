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
    [Tooltip("Extra torque for wide turns.")]
    public float maxFastTurnTorque;
    [Tooltip("the default Value for angular drag")]
    public float defaultAngularDrag;
    [Tooltip("Multiplier for angular drag which slows down the rotation")]
    public float maxVariableAngularDrag;
    float variableAngularDrag;
    [Tooltip("angular difference between Spaceship and LTS. Is the Spaceship within this angle, the friction will be increased")]
    public float brakeAngle;

    [Header("Strafe Stats")]
    [Tooltip("The speed in which the spaceship should move leftwards/rightwards by strafing")]
    public float strafeSpeed;
    float strafingLeftValue;
    float strafingRightValue;
    float strafingValue;
    bool isStrafingLeft = false;
    bool isStrafingRight = false;

    [Header("Gamepad Deadzone Setup")]
    [Tooltip("The Deadzone of the left thumbstick")]
    public float deadZoneRadiusLTS;
    Vector2 leftStickInput;
    [Tooltip("The Deadzone of both the LT and RT Triggers")]
    public float deadZoneLTRT;


    float normalizedAngleDifference;
    Rigidbody2D rb;

    
    /**
     * prepare some values by starting this script
     */
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.drag = defaultFriction;
    }

    void FixedUpdate()
    {
        //Check if the spaceship has to be accelerated forward
        HandleAcceleration();
        
        //Check if the Spaceship has to turn
        HandleTurning();

        //Check if the spaceship has to strafe
        HandleStrafing();
    }

    /**
     * Sets the spaceship' s speed when requirements are given
     */
    void HandleAcceleration()
    {
        //When the current Value of the Thumbstick is out of the deadzone...
        if (leftStickInput.magnitude > deadZoneRadiusLTS)
        {
            //print("Magnitude: " + leftStickInput.normalized.magnitude + " On Coordinates " + leftStickInput);
            //print("On Coordinates " + leftStickInput);
            //When dragging the Left Thumbstick a little bit, the acceleration is normal
            if (leftStickInput.magnitude < boostZone)
            {
                currentAcceleration = (leftStickInput.normalized.magnitude * accelerationValue);
                //print("NORMAL Speed: " + currentAcceleration);
            }

            //When dragging the Left Thumbstick to it's limit, a boost has to be applied, to increase the acceleration to a maximum
            if (leftStickInput.magnitude >= boostZone)
            {
                currentAcceleration = (leftStickInput.normalized.magnitude * accelerationValue * accelerationBoostMultiplier);
                print("BOOST!");
            }
            //Adds the force to the gameobject to move it. Acceleration feels better, when rotating the object to 180°
            rb.AddForce((transform.up * currentAcceleration) - ((transform.up * currentAcceleration) * Mathf.Abs(normalizedAngleDifference) / 180));
        }
    }
    

    /**
     * Here is described how the spaceship has to turn in the desired directions
     */
    void HandleTurning()
    {   
        //When out of Deadzone, the Spaceship is able to turn around
        if (leftStickInput.magnitude > deadZoneRadiusLTS)
        {
            //calculate y and x values of leftStickInput to get a float value, that it can be better compared to values of the spaceship's rotation values
            float angleLTS = Mathf.Atan2(leftStickInput.y, leftStickInput.x) * Mathf.Rad2Deg;
     
            //Conversion by adding -90 on Ship's rotation
            float convShipRotation = transform.rotation.eulerAngles.z + 90;
            
            //subtracts 360 from ship's roration when its current rotation is greater than 180, to simplify further calculations
            if (convShipRotation > 180f)
            {
                convShipRotation -= 360f;
            }

            //stores the difference between the desired rotation (described in the angleLTS parameter) and the ship's current rotation for further usage
            normalizedAngleDifference = angleLTS - convShipRotation;

            //subtracts 360 to normalizedAngleDifference when its current value is greater than 180. That's because the rotation value can only be between -180 and 180
            if (normalizedAngleDifference > 180)
            {
                normalizedAngleDifference -= 360;
            }
            //adds 360 to normalizedAngleDifference when its current value is less than -180. That's because the rotation value can only be between -180 and 180
            if (normalizedAngleDifference < -180)
            {
                normalizedAngleDifference += 360;
            }

            float angleDifference = normalizedAngleDifference;

            //Debug info
            //print("SS:    " + convShipRotation + "  LTS:   " + angleLTS + "  DIFF:    " + angleDifference);

            //when Spaceships current rotation is greater than the brakeAngle, reduce the friction to increase the torque speed
            if (Mathf.Abs(angleDifference) > brakeAngle)
            {
                variableAngularDrag = 0;
            }
            //Otherwise, when the angleDifference is less thatn the brakeAngle's value, increase the angular drag exponentially the smaller the angleDifference becomes
            else
            {
                variableAngularDrag = maxVariableAngularDrag - (maxVariableAngularDrag * Mathf.Abs(angleDifference)/ brakeAngle);
            }

            //set the angular Drag Property of the Rigidbody Component
            rb.angularDrag = defaultAngularDrag + Mathf.Abs(variableAngularDrag);


            //rotate the spaceship leftwards
            if (Mathf.Sign(normalizedAngleDifference) > 0)
            {
                //Debug info
                //print("LEFT");
                torque = Mathf.Abs(torque);
                maxFastTurnTorque = Mathf.Abs(maxFastTurnTorque);
            }
            else
            {
                //Debug info
                //print("RIGHT");
                //If torque is positive, switch it's sign ONCE
                if (Mathf.Sign(torque) > 0)
                {
                    torque = -torque;
                    maxFastTurnTorque = -maxFastTurnTorque;
                }
            }

            
            float extraTorque = maxFastTurnTorque * (Mathf.Abs(normalizedAngleDifference) / 180);
            float scaledTotalTorque = (torque + extraTorque) * leftStickInput.magnitude;
            //Finally add torque to rotate the Spaceship
            rb.AddTorque(scaledTotalTorque, ForceMode2D.Force);
            
            //Debug info
            //print(scaledTotalTorque);
        }
    }

    /**
     * Handles strafing by checking LT and RT with XOR. Only one of them can be pressed at the same time. The other ine has to be released.
     * If this condition occurs, a strafe in the respecrive direction may be applied. 
     * If both of them are (not) pressed, the XOR Statement will return false and the strafe will be blocked immediately 
     */
    void HandleStrafing()
    {
        //XOR Check: Only one of LT or RT can have a value to apply lateral force/strafe
        if (isStrafingLeft ^ isStrafingRight)
        {
            //Debug info
            //print("Strafe enabled");

            //If ship strafes leftwards set stafingValue to corresponding value. Make sure, the value is inverted to simplify further calculations.
            if (isStrafingLeft)
            {
                strafingValue = -strafingLeftValue;
            }

            // If ship strafes rightwards set stafingValue to corresponding value.
            if (isStrafingRight)
            {
                strafingValue = strafingRightValue;
            }
        }

        //When XOR will return false (because both of LT/RT are pressed or both of them are not pressed) no strafe will be applied or rather the strafe will be resetted
        else
        {
            //Debug info
            //print("strafe blocked");
            strafingValue = 0;
        }

        print("Strafing Value * Speed: " + strafingValue * strafeSpeed);
        //Apply the force sideways, to make the Spaceship strafe
        rb.AddForce(transform.right * strafingValue * strafeSpeed);
    }

    #region Input System Controls

    /**
     * When pressing the left Thumbstick, a multiplier has to be applied to the friction to slow down the spaceship.
     */
    public void OnBrake(InputAction.CallbackContext context)
    {
        //On LTS pressed, the drag is increased to slow down the spaceship
        if (context.started)
        {
            rb.drag = defaultFriction * brakeDragMultiplier;
            //print("Pressed");
        }

        //On LTS released, the friction is set to it's default value
        if (context.canceled)
        {
            rb.drag = defaultFriction;
            //print("Released");
        }
    }

    /**
     * Called, then the left Thumbstick is dragged
     * The input value will be stored for further usage
     */
    public void OnLeftThumbstickInput(InputAction.CallbackContext context)
    {
        leftStickInput = context.ReadValue<Vector2>();
        //print("LTS Input: " + leftStickInput.magnitude);
    }

    /**
     * Called, then the left Trigger (LT) is dragged
     * When LT is pressed, two things occur: 
     * - its value will be stored for further usage
     * - it's flagged, that the ship should strafe leftwards.
     * If the trigger'S value is within the specified deadzone, the flag is set to false and it's stored value will become "0"
     */
    public void OnStrafeLeft(InputAction.CallbackContext context)
    {
        //Store LT input in temporary parameter
        float temp = context.ReadValue<float>();

        //Apply the value of LT Input to strafingLeftValue parameter when out of Deadzone
        if (temp > deadZoneLTRT)
        {
            strafingLeftValue = temp;
            isStrafingLeft = true;
        }
        else
        {
            isStrafingLeft = false;
        }
    }

    /**
     * Called, then the left Trigger (RT) is dragged
     * When LT is pressed, two things occur: 
     * - its value will be stored for further usage
     * - it's flagged, that the ship should strafe leftwards.
     * If the trigger'S value is within the specified deadzone, the flag is set to false and it's stored value will become "0"
     */
    public void OnStrafeRight(InputAction.CallbackContext context)
    {
        //Store RT input in temporary parameter
        float temp = context.ReadValue<float>();

        //Apply the value of RT Input to strafingRightValue parameter when out of Deadzone
        if (temp > deadZoneLTRT)
        {
            strafingRightValue = temp;
            isStrafingRight = true;
        }
        else
        {
            isStrafingRight = false;
        }
    }
    #endregion
}