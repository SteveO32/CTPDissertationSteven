using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;

public class UIManager_Copy : MonoBehaviour {

    // UI Manager Variables

    public bool isActive;
    public static UIManager_Copy instance;

    // Menu Variables
    public GameObject pauseMenuObject;

    // Timer Variables;
    private float startTime;
    private string minutes;
    private string seconds;
    private bool timerArcadeMode;

    // Health Variables
    private float health;
    private int lives;

    // Power Variables
    private float powerValue;

    // Score Variables
    private int score;
    private bool scoreArcadeMode;

    // Warning Text Variables
    private Text warningText;

    // Countdown Text Variables
    private Text countdownText;

    // Placement Text Variables
    private Text placementText;

    // Aircraft Variables
    public GameObject player;
    public GameObject gyroscope;
    public Text altimeter;

    // Use this for initialization
    void Start () {
        instance = this;
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {

        // Gyroscope
        float euler_z = player.transform.eulerAngles.z;
        gyroscope.transform.eulerAngles = new Vector3(gyroscope.transform.eulerAngles.x, gyroscope.transform.eulerAngles.y, euler_z);

        // Altimeter
        float position_y = Mathf.Round(player.transform.position.y);
        altimeter.text = position_y.ToString() + " ft";

        // Pause Menu
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    public float HealthAsFloat(float currentHealth, bool takenDamage, float damageAmount)
    {
        if(takenDamage)
        {
            health -= (currentHealth - damageAmount);
        }

        currentHealth = health;

        return currentHealth;
    }

    public float HealthAsInt(bool takenDamage, int currentLives)
    {
        lives = currentLives;

        if (takenDamage)
        {
            lives -= 1;
        }

        currentLives = lives;

        return currentLives;
    }

    public Text Timer(float timerData, Text timerText, bool isArcade)
    {
        timerArcadeMode = isArcade;
        float timerValue = (Time.time - startTime);

        if (timerArcadeMode)
        {
            seconds = ((int)timerValue % 60).ToString();
            timerText.text = seconds;
        }
        else
        {
            minutes = ((int)timerValue / 60).ToString();
            seconds = (timerValue % 60).ToString("f2");
            timerText.text = minutes + "." + seconds;
        }
        timerData = timerValue;

        return timerText;
    }

    public Slider PowerBar(Slider pwrBar, float sliderValue)
    {
        powerValue = sliderValue;
        pwrBar.value = powerValue;

        return pwrBar;
    }

    public int Score(int currentScore, int scoreToAdd)
    {
        score = currentScore;
        score += scoreToAdd;

        currentScore = score;

        return currentScore;
    }

    public Text CountdownTimer(Text countdownText, float countdownStartValue)
    {
        float timerValue = (countdownStartValue - Time.time);
        int secondsValue = (int)timerValue % 60;

        if (secondsValue <= 0)
        {
            countdownText.text = "Start!";
        }
        else
        {

            countdownText.text = secondsValue.ToString();
        }

        return countdownText;
    }

    public void PauseMenu()
    {
        if (pauseMenuObject.gameObject.activeSelf)
        {
            pauseMenuObject.gameObject.SetActive(false);
           // GameManager.isGamePaused = false;
        }
        else
        {
            pauseMenuObject.gameObject.SetActive(true);
          // GameManager.isGamePaused = true;
        }
    }
}
