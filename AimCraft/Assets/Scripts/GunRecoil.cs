using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float baseRecoilAmount = 1f; 
    public float maxRecoil = 4f; 
    public float recoverySpeed = 1f; 
    public int initialAccurateShots = 3;
    public float recoilIncreaseRate = 1.5f; 

    private Vector3 originalRotation;
    private Vector3 currentRecoil;
    private int shotsFired = 0;

    void Start()
    {
        originalRotation = transform.localEulerAngles; //captures the current local rotation of the gun and stores it
    }

    void Update() //slowly smoothen out the recoil to original location
    {
        currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, recoverySpeed * Time.deltaTime); 
        transform.localEulerAngles = originalRotation + currentRecoil;
    }

    public void ApplyRecoil()
    {
        float recoilAmount = baseRecoilAmount;

        if (shotsFired > initialAccurateShots) //when the shots fired is more than initialAccurateShots, add more to the recoil amount
        {
            recoilAmount += (shotsFired - initialAccurateShots) * recoilIncreaseRate;
            recoilAmount = Mathf.Clamp(recoilAmount, baseRecoilAmount, maxRecoil);
        }

        Vector3 recoil = new Vector3(-recoilAmount, Random.Range(-recoilAmount, recoilAmount), 0); //randomly calculate the recoil ammount and add to the current recoil
        currentRecoil += recoil;
        currentRecoil.x = Mathf.Clamp(currentRecoil.x, -maxRecoil, maxRecoil);

        shotsFired++;
    }

    public void ResetRecoil()
    {
        shotsFired = 0;
    }
}
