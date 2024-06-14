using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamemodeNStart : MonoBehaviour
{
    public GameObject targetPrefab; // Assign your Target prefab in the Inspector
    public float spawnInterval = 2f; // Interval between each spawn
    public float spawnChance = 0.5f; // Probability of spawning a target each interval

    private float nextSpawnTime;

    private float timer; // Timer variable to track elapsed time
    public TextMeshProUGUI timerText;
    public TextMeshPro startStopText;

    public bool spawn = false;
    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (spawn && Time.time >= nextSpawnTime)
        {
            SpawnTarget();
            // Check if we should spawn a target
            if (Random.value < spawnChance)
            {
                SpawnTarget();
            }

            // Schedule the next spawn time
            nextSpawnTime = Time.time + spawnInterval;
        }

        if (spawn)
        {
            timer += Time.deltaTime;

            // Update the timer text on the screen
            UpdateTimerText();
        }
    }
    public void pressStart()
    {
        spawn = !spawn;
        UpdateStartStopText();
        
        if (!spawn) //No spawn
        {
            timer = 0f;
            UpdateTimerText(); // Update text immediately when stopping spawn
            
        }
    }

    void UpdateTimerText()
    {
        timerText.text = "Time: " + timer.ToString("F2"); // Display timer with 2 decimal places

    }
    void UpdateStartStopText()
    {
        if (spawn)
        {
            startStopText.text = "Stop";
        }
        else
        {
            startStopText.text = "Start";
        }
    }
    void SpawnTarget()
    {
        // Randomly select a spawn point
        float xPos = Random.Range(-8f, 8f);
        float zPos = Random.Range(-6f, 8f);

        Vector3 randomPosition = new Vector3(xPos, 0, zPos);
        // Instantiate a new target at the spawn point
        Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
        GameObject newTarget = Instantiate(targetPrefab, randomPosition, targetRotation);

        // Optionally, you can set up additional behaviors or configurations for the spawned target here
    }
}
