using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyBehavior : MonoBehaviour
{
    [Tooltip("the radius from the centre of the field to the vertical borders (left/right)")]
    public float borderX;
    [Tooltip("the radius from the centre of the field to the horizontal borders (top/bottom)")]
    public float borderY;

    [Tooltip("The minimum scale of the target dummy")]
    public float minScale;
    [Tooltip("The maximum scale of the target dummy")]
    public float maxScale;

    //When the Target Dummy is hit by a bullet, destroy the Bullet and change the dummy's position
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("Collision detected");
     
        //Destroy the bullet
        Destroy(other);

        //Calculate a new random position within the field borders
        Vector3 temp = transform.position;
        temp.x = Random.Range(-borderX, borderX);
        temp.y = Random.Range(-borderY, borderY);

        //Change the target dummy's position
        transform.position = temp;


        //Change the target dummy's scale randomly
        temp = transform.localScale;
        
        float randomScale = Random.Range(minScale, maxScale);

        temp.x = randomScale;
        temp.y = randomScale;
        temp.z = randomScale;

        transform.localScale = temp;
    }
}
