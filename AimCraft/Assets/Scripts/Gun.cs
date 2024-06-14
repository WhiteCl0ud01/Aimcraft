using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float fireRate = 1f;
    private float nextFireTime;
    public Camera gameCamera;
    public Transform bulletSpawnPoint;
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
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Generate a ray from the game camera through the center of the screen
        Ray ray = gameCamera.ScreenPointToRay(screenCenter);

        // Perform the raycast
        RaycastHit hitinfo;
        bool hit = Physics.Raycast(ray, out hitinfo, 20f);

        //Debug.DrawRay(bulletSpawnPoint.position, ray.direction * 100, Color.red, 2.0f);

        //Create the muzzle flash
        GameObject tempFlash;
        tempFlash = Instantiate(muzzleFlashPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        //Destroy the muzzle flash effect
        Destroy(tempFlash, 0.15f);


        if (hit)
        {
            print(hitinfo.collider.gameObject.tag);
            if(hitinfo.collider.gameObject.tag == "Target")
            {
                Target target = hitinfo.collider.GetComponentInParent<Target>();
                if (target != null)
                {
                    target.OnHit(hitinfo.collider.gameObject.name);
                }
            }
            if (hitinfo.collider.gameObject.tag == "GamemodeNStart")
            {
                GamemodeNStart start = hitinfo.collider.GetComponentInParent<GamemodeNStart>();
                start.pressStart();
            }

        }
    }

    
}
