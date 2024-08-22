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
    public GameObject woodImpactPrefab;
    public GameObject stoneImpactPrefab;
    public GameObject bodyImpactPrefab;
    public TextMeshProUGUI ammoText;
    private int ammoCount = 30;
    private int maxAmmoCount = 30;
    public AudioClip gunShot;
    public AudioClip reloading;
    private AudioSource audioSource;
    private bool infiniteBullet = false;
    public TextMeshPro infiniteBulletText;
    public TextMeshPro infiniteBulletSelectorText;
    private bool isReloading = false;
    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!infiniteBullet)
        {
            ammoText.text = ammoCount + "/" + maxAmmoCount;
            if (ammoCount <= 0 && !isReloading)
            {
                StartCoroutine(Reload());
                audioSource.PlayOneShot(reloading);
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
        else
        {   ammoText.text = "";
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }
    IEnumerator Reload()
    {
        isReloading = true;
        ammoText.text = "Reloading";
        yield return new WaitForSeconds(1.6f);
        ammoCount = maxAmmoCount; 
        ammoText.text = ammoCount + "/" +maxAmmoCount;
        isReloading = false;
    }
    void UpdateONOFFText()
    {
        
        if (infiniteBullet)
        {
            infiniteBulletText.text = "Infinite Bullet ON";
            infiniteBulletSelectorText.text = "OFF";
        }
        else
        {
            infiniteBulletText.text = "Infinite Bullet OFF";
            infiniteBulletSelectorText.text = "ON";
        }
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
            if (hitinfo.collider.gameObject.tag == "Wood") //check if the object hitted has a tag named "Wood"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactInstance, 0.2f);

            }
            if (hitinfo.collider.gameObject.tag == "Obstacle") //check if the object hitted has a tag named "Obstacle"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactInstance, 0.2f);

            }
            if (hitinfo.collider.gameObject.tag == "Stone") //check if the object hitted has a tag named "Stone"
            {
                GameObject impactInstance = Instantiate(stoneImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactInstance, 0.2f);

            }
            if (hitinfo.collider.gameObject.tag == "Target") //check if the object hitted has a tag named "Target"
            {
                Target target = hitinfo.collider.GetComponentInParent<Target>(); //get the parent of the object
                if (target != null)
                {
                    target.OnHit(hitinfo.collider.gameObject.name); //perform onHit function
                    GameObject impactInstance = Instantiate(bodyImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                    Destroy(impactInstance, 0.2f);
                }
            }
            if (hitinfo.collider.gameObject.tag == "GamemodeNStart") //check if the object hitted has a tag named "GamemodeNStart"
            {
                GamemodeNStart start = hitinfo.collider.GetComponentInParent<GamemodeNStart>(); //get the parent of the object
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactInstance, 0.2f);
                if (hitinfo.collider.gameObject.name == "Start")
                {
                    start.pressStart(); //perform pressStart function
                }
                else
                {
                    start.gamemodeSelector(hitinfo.collider.gameObject.name);
                }
            }
            //for demo
            if (hitinfo.collider.gameObject.tag == "Demo")
            {
                TargetDemo targetDemo = hitinfo.collider.GetComponentInParent<TargetDemo>(); //get the parent of the object
                if (targetDemo != null)
                {
                    targetDemo.OnHit(hitinfo.collider.gameObject.name); //perform onHit function
                    GameObject impactInstance = Instantiate(bodyImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                    Destroy(impactInstance, 0.3f);
                }
            }

            if (hitinfo.collider.gameObject.tag == "Timer") //check if the object hitted has a tag named "Timer"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactInstance, 0.2f);
                GamemodeNStart start = hitinfo.collider.GetComponentInParent<GamemodeNStart>();
                start.changeTimer(hitinfo.collider.gameObject.name);
                print(hitinfo.collider.gameObject.name);
            }
            if (hitinfo.collider.gameObject.tag == "Sensitivity") //check if the object hitted has a tag named "Sensitivity"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactInstance, 0.2f);
                Sensitivity sensitivityobject = hitinfo.collider.GetComponentInParent<Sensitivity>();
                sensitivityobject.changeSens(hitinfo.collider.gameObject.name);
                print(hitinfo.collider.gameObject.name);
            }
            if (hitinfo.collider.gameObject.tag == "Bullet") //check if the object hitted has a tag named "Bullet"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactInstance, 0.2f);
                infiniteBullet = !infiniteBullet;
                ammoCount = 30;
                UpdateONOFFText();
            }
        }
    }

    
}
