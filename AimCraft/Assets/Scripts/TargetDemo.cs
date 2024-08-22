using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetDemo : MonoBehaviour
{
    public TextMeshPro demoText;
    void Start()
    {
    }

    public void OnHit(string bodyparts)
    {
        if (bodyparts == "BodyHitbox")
        {
            demoText.text = "Body shot deals 40 damage which needs 4 shots to kill the target";
        }
        else if (bodyparts == "HeadHitbox")
        {
            demoText.text = "Headshot deals 150 damage which kills the target instantly";
        }
    }
}
