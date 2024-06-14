using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float Health = 150;
    public void OnHit(string bodyparts)
    {
        if(bodyparts == "BodyHitbox")
        {
            Health -= 40;
        }
        else if(bodyparts == "HeadHitbox")
        {
            Health -= 150;
        }
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

}