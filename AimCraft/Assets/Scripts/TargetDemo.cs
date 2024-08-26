using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetDemo : MonoBehaviour
{
    public TextMeshPro demoText;
    public int shotHit;
    private string[] messages = {
          "Don't shoot me!",
          "Please start the game!",
          "You got this!",
          "Take your best shot!",
          "Aim carefully!",
          "Stay focused!",
          "Nice move!",
          "Stay sharp!",
          "Stay calm!"
     };
    void Start()
    {
        
    }

    public void OnHit(string bodyparts)
    {
        shotHit++;
        float randomValue = Random.Range(0f, 1f);
        if (shotHit > 10 && randomValue > 0.7)
        {
            int randomInt = Random.Range(0, 8);
            demoText.text = messages[randomInt];
        }
        else
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
}
