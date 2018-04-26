using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Threading;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI System
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class UIManager : MonoBehaviour
    {
        // UI Manager Variables
        public UIManager UI_instance;
        public List<Canvas> canvases;

        public bool testMode;
        public bool arcadeMode;
        public bool singleCameraMode;
        public int numberOfPlayers;
        private static int playerID;

        private bool ranOnce;
        public readonly string elementAssetPath = "UI/Elements/";

        // Canvas Elements
        public bool addTimer;
        public bool addScore;
        public int initialScore;

        public bool addLivesOrAmmo;
        public int startingLivesAmmo;
        public bool addHealthBar;
        public bool addPowerBar;

        public bool addWarning;
        public bool warningWithImage;
        public bool addCountdown;
        public int countdownstartValue;
        private bool addRespawn; // still in development
        private int respawnStartValue; // still in development
        public bool addPlacement;

        private bool addPauseMenu; // still in development
        private bool addLoseScreen; // still in development
        private bool addWinScreen; // still in development

        // Menu Variables
        private GameObject pauseMenuPrefab;
        public Menu menu_reference {  get; private set; }

        // Timer Variables;
        private GameObject timerPrefab;
        public Timer timer_reference { get; private set; }

        // Health Variables
        private GameObject healthBarPrefab;
        public HealthBar healthBar_reference { get; private set; }

        // Ammo Variables
        private GameObject livesImagePrefab;
        public LivesOrAmmo livesAmmoImage_reference { get; private set; }

        // Power Variables
        private GameObject powerBarPrefab;
        public PowerBar powerBar_reference { get; private set; }

        // Score Variables
        private GameObject scorePrefab;
        public Score score_reference { get; private set; }

        // Single Camera Score Variables
        private GameObject sc_scoreboardPrefab;
        public ScoreboardManager sc_score_reference { get; private set; }

        // Warning Text Variables
        private GameObject warningTextPrefab;
        public WarningText warningText_reference { get; private set; }

        // Countdown Text Variables
        private GameObject countdownTextPrefab;
        public Countdown countdown_reference { get; private set; }

        // Respawn Text Variables
        private GameObject respawnTextPrefab;
        public Respawn respawn_reference { get; private set; }

        // Placement Text Variables
        private GameObject placementTextPrefab;
        public PlacementText placementText_reference { get; private set; }

        // Lose Screen Variables
        private GameObject loseScreenPrefab;
        public LoseScreen loseScreen_reference { get; private set; }

        // Win Screen Variables
        private GameObject winScreenPrefab;
        public WinScreen winScreen_reference { get; private set; }

        // Use this for initialization
        void Start()
        {
            if (!UI_instance)
                UI_instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateElements();
        }

        public void SetUpUIManager()
        {
            if (!UI_instance)
                UI_instance = this;

            this.gameObject.tag = "UI Canvas";

            CreateReferences();
            InitialiseTestModes();
            CreateCanvases();
            CreateElements();
            CreateUI();
        }

        void CreateCanvases()
        {
            canvases = new List<Canvas>();
            for (int i = 0; i < numberOfPlayers; i++)
            {
                GameObject[] temp = GameObject.FindGameObjectsWithTag("UI Canvas");
                canvases.Add(temp[i].GetComponent<Canvas>());

                if (playerID <= 0)
                {
                    countdown_reference.SetUpCanvasWithID(canvases[0], playerID);
                    playerID++;
                }
            }
        }

        private void CreateReferences()
        {
            menu_reference = new Menu(this);        
            score_reference = new Score(initialScore, this);
            timer_reference = new Timer(this);
            powerBar_reference = new PowerBar(this);
            healthBar_reference = new HealthBar(this);
            winScreen_reference = new WinScreen(this);
            loseScreen_reference = new LoseScreen(this);
            warningText_reference = new WarningText(this, warningWithImage);
            sc_score_reference = new ScoreboardManager(this);
            countdown_reference = new Countdown(countdownstartValue, this);
            respawn_reference = new Respawn(respawnStartValue, this);
            livesAmmoImage_reference = new LivesOrAmmo(startingLivesAmmo, this);
            placementText_reference = new PlacementText(numberOfPlayers, this);
        }

        private void InitialiseTestModes()
        {
            if (testMode)
            {
                menu_reference.InitTestMode();
                score_reference.InitTestMode();
                timer_reference.InitTestMode();
                powerBar_reference.InitTestMode();
                healthBar_reference.InitTestMode();
                winScreen_reference.InitTestMode();
                loseScreen_reference.InitTestMode();
                sc_score_reference.InitTestMode();
                respawn_reference.InitTestMode();
                livesAmmoImage_reference.InitTestMode();
                placementText_reference.InitTestMode();
            }
        }

        private void CreateUI()
        {
            CreateScore();
            CreateTimer();
            CreatePowerBar();
            CreateHealthBar();
            CreatePauseMenu();
            CreateWinScreen();
            CreateLivesImage();
            CreateLoseScreen();
            CreateWarningText();
            CreateSCScoreboard();
            CreateRespawnTimer();
            CreatePlacementText();
            CreateCountdownTimer();
        }

        private void CreateElements()
        {
            timerPrefab = (GameObject)Resources.Load(elementAssetPath + "Timer Text");
            scorePrefab = (GameObject)Resources.Load(elementAssetPath + "Score Text");
            sc_scoreboardPrefab = (GameObject)Resources.Load(elementAssetPath + "Scoreboard");
            powerBarPrefab = (GameObject)Resources.Load(elementAssetPath + "Power Bar");
            healthBarPrefab = (GameObject)Resources.Load(elementAssetPath + "Health Bar");
            winScreenPrefab = (GameObject)Resources.Load(elementAssetPath + "Win Screen");
            livesImagePrefab = (GameObject)Resources.Load(elementAssetPath + "Lives Image");
            warningTextPrefab = (GameObject)Resources.Load(elementAssetPath + "Warning Text");
            respawnTextPrefab = (GameObject)Resources.Load(elementAssetPath + "Respawn Text");
            loseScreenPrefab = (GameObject)Resources.Load(elementAssetPath + "Lose_Death Screen");
            placementTextPrefab = (GameObject)Resources.Load(elementAssetPath + "Placement Text");
            countdownTextPrefab = (GameObject)Resources.Load(elementAssetPath + "Countdown Text");
            pauseMenuPrefab = (GameObject)Resources.Load(elementAssetPath + "Menu/Menu Prefabs/Menu Prefab");
        }

        public void SetNumberOfPlayers(int num)
        {
            numberOfPlayers = num;
        }

        public void SetStartingLives(int num)
        {
            numberOfPlayers = num;
        }

        public void PlayCountdownSound()
        {
            StartCoroutine(countdown_reference.CountSound());
        }

        public void PlayCountdownStartSound()
        {
            StartCoroutine(countdown_reference.StartSound());
            StopCountdownSounds();
        }

        public void StopCountdownSounds()
        {
            StopCoroutine(countdown_reference.CountSound());
            StopCoroutine(countdown_reference.StartSound());
        }

        public void ShowWarningText()
        {
            StartCoroutine(warningText_reference.Flash());
        }

        public void HideWarningText()
        {
            StopCoroutine(warningText_reference.Flash());
        }

        private void CreateHealthBar()
        {
            if (addHealthBar)
            {
                try
                {
                    healthBar_reference.SetUpElement(healthBarPrefab);
                }
                catch
                {
                    Debug.Log("Health Bar has bugged");
                }
            }
        }

        private void CreateLoseScreen()
        {
            if (addLoseScreen)
            {
                try
                {
                    loseScreen_reference.SetUpElement(loseScreenPrefab);
                }
                catch
                {
                    Debug.Log("Health Bar has bugged");
                }
            }
        }

        private void CreateWinScreen()
        {
            if (addWinScreen)
            {
                try
                {
                    winScreen_reference.SetUpElement(winScreenPrefab);
                }
                catch
                {
                    Debug.Log("Health Bar has bugged");
                }
            }
        }

        private void CreateLivesImage()
        {
            if (addLivesOrAmmo)
            {
                try
                {
                    livesAmmoImage_reference.SetUpElement(livesImagePrefab);
                }
                catch
                {
                    Debug.Log("Lives image has bugged");
                }
            }
        }

        private void CreatePlacementText()
        {
            if (addPlacement)
            {
                try
                {
                    placementText_reference.SetUpCanvases();
                    placementText_reference.SetUpElement(placementTextPrefab);
                }
                catch
                {
                    Debug.Log("Placement text has bugged");
                }
            }
        }

        private void CreateWarningText()
        {
            if (addWarning)
            {
                try
                {
                    warningText_reference.SetUpElement(warningTextPrefab);
                    if (testMode)
                    {
                        warningText_reference.InitTestMode();
                    }
                }
                catch
                {
                    Debug.Log("Warning text has bugged");
                }
            }
        }

        private void CreateTimer()
        {
            if (addTimer)
            {
                try
                {
                    timer_reference.SetUpElement(timerPrefab);
                }
                catch
                {
                    Debug.Log("Timer has bugged");
                }
            }
        }

        private void CreatePowerBar()
        {
            if (addPowerBar)
            {
                try
                {
                    powerBar_reference.SetUpElement(powerBarPrefab);
                }
                catch
                {
                    Debug.Log("Power Bar has bugged");
                }
            }
        }

        private void CreateScore()
        {
            if (addScore)
            {
                try
                {
                    score_reference.SetUpElement(scorePrefab);
                }
                catch
                {
                    Debug.Log("Score has bugged");
                }
            }
        }

        private void CreateSCScoreboard()
        {
            if (singleCameraMode)
            {
                try
                {
                    sc_score_reference.SetUpCanvases();
                    sc_score_reference.SetUpElement(sc_scoreboardPrefab);
                }
                catch
                {
                    Debug.Log("Single Camera Score has bugged");
                }
            }
        }

        private void CreateCountdownTimer()
        {
            if (addCountdown)
            {
                try
                {
                    countdown_reference.SetUpElement(countdownTextPrefab);
                    if (testMode)
                    {
                        countdown_reference.InitTestMode();
                    }
                }
                catch
                {
                    Debug.Log("Countdown has bugged");
                }
            }
        }

        private void CreateRespawnTimer()
        {
            if (addRespawn)
            {
                try
                {
                    respawn_reference.SetUpElement(respawnTextPrefab);
                }
                catch
                {
                    Debug.Log("Respawn has bugged");
                }
            }
        }

        private void CreatePauseMenu()
        {
            if (addPauseMenu)
            {
                try
                {
                    menu_reference.SetUpElement(pauseMenuPrefab);
                }
                catch
                {
                    Debug.Log("Pause Menu has bugged");
                }
            }
        }

        private void UpdateElements()
        {
            if (addCountdown)
            {
                countdown_reference.CountdownData();
            }
            if (addRespawn)
            {
                respawn_reference.RespawnData();
            }
            if (addHealthBar)
            {
                healthBar_reference.HealthBarData();
            }
            if (addLivesOrAmmo)
            {
                livesAmmoImage_reference.LivesData();
            }
            if (addPauseMenu)
            {
                menu_reference.PauseMenuData();
            }
            if (addPlacement)
            {
                placementText_reference.PlacementTextData();
            }
            if (addPowerBar)
            {
                powerBar_reference.PowerBarData();
            }
            if (addScore)
            {
                score_reference.ScoreData();
            }
            if (addTimer)
            {
                timer_reference.TimerData();
            }

            if(singleCameraMode)
            {
                sc_score_reference.ScoreManagerData();
            }

            if (addWarning)
            {
                warningText_reference.WarningTextData();
            }
        }
    }
}