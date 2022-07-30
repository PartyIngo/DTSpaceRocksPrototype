using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerCharacterMovement : MonoBehaviour
{
    [Header("Acceleration Stats")]
    [Tooltip("The inner boundaries for boost. If the Thumbstick's value is greater, a boost will be applied.")]
    public float boostZone;
    [Tooltip("How fast has the acceleration to be")]
    public float accelerationValue;
    [Tooltip("How large has the boost multiplier to be, when dragging the Thumbstick to it's limit")]
    public float accelerationBoostMultiplier;
    float currentAcceleration;
    Vector2 newForce;

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
    float angleDifference;

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

    [Header("Field Boundaries")]
    [Tooltip("The Distance from left or right Border to the center (0/0)")]
    public float borderDistanceX;
    [Tooltip("The Distance from top or bottom Border to the center (0/0)")]
    public float borderDistanceY;

    [Header("VFX: Spaceship Acceleration")]
    [Tooltip("The Component for the Spaceship's Flame")]
    public SpriteRenderer spaceshipFlame;
    [Tooltip("the Sprite Renderer asset of the boosting flame")]
    public SpriteRenderer boostFlame;
    [Tooltip("The glowing dot to lighten up the area around the flame")]
    public SpriteRenderer flameGlow;
    [Tooltip("the Sprite Renderer asset of the accelerating flame")]
    public SpriteRenderer accelFlame;
    [Tooltip("Color of glowing dot for acceleration flame")]
    public Color accelerationDotColor;
    [Tooltip("Color of glowing dot for boost flame")]
    public Color boostDotColor;

    [Header("Player Health & incoming Damage")]
    [Tooltip("The maximum amount of lives, the player has.")]
    [Range(0, 10)]
    [SerializeField]
    public int lives;
    [Tooltip("The Duration of invulnerability after being hit in Seconds")]
    [SerializeField]
    float respawnInvulnerabilityDuration;
    [Tooltip("The current cooldown for invulnerability")]
    float invulnerabilityCooldown;
    [Tooltip("The UI indicator for current Lives")]
    public Sprite healthsprite;




    [Header("VFX: Incoming Damage")]
    [Tooltip("Color tint when damaged")]
    public Color damageTint;
    [Tooltip("Duration of Color tint when damaged in seconds")]
    public float damageDuration;
    [Tooltip("If the asteroids gets damage just now")]
    bool getsDamage;
    float nextTime;

    public Animator spaceShipAnimator;

    SpriteRenderer ownSprite;
    float normalizedAngleDifference;
    Rigidbody2D rb;


    
    /**
     * prepare some values by starting this script
     */
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.drag = defaultFriction;

        ownSprite = GetComponent<SpriteRenderer>();

        spaceShipAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        handleIncomingDamage();
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
            //vfx: enable normal flame
            spaceshipFlame.enabled = true;

            //When dragging the Left Thumbstick a little bit, the acceleration is normal
            if (leftStickInput.magnitude < boostZone)
            {
                currentAcceleration = (leftStickInput.normalized.magnitude * accelerationValue);
            }

            //When dragging the Left Thumbstick to it's limit, a boost has to be applied, to increase the acceleration to a maximum
            if (leftStickInput.magnitude >= boostZone)
            {
                currentAcceleration = (leftStickInput.normalized.magnitude * accelerationValue * accelerationBoostMultiplier);
                //print("BOOST!");

                //VFX: change glow color and flame from acceleration to boost variant
                spaceshipFlame.enabled = false;
                boostFlame.enabled = true;
            }
            //when not boosting, the boosting flame has to be disabled
            else
            {
                boostFlame.enabled = false;
            }

            //Adds the force to the gameobject to move it. Acceleration feels better, when rotating the object to 180°
            newForce = (transform.up * currentAcceleration) - ((transform.up * currentAcceleration) * Mathf.Abs(normalizedAngleDifference) / 180);
            rb.AddForce(newForce);
        }

        //LTS is within deadzone, so neither boost nor acceleration are applied
        else
        {
            //VFX: disable flame sprite and glow because ship isn't moving and fire is extinguished

            spaceshipFlame.enabled = false;
            boostFlame.enabled = false;
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

            angleDifference = normalizedAngleDifference;

            //when Spaceships current rotation is greater than the brakeAngle, reduce the friction to increase the torque speed
            if (Mathf.Abs(angleDifference) > brakeAngle)
            {
                variableAngularDrag = 0;
            }

            //Otherwise, when the angleDifference is less than the brakeAngle's value, increase the angular drag exponentially the smaller the angleDifference becomes
            else
            {
                variableAngularDrag = maxVariableAngularDrag - (maxVariableAngularDrag * Mathf.Abs(angleDifference) / brakeAngle);
            }

            //set the angular Drag Property of the Rigidbody Component
            rb.angularDrag = defaultAngularDrag + Mathf.Abs(variableAngularDrag);

            //rotate the spaceship leftwards
            if (Mathf.Sign(normalizedAngleDifference) > 0)
            {
                torque = Mathf.Abs(torque);
                maxFastTurnTorque = Mathf.Abs(maxFastTurnTorque);
            }
            else
            {
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
        }
    }

    /**
     * Handles strafing by checking LT and RT with XOR. Only one of them can be pressed at the same time. The other ine has to be released.
     * If this condition occurs, a strafe in the respecrive direction may be applied. 
     * If both of them are (not) pressed, the XOR Statement will return false and the strafe will be blocked immediately 
     */
    void HandleStrafing()
    {
        //If LT+RT are pressed, divide linear drag by 4.
        if (isStrafingLeft && isStrafingRight)
        {
            rb.drag = defaultFriction/4;
        }
        //Else the linear friction resets.
        else
        {
            rb.drag = defaultFriction;
        }

        //Apply the force sideways, to make the Spaceship strafe
        rb.AddForce(transform.right * strafingValue * strafeSpeed);
    }


    /**
     * Check for Collisions with foes
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            getsDamage = true;
        }
    }


    /**
     * Handle the player's incoming damage
     * The player has a specific amount of lives
     */
    void handleIncomingDamage()
    {
        if (getsDamage)
        {
            ownSprite.color = damageTint;
            nextTime = Time.time + damageDuration;
            getsDamage = false;

            //1. Check for remaining health
            //1.1 If remaining life <= 0 --> Overlay/Game Over
            //1.2 Else set cooldown
            // while in Cooldown, the collider is disabled. If the cooldown expired, the collider is enabled again


            //the player has lives remaining and is not invulnerable at the moment, then we can subtract a life after being hit. The cooldown needs to be applied again
            if (lives > 1)
            {
                if (Time.time > invulnerabilityCooldown)
                {
                    invulnerabilityCooldown = Time.time + respawnInvulnerabilityDuration;
                    lives--;
                }
                Debug.Log("Lives: " + lives);
                Debug.Log("Time: " + Time.time);
            }
            else
            {
                if (Time.time > invulnerabilityCooldown)
                {
                    //Trigger visibility of Game Over Overlay
                    ScoreScript.isOverlayActive = true;

                    Destroy(gameObject);
                }
                Debug.Log("Lives: " + lives);
                Debug.Log("Time: " + Time.time);
            }


            ////Reload Scene
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        //TODO: optimizing that the color changes not every frame
        if (Time.time > nextTime)
        {
            ownSprite.color = Color.white;
        }
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
        }

        //On LTS released, the friction is set to it's default value
        if (context.canceled)
        {
            rb.drag = defaultFriction;
        }
    }

    /**
     * Called, then the left Thumbstick is dragged
     * The input value will be stored for further usage
     */
    public void OnLeftThumbstickInput(InputAction.CallbackContext context)
    {
        leftStickInput = context.ReadValue<Vector2>();
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