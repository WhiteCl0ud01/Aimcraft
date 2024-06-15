using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDemo : MonoBehaviour
{

    void Start()
    {
    }

    public void OnHit(string bodyparts)
    {
        if (bodyparts == "BodyHitbox")
        {
            print("Hit 40");
        }
        else if (bodyparts == "HeadHitbox")
        {
            print("Hit 150");
        }
    }
}
