using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float Health = 150;
    public float lifetime = 0.5f;
    private GamemodeNStart gamePanelScript;

    void Start()
    {
        GameObject gamePanelObject = GameObject.Find("GamePanel");
        gamePanelScript = gamePanelObject.GetComponent<GamemodeNStart>();

    }
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            if (gamePanelScript.gamemodeType != "Survival")
            {
                gamePanelScript.missTarget();
                Destroy(gameObject);
            }
        }
        if (!gamePanelScript.spawn){
            gamePanelScript.missTarget();
            Destroy(gameObject);
        }
    }

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
            gamePanelScript.hitTarget();
            Destroy(gameObject);
            
        }
    }

}

