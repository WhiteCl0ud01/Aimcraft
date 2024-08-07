using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gun : MonoBehaviour
{
    public float fireRate = 1f;
    private float nextFireTime;
    public Camera gameCamera;
    public Transform muzzleSpawnPoint;
    public GameObject muzzleFlashPrefab;
    public GameObject impactPrefab;
    public TextMeshProUGUI ammoText;
    private int ammoCount = 30;
    private int maxAmmoCount = 30;
    public AudioClip gunShot;
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (ammoCount <= 0)
        {
            StartCoroutine(Reload());
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                Shoot();
                ammoText.text = ammoCount + "/" + maxAmmoCount;
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    IEnumerator Reload()
    {
        ammoText.text = "Reloading";
        yield return new WaitForSeconds(1.5f);
        ammoCount = maxAmmoCount; 
        ammoText.text = ammoCount + "/" +maxAmmoCount;
    }

    void Shoot()
    {
        audioSource.PlayOneShot(gunShot);
        ammoCount -= 1;
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
                    GameObject impactInstance = Instantiate(impactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                    Destroy(impactInstance, 0.2f);
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
                    GameObject impactInstance = Instantiate(impactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                    Destroy(impactInstance, 0.3f);
                }
            }

            if (hitinfo.collider.gameObject.tag == "Timer") //check if the object hitted has a tag named "Timer"
            {
                GamemodeNStart start = hitinfo.collider.GetComponentInParent<GamemodeNStart>();
                start.changeTimer(hitinfo.collider.gameObject.name);
                print(hitinfo.collider.gameObject.name);
            }
            if (hitinfo.collider.gameObject.tag == "Sensitivity") //check if the object hitted has a tag named "Sensitivity"
            {
                Sensitivity sensitivityobject = hitinfo.collider.GetComponentInParent<Sensitivity>();
                sensitivityobject.changeSens(hitinfo.collider.gameObject.name);
                print(hitinfo.collider.gameObject.name);
            }
        }
    }

    
}
