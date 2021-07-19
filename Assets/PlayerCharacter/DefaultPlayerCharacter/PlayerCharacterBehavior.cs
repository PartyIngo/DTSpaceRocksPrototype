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

    [Header("Gamepad Deadzone Setup")]
    public float deadZoneRadius;
    Vector2 leftStickInput;

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
        handleAcceleration();
        
        //Check if the Spaceship has to turn
        handleTurning();
    }

    /**
     * Sets the spaceship' s speed when requirements are given
     */
    void handleAcceleration()
    {
        print(" ");
        //When the current Value of the Thumbstick is out of the deadzone...
        if (leftStickInput.magnitude > deadZoneRadius)
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
     * Here is described how the spaceship has to turn in the desired directions
     */
    void handleTurning()
    {   
        //When out of Deadzone, the Spaceship is able to turn around
        if (leftStickInput.magnitude > deadZoneRadius)
        {
            float angleLTS = Mathf.Atan2(leftStickInput.y, leftStickInput.x) * Mathf.Rad2Deg;
     
            //Conversion
            //Add -90 on Ship's rotation
            float convShipRotation = transform.rotation.eulerAngles.z + 90;
            //ship and LTS are equal and are not > 360
            if (convShipRotation > 180f)
            {
                convShipRotation -= 360f;
            }

            //Notmalize the difference between the ships rotation and the LTS input.
            //This has to be done because of the transition from the values 179.9 to 180.0.
            //Otherwise the ship will rotate in the wrong direction/through the greater angle
            normalizedAngleDifference = angleLTS - convShipRotation;

            if (normalizedAngleDifference > 180)
            {
                normalizedAngleDifference -= 360;
            }
            if (normalizedAngleDifference < -180)
            {
                normalizedAngleDifference += 360;
            }
            float angleDifference = normalizedAngleDifference;

            //Debug output
            //print("SS:    " + convShipRotation + "  LTS:   " + angleLTS + "  DIFF:    " + angleDifference);

            //when ship is rotated far from the LTS's position, reduce the friction miltiplier
            if (Mathf.Abs(angleDifference) > brakeAngle)
            {
                variableAngularDrag = 0;
            }
            //Otherwise, when the ship is rotated close to LTS's position, increase it more, the closer both values are
            else
            {
                variableAngularDrag = maxVariableAngularDrag - (maxVariableAngularDrag * Mathf.Abs(angleDifference)/ brakeAngle);
            }

            //set the angular Drag Property of the Rigidbody Component
            rb.angularDrag = defaultAngularDrag + Mathf.Abs(variableAngularDrag);


            //rotate leftwards
            if (Mathf.Sign(normalizedAngleDifference) > 0)
            {
                //print("LEFT");
                torque = Mathf.Abs(torque);
                maxFastTurnTorque = Mathf.Abs(maxFastTurnTorque);
            }
            else
            {
                //print("RIGHT");
                //If torque is positive, switch it's sign ONCE
                if (Mathf.Sign(torque) > 0)
                {
                    torque = -torque;
                    maxFastTurnTorque = -maxFastTurnTorque;
                }
            }

            //Finally add torque to rotate the Spaceship
            float extraTorque = maxFastTurnTorque * (Mathf.Abs(normalizedAngleDifference) / 180);
            float scaledTotalTorque = (torque + extraTorque) * leftStickInput.magnitude;
            rb.AddTorque(scaledTotalTorque, ForceMode2D.Force);
            print(scaledTotalTorque);
        }
    }
    
    /**
     * Called, then the left Thumbstick is dragged
     */
    public void OnLeftThumbstickInput(InputAction.CallbackContext context)
    {
        leftStickInput = context.ReadValue<Vector2>();
        //print("LTS Input: " + leftStickInput.magnitude);
    }
}