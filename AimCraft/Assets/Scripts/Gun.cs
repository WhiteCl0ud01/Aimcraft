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

    //Bullets & Recoil
    public GameObject bulletPrefab;
    private float bulletSpeed = 100f;
    public float spreadAngle = 1f;
    public GunRecoil gunRecoil;

    public int initialAccurateShots = 5; 
    private int shotsFired = 0; 
    public float initialSpreadAngle = 1f;
    public float maxSpreadAngle = 10f; 
    public float spreadIncreaseRate = 0.5f;
    public float spreadRecoveryRate = 0.2f;
    private float currentSpreadAngle;
    private float recoilTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //For sound effects
    }

    void Update()
    {
        if (!infiniteBullet) //if its limited bullets
        {
            ammoText.text = ammoCount + "/" + maxAmmoCount;
            if (ammoCount <= 0)
            {
                if (!isReloading) //Reloading
                {
                    StartCoroutine(Reload());
                    audioSource.PlayOneShot(reloading);
                }
            }
            else
            {
                recoilShooting();
            }
        }
        else
        { 
            ammoText.text = "";
            recoilShooting();
        }
    }

    public void recoilShooting() //Logic for recoil shooting
    {
        if (Input.GetMouseButton(0)) //Left click is pressed
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                ammoText.text = ammoCount + "/" + maxAmmoCount;
                nextFireTime = Time.time + fireRate;
            }

            if (recoilTime == 0) //Track the time when shooting started
            {
                recoilTime = Time.time;
            }
            
        }
        else //Left click is released
        {
            shotsFired = 0; //Reset values
            recoilTime = 0; //Reset values
            gunRecoil.ResetRecoil(); //Reset the recoil count for the gun
            currentSpreadAngle = Mathf.Lerp(currentSpreadAngle, initialSpreadAngle, spreadRecoveryRate * Time.deltaTime); //Calculate the spread angle
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
        
        audioSource.PlayOneShot(gunShot); //Play sound effect
        ammoCount -= 1; //Reduce ammo count

        gunRecoil.ApplyRecoil(); // Apply recoil to the gun(moves it)
        
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0); // locates the middle of the screen
        Ray ray = gameCamera.ScreenPointToRay(screenCenter); // points to the middle of the screen
        Vector3 directionWithSpread; 

        if (shotsFired < initialAccurateShots) //For the first few accurate shots
        {
            directionWithSpread = ray.direction; //use the ray direction directly
        }
        else //For the inaccurate sprays
        {
            float timeHeld = Time.time - recoilTime; //Using the time that the user hold onto the left click button
            //Calculate the bullet spread angle based on how long the fire button is held, ensuring the angle stays within a specified range from initialSpreadAngle to maxSpreadAngle.
            currentSpreadAngle = Mathf.Clamp(initialSpreadAngle + timeHeld * spreadIncreaseRate, initialSpreadAngle, maxSpreadAngle);

            //Ensure bullets fired have a spread effect in the X and Y axis rotations while keeping the z-axis rotation unchanged.
            Quaternion spreadRotation = Quaternion.Euler(
                Random.Range(-currentSpreadAngle, currentSpreadAngle),
                Random.Range(-currentSpreadAngle, currentSpreadAngle),
                0
            );

            directionWithSpread = spreadRotation * ray.direction; // multiply the spreadRotation by the ray.direction to alter the direction of the bullet to include the spread effect
        }

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, muzzleSpawnPoint.position, muzzleSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>(); 
        rb.velocity = directionWithSpread * bulletSpeed; 
        rb.useGravity = true;
        bullet.transform.Rotate(90f, 0f, 0f); //Rotate the bullet so it is facing the correct direction
        Destroy(bullet, 2f); //Destroy the bullet after 2 seconds

        shotsFired++;


        RaycastHit hitinfo;
        bool hit = Physics.Raycast(ray.origin, directionWithSpread, out hitinfo, 20f); //perform the raycast

        GameObject tempFlash = Instantiate(muzzleFlashPrefab, muzzleSpawnPoint.position, muzzleSpawnPoint.rotation); //Spawning the muzzle flash at the muzzle spawnpoint
        Destroy(tempFlash, 0.15f); //Destroy the muzzle flash effect


        if (hit) //if it hits something
        {
            if (hitinfo.collider.gameObject.tag == "Wood") //check if the object hitted has a tag named "Wood"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal)); //Instantiate the bullet impact on wood
                Destroy(impactInstance, 0.2f); //Destroy the impact after 0.2 sec

            }
            if (hitinfo.collider.gameObject.tag == "Obstacle") //check if the object hitted has a tag named "Obstacle"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal)); //Instantiate the bullet impact on wood
                Destroy(impactInstance, 0.2f); //Destroy the impact after 0.2 sec

            }
            if (hitinfo.collider.gameObject.tag == "Stone") //check if the object hitted has a tag named "Stone"
            {
                GameObject impactInstance = Instantiate(stoneImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal)); //Instantiate the bullet impact on stone
                Destroy(impactInstance, 0.2f); //Destroy the impact after 0.2 sec

            }
            if (hitinfo.collider.gameObject.tag == "Target") //check if the object hitted has a tag named "Target"
            {
                Target target = hitinfo.collider.GetComponentInParent<Target>(); //get the parent of the object
                if (target != null)
                {
                    target.OnHit(hitinfo.collider.gameObject.name); //perform onHit function
                    GameObject impactInstance = Instantiate(bodyImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal)); //Instantiate the bullet impact on body
                    Destroy(impactInstance, 0.2f); //Destroy the impact after 0.2 sec
                }
            }
            if (hitinfo.collider.gameObject.tag == "GamemodeNStart") //check if the object hitted has a tag named "GamemodeNStart"
            {
                GamemodeNStart start = hitinfo.collider.GetComponentInParent<GamemodeNStart>(); //get the GamemodeNStart script

                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal)); //Instantiate the bullet impact on wood
                Destroy(impactInstance, 0.2f); //Destroy the impact after 0.2 sec

                if (hitinfo.collider.gameObject.name == "Start")
                {
                    start.pressStart(); //perform pressStart function
                }
                else
                {
                    start.gamemodeSelector(hitinfo.collider.gameObject.name); //Change the gamemodes
                }
            }
            //for demo
            if (hitinfo.collider.gameObject.tag == "Demo")
            {
                TargetDemo targetDemo = hitinfo.collider.GetComponentInParent<TargetDemo>(); //get the parent of the object
                if (targetDemo != null)
                {
                    targetDemo.OnHit(hitinfo.collider.gameObject.name); //perform onHit function
                    GameObject impactInstance = Instantiate(bodyImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));  //Instantiate the bullet impact on body
                    Destroy(impactInstance, 0.2f); //Destroy the impact after 0.2 sec
                }
            }

            if (hitinfo.collider.gameObject.tag == "Timer") //check if the object hitted has a tag named "Timer"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal)); //Instantiate the bullet impact on wood
                Destroy(impactInstance, 0.2f); //Destroy the impact after 0.2 sec

                GamemodeNStart start = hitinfo.collider.GetComponentInParent<GamemodeNStart>(); //get the GamemodeNStart script
                start.changeTimer(hitinfo.collider.gameObject.name); //Change the timers

            }
            if (hitinfo.collider.gameObject.tag == "Sensitivity") //check if the object hitted has a tag named "Sensitivity"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal)); //Instantiate the bullet impact on wood
                Destroy(impactInstance, 0.2f); //Destroy the impact after 0.2 sec

                Sensitivity sensitivityobject = hitinfo.collider.GetComponentInParent<Sensitivity>(); //get the Sensitivity script
                sensitivityobject.changeSens(hitinfo.collider.gameObject.name); //change the Sensitivity
            }
            if (hitinfo.collider.gameObject.tag == "Bullet") //check if the object hitted has a tag named "Bullet"
            {
                GameObject impactInstance = Instantiate(woodImpactPrefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal)); //Instantiate the bullet impact on wood
                Destroy(impactInstance, 0.2f); //Destroy the impact after 0.2 sec
                infiniteBullet = !infiniteBullet;
                ammoCount = 30;
                UpdateONOFFText();
            }
        }
    }

    
}
