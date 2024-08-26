using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamemodeNStart : MonoBehaviour
{
    public GameObject targetPrefab; // Assign your Target prefab in the Inspector
    public float spawnInterval = 0.5f; // Interval between each spawn
    private int maxNoOfTarget = 2; //Number of targets allowed on the map
    public int NoOfTargetOnMap = 0; //Number of targets on the map
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
    public TextMeshPro gamemodeText;
    // Options
    public TextMeshPro precisionText;
    public TextMeshPro flickText;
    public TextMeshPro survivalText;
    public TextMeshPro Timer1Text;
    public TextMeshPro Timer2Text;
    public TextMeshPro Timer3Text;
    public TextMeshPro Timer4Text;
    //Tutorial
    public TextMeshPro gamemodeTutorialText;
    public string gamemodeType;
    public string prevgamemodeType;
    private float timePassed;
    //spawn pointrange
    private float[] xPosRange = new float[] { -4f, 4f };
    private float[] zPosRange = new float[] { -6f, 9f };//  9toward the back wall
    public bool spawn = false; //spawning started
    public GameObject Obstacles;
    public GameObject Options;
    public GameObject audioSourceOrigin;
    public AudioSource audioSource;
    public AudioClip killEffect;
    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
        gamemodeType = "Flick Test";
        Obstacles.SetActive(false);
        audioSource = audioSourceOrigin.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (spawn)
        {
            timer -= Time.deltaTime; // Count down to 0
            // Update the timer text on the screen
            UpdateTimerText();
            if(gamemodeType == "Flick Test" || gamemodeType == "Precision")
            {
                if(Time.time >= nextSpawnTime)
                {
                    SpawnTarget();
                    NoOfTargetOnMap += 1;
                    nextSpawnTime = Time.time + spawnInterval;
                }
            }
            if(gamemodeType == "Survival")
            {
                timePassed+= Time.deltaTime;
                if (maxNoOfTarget > NoOfTargetOnMap) // If spawning is true and Number of targets on the map is less than Number of targets allowed on the map
                 {
                    SpawnTarget();
                    NoOfTargetOnMap += 1;
                 }
            }
        }

        if (timer <= 0) //if countdown to 0
        {
            spawn = false;
            UpdateTimerDuration();
            UpdateTimerText();
            UpdateStartStopText();
            Options.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the game
            Application.Quit();
        }
        }
    public void pressStart() //When Start/Stop is presssed
    {
        spawn = !spawn; //change the spawning of target to on/off
        UpdateStartStopText();
        UpdateTimerDuration();
        UpdateTimerText();
        Options.SetActive(true);
        if (spawn) // if the game starting/restarting change the score text to 0;
        {
            score = 0;
            missed = 0;
            timePassed = 0;
            UpdateScoreText();
            UpdateMissedText();
            UpdateHitRateText();
            Options.SetActive(false);
        }
        else //Stopping the game
        {
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
        if (gamemodeType == "Survival")
        {
            missedText.text = "";
        }
        else
        {
            missedText.text = "Missed: " + missed; // Display missed score
        }
    }
    void UpdateGameModeText()
    {
        gamemodeText.text = "Game mode Selected: " + gamemodeType; // Display missed score

    }
    void UpdateHitRateText()
    {
        if (gamemodeType == "Survival")
        {
            if (spawn)
            {
                hitRateText.text = "";
            }
            else
            {
                hitRate = (score / (float)timePassed);
                if (hitRate > 0 && timePassed > 0)
                {
                    hitRateText.text = $"Target Hit Per Second: {hitRate:F2}";
                }
                else
                {
                    hitRateText.text = "Target Hit Per Second: 0";
                }
            }
        }
        else
        {
            int total = score + missed;
            if (total == 0)
            {
                hitRateText.text = "Hit Rate: 0.00%"; // Display hit rate
            }
            else
            {

                hitRate = (score / (float)total) * 100;
                hitRateText.text = $"Hit Rate: {hitRate:F2}%"; // Display hit rate
            }
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
        float xPos = Random.Range(xPosRange[0], xPosRange[1]);
        float zPos = Random.Range(zPosRange[0], zPosRange[1]);
        Vector3 randomPosition = new Vector3(xPos, 0, zPos);
        // Instantiate a new target at the spawn point
        Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
        GameObject newTarget = Instantiate(targetPrefab, randomPosition, targetRotation);

    }
    public void hitTarget()
    {
        audioSource.PlayOneShot(killEffect);
        score += 1;
        UpdateScoreText();
        UpdateHitRateText();
        NoOfTargetOnMap -= 1;
        if (gamemodeType == "Survival")
        {
            timer += 0.1f;
        }
    }
    public void missTarget()
    {
        if (gamemodeType != "Survival")
        {
            missed += 1;
            UpdateMissedText();
            UpdateHitRateText();
        }
        NoOfTargetOnMap -= 1;
    }
    public void changeTimer(string timing)
    {
        if (gamemodeType == "Survival")
        {
            timeSelectedText.text = "";
        }
        else
        {
            if (timing == "15Second")
            {
                setTime = 15f;
                Timer1Text.color = Color.blue;
                Timer2Text.color = Color.white;
                Timer3Text.color = Color.white;
                Timer4Text.color = Color.white;
                timeSelectedText.text = "Time Selected: 15 sec";
            }
            else if (timing == "30Second")
            {
                Timer1Text.color = Color.white;
                Timer2Text.color = Color.blue;
                Timer3Text.color = Color.white;
                Timer4Text.color = Color.white;
                setTime = 30f;
                timeSelectedText.text = "Time Selected: 30 sec";
            }
            else if (timing == "45Second")
            {
                Timer1Text.color = Color.white;
                Timer2Text.color = Color.white;
                Timer3Text.color = Color.blue;
                Timer4Text.color = Color.white;
                setTime = 45f;
                timeSelectedText.text = "Time Selected: 45 sec";
            }
            else if (timing == "60Second")
            {
                Timer1Text.color = Color.white;
                Timer2Text.color = Color.white;
                Timer3Text.color = Color.white;
                Timer4Text.color = Color.blue;
                setTime = 60f;
                timeSelectedText.text = "Time Selected: 60 sec";
            }
        }
    }
    public void displayTimerText(bool set)
    {
        if(set)
        {
            Timer1Text.text = "15 sec";
            Timer2Text.text = "30 sec";
            Timer3Text.text = "45 sec";
            Timer4Text.text = "60 sec";
        }
        else
        {
            Timer1Text.text = "";
            Timer2Text.text = "";
            Timer3Text.text = "";
            Timer4Text.text = "";

        }
    }
    public void gamemodeSelector(string gamemode)
    {
        gamemodeType = gamemode;
        UpdateGameModeText();
        if (gamemode == "Survival")
        {
            precisionText.color = Color.white;
            survivalText.color = Color.blue;
            flickText.color = Color.white;
            displayTimerText(false);
            Obstacles.SetActive(false);

            gamemodeTutorialText.text = "In Survival mode, 2 targets are always visible within the range. Players have 30 seconds to shoot as many targets as possible, with each kill adding 0.1 seconds to the timer for extended playtime.";
            changeTimer("");
            UpdateMissedText();
            UpdateHitRateText();
            xPosRange[0] = -4f;
            xPosRange[1] = 4f;
            zPosRange[0] = -6f;
            zPosRange[1] = 9f;
            setTime = 30f;
            
        }
        else if (gamemode == "Precision")
        {
            precisionText.color = Color.blue;
            survivalText.color = Color.white;
            flickText.color = Color.white;
            displayTimerText(true);
            gamemodeTutorialText.text = "In Precision mode, obstacles are placed to partially obscure the targets, so only a portion of each target is visible. Targets only spawn at the back of the range.";
            Obstacles.SetActive(true);
            changeTimer(setTime.ToString() + "Second");
            UpdateMissedText();
            UpdateHitRateText();
            xPosRange[0] = -4f;
            xPosRange[1] = 4f;
            zPosRange[0] = 8f;
            zPosRange[1] = 9f;
        }
        else if (gamemode == "Flick Test")
        {
            precisionText.color = Color.white;
            survivalText.color = Color.white;
            flickText.color = Color.blue;
            displayTimerText(true);
            gamemodeTutorialText.text = "In Flick Test mode, targets appear and disappear within 0.5 seconds and spawn randomly across the entire available range.";
            Obstacles.SetActive(false);
            changeTimer(setTime.ToString() + "Second");
            UpdateMissedText();
            UpdateHitRateText();
            xPosRange[0] = -4f;
            xPosRange[1] = 4f;
            zPosRange[0] = -6f;
            zPosRange[1] = 9f;
        }
    }
}
