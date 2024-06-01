using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
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
        Destroy(gameObject);
    }
}

