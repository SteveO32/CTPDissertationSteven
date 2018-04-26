using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Countdown Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class Countdown
    {
        private Text countdownText;
        private float counter;
        private int soundID;
        public int initialValue;
        private bool startTimer;
        private bool startGame;
        private bool hasEnded;
        GameObject countdownObject;
        UIManager ui_manager;
        public static Countdown instance;
        private static Canvas player1Canvas;
        private int passNum;

        private AudioSource audioElement;
        private AudioClip beep;
        private AudioClip longBeep;

        float timerValue;
        float secondsValue;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool BeginGame()
        {
            return startGame;
        }

        public void SetUpCanvasWithID(Canvas canvasOne, int ID)
        {
            player1Canvas = canvasOne;
            soundID = ID;
        }

        public void ActivateCountdown()
        {
            startTimer = true;
            startGame = false;
        }

        public void SetCustomSounds(AudioClip countClip, AudioClip startClip)
        {
            beep = countClip;
            longBeep = startClip;
        }

        public Countdown(int countdownInitialValue, UIManager manager)
        {
            instance = this;
            startGame = false;
            hasEnded = false;
            initialValue = 0;
            initialValue = countdownInitialValue;
            passNum = 0;
            ui_manager = manager;
        }

        public void InitTestMode()
        {
            initialValue = 4;
            startTimer = true;
        }

        private void ResetCountdown()
        {
            timerValue = 0;
            secondsValue = 0;
            counter = 0;
            startTimer = false;
            hasEnded = false;
            countdownText.gameObject.SetActive(false);
        }

        public void CountdownData()
        {
            if (startTimer)
            {
                countdownText.gameObject.SetActive(true);
                counter += Time.deltaTime;
                timerValue = ((initialValue + 1) - counter);
                secondsValue = (int)timerValue % 60;

                if (secondsValue == 0 && hasEnded == false)
                {
                    countdownText.text = "Start!";
                    if (soundID == 0 && !audioElement.isPlaying)
                    {
                        ui_manager.PlayCountdownStartSound();
                        ui_manager.StopCountdownSounds();
                    }
                   // hasEnded = true;                
                }
                else if (secondsValue < 0)
                {
                    if (passNum == 4)
                    {
                        startTimer = false;
                        startGame = true;
                        ResetCountdown();
                    }                                                                          
                    passNum++;                                                                 
                }                                                                              
                else if (secondsValue > 0)                                                     
                {                                                                              
                    countdownText.gameObject.SetActive(true);                                  
                    countdownText.text = secondsValue.ToString();                              
                    if (soundID == 0 && !audioElement.isPlaying)                               
                    {
                        ui_manager.PlayCountdownSound();                                 
                    }                                                                          
                }                                                                              
            }                                                                                  
        }                                                                                      
                                                                                               
        public void SetUpElement(GameObject prefab)
        {
            countdownObject = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;
            countdownObject.transform.SetParent(ui_manager.transform);
            countdownText = countdownObject.GetComponent<Text>();
            audioElement = countdownObject.GetComponent<AudioSource>();

            countdownText.transform.localScale = Vector3.one;
            countdownText.transform.localEulerAngles = Vector3.zero;
            countdownText.transform.localPosition = Vector3.zero;
            countdownText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
            countdownText.gameObject.SetActive(false);
            longBeep = (AudioClip)Resources.Load(ui_manager.elementAssetPath + "Audio/Race Countdown - Beep Long");
            beep = (AudioClip)Resources.Load(ui_manager.elementAssetPath + "Audio/Race Countdown - Beep");
        }

        public IEnumerator CountSound()
        {
            if (soundID == 0 && !audioElement.isPlaying)
            {
                for (int i = 0; i < initialValue; i++)
                {
                    audioElement.PlayOneShot(beep, 0.1f);
                    yield return new WaitForSeconds(beep.length);
                }
                ui_manager.StopCountdownSounds();
            }         
        }

        public IEnumerator StartSound()
        {
            if (soundID == 0 && !audioElement.isPlaying)
            {
                audioElement.PlayOneShot(longBeep, 0.1f);
                yield return new WaitForSeconds(longBeep.length);
                hasEnded = true;
            }
        }
    }
}