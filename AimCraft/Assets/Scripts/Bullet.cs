using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0.001f;
    public float lifeTime = 2f;

    void Start()
    {
        // Remove the bullet after 2 seconds
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // shoot the bullets towards the target
        transform.Translate(Vector3.forward * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Destroy the bullet on impact
        print("Attack Right!");
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        print("HIT1");
        // Check if the bullet hits the target
        if (other.CompareTag("Target"))
        {
            // Make the target disappear
            Destroy(other.gameObject);

            // Optionally, destroy the bullet as well upon collision
            Destroy(gameObject);

            print("HIT2");
        }
    }
}

