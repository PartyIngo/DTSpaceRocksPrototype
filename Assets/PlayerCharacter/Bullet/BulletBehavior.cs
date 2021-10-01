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

    [Tooltip("The damage this bullet deals.")]
    public float damage;

    Rigidbody2D rb;
    public float force;
    Vector2 tempForce;


    private void Start()
    {
        tempForce = transform.up * force;

        print("tempForce" + tempForce);

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(tempForce);
    }

    void Update()
    {
        //The projectile has to fly forward ad a specified speed
        //transform.position += transform.up * Time.deltaTime * speed;


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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Bullet Damage Before:  " + damage);
        if (collision.gameObject.tag == "Enemy")
        {
            print("Bullet Damage After:  " + damage);

            collision.gameObject.SendMessage("Damage", damage);

            //TBD: DEstroy VFX
            Destroy(gameObject);
        }

        //if (collision.gameObject.tag == "bullet")
        //{
        //    Physics.IgnoreCollision(theobjectToIgnore.collider, collider);
        //}

    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }


}
