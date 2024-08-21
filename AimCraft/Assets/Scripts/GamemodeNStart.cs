using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamemodeNStart : MonoBehaviour
{
    public GameObject targetPrefab; // Assign your Target prefab in the Inspector
    public float spawnInterval = 0.5f; // Interval between each spawn

    private float nextSpawnTime;
    private float setTime = 30f; //Timer variable of the time selected, default is 30s
    private float timer; // Timer variable to track elapsed time
    private int score; // Score variable to track the number of target hitted
    private int missed; // Score variable to track the number of target missed
    private float hitRate; // Score variable to track the hit rate
    public TextMeshProUGUI timerText; //Timer text
    public TextMeshPro timeSelectedText; //Time selected text
    public TextMeshProUGUI scoreText; //Score text
    public TextMeshProUGUI missedText; //Missed text
    public TextMeshProUGUI hitRateText; //Hit Rate text
    public TextMeshPro startStopText; //Start and Stop text

    private string gamemodeType;


    public bool spawn = false;
    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (spawn && Time.time >= nextSpawnTime) // If spawning is true, spawn new target
        {
            SpawnTarget();
            // Schedule the next spawn time
            nextSpawnTime = Time.time + spawnInterval;
        }

        if (spawn) // If the game is started
        {
            timer -= Time.deltaTime; // Count down to 0
            // Update the timer text on the screen
            UpdateTimerText();
        }

        if (timer <= 0) //if countdown to 0
        {
            spawn = false;
            UpdateTimerDuration();
            UpdateTimerText();
            UpdateStartStopText();

            
        }
    }
    public void pressStart() //When Start/Stop is presssed
    {
        spawn = !spawn; //change the spawning of target to on/off
        UpdateStartStopText();
        UpdateTimerDuration();
        UpdateTimerText();
        if (spawn) // if the game starting/restarting change the score text to 0;
        {
            score = 0;
            missed = 0;
            UpdateScoreText();
            UpdateMissedText();
            UpdateHitRateText();
        }
    }
    void UpdateTimerDuration()
    {
        if (spawn)
        {
            timer = setTime;
        }
        else
        {
            timer = 0f;
        }

    }
    void UpdateTimerText()
    {
        timerText.text = "Time: " + timer.ToString("F2"); // Display timer with 2 decimal places

    }
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score; // Display score

    }
    void UpdateMissedText()
    {
        missedText.text = "Missed: " + missed; // Display missed score

    }
    void UpdateHitRateText()
    {
        int total = score + missed;
        if (total == 0)
        {
            hitRateText.text = "Hit Rate: 0.00%"; // Display hit rate
        }
        else {

            hitRate = (score / (float)total) * 100;
            hitRateText.text = $"Hit Rate: {hitRate:F2}%"; // Display hit rate
        }
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

    }
    public void hitTarget()
    {
        score += 1;
        UpdateScoreText();
        UpdateHitRateText();
    }
    public void missTarget()
    {
        missed += 1;
        UpdateMissedText();
        UpdateHitRateText();
    }
    public void changeTimer(string timing)
    {
        if (timing == "15Second")
        {
            setTime = 15f;
            timeSelectedText.text = "Time Selected: 15 sec";
        }
        else if (timing == "30Second")
        {
            setTime = 30f;
            timeSelectedText.text = "Time Selected: 30 sec";
        }
        else if (timing == "45Second")
        {
            setTime = 45f;
            timeSelectedText.text = "Time Selected: 45 sec";
        }
        else if (timing == "60Second")
        {
            setTime = 60f;
            timeSelectedText.text = "Time Selected: 60 sec";
        }
    }
}
