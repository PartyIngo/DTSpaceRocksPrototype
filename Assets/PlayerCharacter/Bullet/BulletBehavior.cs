using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [Tooltip("The speed of which the bullet is flying")]
    public float speed;

    [Tooltip("the radius from the centre of the field to the vertical borders (left/right)")]
    public float borderX;
    [Tooltip("the radius from the centre of the field to the horizontal borders (top/bottom)")]
    public float borderY;

    void Update()
    {
        //The projectile has to fly forward ad a specified speed
        transform.position += transform.up * Time.deltaTime * speed;

        //Destroy the bullet when outside of the field
        if (transform.position.x < -borderX)
        {
            Destroy(gameObject);
        }
        if (transform.position.x > borderX)
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -borderY)
        {
            Destroy(gameObject);
        }
        if (transform.position.y > borderY)
        {
            Destroy(gameObject);
        }
    }
}