using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Checks if the gameObject is out of boundaries of the playing field. 
 * If so, the gameObject will be translated to the opposite side of the playing field, 
 * to re-enter the field again
 */
public class BoundariesHandler : MonoBehaviour
{
    public int boundaryX;
    public int boundaryY;
    Vector3 tmp;

    // Update is called once per frame
    void Update()
    {
        tmp = gameObject.transform.position;

        if (gameObject.transform.position.x > boundaryX)
        {
            //Change it's position to be inside of the left border of the field
            tmp.x = -boundaryX + 1;   //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
            gameObject.transform.position = tmp;
        }

        //If spaceship has passed the left border...
        if (gameObject.transform.position.x < -boundaryX)
        {
            //Change it's position to be inside of the left border of the field
            tmp.x = boundaryX - 1;   //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
            gameObject.transform.position = tmp;
        }

        //If spaceship hahs passed top border...
        if (gameObject.transform.position.y > boundaryY)
        {
            //Change it's position to be inside of the left border of the field
            tmp.y = -boundaryY + 1;    //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
            gameObject.transform.position = tmp;
        }

        //If spaceship hahs passed bottom border...
        if (gameObject.transform.position.y < -boundaryY)
        {
            //Change it's position to be inside of the left border of the field
            tmp.y = boundaryY - 1;    //Make sure, the ship will spawn WITHIN the borders to avoid switching its signs infinitely
            gameObject.transform.position = tmp;
        }
    }
}
