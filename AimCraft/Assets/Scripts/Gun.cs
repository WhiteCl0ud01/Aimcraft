using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireRate = 1f;
    private float nextFireTime;
    public Camera gameCamera;
    public Transform muzzleSpawnPoint;
    public GameObject muzzleFlashPrefab;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    void Shoot()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0); //position of the center of the screen
        Ray ray = gameCamera.ScreenPointToRay(screenCenter); //pointing the ray to the center of the screen

        RaycastHit hitinfo;
        bool hit = Physics.Raycast(ray, out hitinfo, 20f); //perform the raycast

        //Debug.DrawRay(muzzleSpawnPoint.position, ray.direction * 100, Color.red, 2.0f);//for debugging the ray

        GameObject tempFlash = Instantiate(muzzleFlashPrefab, muzzleSpawnPoint.position, muzzleSpawnPoint.rotation); //Spawning the muzzle flash at the muzzle spawnpoint
        Destroy(tempFlash, 0.15f); //Destroy the muzzle flash effect


        if (hit) //if it hits something
        {
            if(hitinfo.collider.gameObject.tag == "Target") //check if the object hitted has a tag named "Target"
            {
                Target target = hitinfo.collider.GetComponentInParent<Target>(); //get the parent of the object
                if (target != null)
                {
                    target.OnHit(hitinfo.collider.gameObject.name); //perform onHit function
                }
            }
            if (hitinfo.collider.gameObject.tag == "GamemodeNStart") //check if the object hitted has a tag named "GamemodeNStart"
            {
                GamemodeNStart start = hitinfo.collider.GetComponentInParent<GamemodeNStart>(); //get the parent of the object
                start.pressStart(); //perform pressStart function
            }
            //for demo
            if (hitinfo.collider.gameObject.tag == "Demo")
            {
                TargetDemo targetDemo = hitinfo.collider.GetComponentInParent<TargetDemo>(); //get the parent of the object
                if (targetDemo != null)
                {
                    targetDemo.OnHit(hitinfo.collider.gameObject.name); //perform onHit function
                }
            }

            if (hitinfo.collider.gameObject.tag == "Timer") //check if the object hitted has a tag named "Timer"
            {
                GamemodeNStart start = hitinfo.collider.GetComponentInParent<GamemodeNStart>();
                start.changeTimer(hitinfo.collider.gameObject.name);
                print(hitinfo.collider.gameObject.name);
            }
        }
    }

    
}
