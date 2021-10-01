using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    #region Variables
    [Header("Asteroid Variables")]

    [Tooltip("Maximum movement force of the Asteroid.")]
    public float forceMax;
    [Tooltip("Actual force of the Asteroid.")]
    Vector2 force;

    [Tooltip("Health of the Asteroid. If it reaches 0, the Asteroid is destroyed.")]
    public float health;

    Rigidbody2D rb;

    [Tooltip("Maximum X-Coord.")]
    public float Xmax;
    [Tooltip("Maximum Y-Coord.")]
    public float Ymax;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        force = new Vector2(Random.Range(-forceMax, forceMax), Random.Range(-forceMax, forceMax));
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(force);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 tmp = transform.position;

        if (transform.position.x > Xmax)
        {
            tmp.x = -Xmax + 1;   
            transform.position = tmp;
        }

        if (transform.position.x < -Xmax)
        {
            tmp.x = Xmax - 1; 
            transform.position = tmp;
        }

        if (transform.position.y > Ymax)
        {
            tmp.y = -Ymax + 1;
            transform.position = tmp;
        }

        if (transform.position.y < -Ymax)
        {
            tmp.y = Ymax - 1;
            transform.position = tmp;
        }
    }

    //Damage & Destroy VFX
    public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
