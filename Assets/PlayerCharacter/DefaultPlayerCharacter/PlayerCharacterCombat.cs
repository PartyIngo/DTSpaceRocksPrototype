using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCharacterCombat : MonoBehaviour
{
    [Header("Enable/Disable specific Functions")]
    public bool isShootingEnabled;
    
    [Header("Shooting Settings")]
    [Tooltip("The gameObject, the playerCharacter shoots")]
    public GameObject bullet;

    [Tooltip("The time between each bullet while the Shooting button is pressed")]
    public float shootingInterval;
    float nextShot = 0.0f;

    [Tooltip("Base damage of the character.")]
    public float damageBase;

    bool isShooting;

    // Update is called once per frame
    void Update()
    {
        if (isShootingEnabled)
        {
            HandleShooting();
        }
    }


    /**
     * Checks if the player wants to shoot as well as the cooldown for a new shot is enabled
     * If both is true, a new bullet will be instantiated
     */
    void HandleShooting()
    {
        //Check if shooting is physically enabled (via Gamepad) AND if time has passed to enable the next shot
        if (isShooting && Time.time > nextShot)
        {
            //Set a new time to disable shooting until time has passed
            nextShot = Time.time + shootingInterval;

            //Create a bullet
            GameObject tempBullet = Instantiate(bullet, transform.position, transform.rotation);
            tempBullet.gameObject.SendMessage("SetDamage", damageBase);
        }
    }

    /**
     * Check for Gamepad Input for Shooting
     * Enable shooting if respective button is pressed and disable it on release
     */
    public void OnShooting(InputAction.CallbackContext context)
    {
        //If pressed, enable shooting
        if (context.started)
        {
            isShooting = true;
        }

        //If released, disable shooting
        if (context.canceled)
        {
            isShooting = false;
        }
    }
}
