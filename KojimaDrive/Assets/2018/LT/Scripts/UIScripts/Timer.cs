using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Timer Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class Timer
    {
        public Timer instance;
        private Text timerTextObject;
        UIManager ui_manager;
        private string currentTimerValue;

        private float startTime;
        private string minutes;
        private string seconds;
        private bool timerArcadeMode;

        // Use this for initialization
        void Start()
        {

        }

        public Timer(UIManager manager)
        {
            instance = this;
            ui_manager = manager;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitTestMode()
        {
            if (timerTextObject != null)
            {

            }
        }

        public string GetTimerValue()
        {
            return currentTimerValue;
        }

        public void SetTimerValue(int valueEquals)
        {
            float temp = valueEquals;

            if (timerArcadeMode)
            {
                seconds = ((int)temp).ToString();
                timerTextObject.text = seconds;
            }
            else
            {
                minutes = ((int)temp / 60).ToString();
                seconds = (temp % 60).ToString("f0");
                timerTextObject.text = minutes + ":" + seconds;

                currentTimerValue = minutes + "." + seconds;
            }
        }

        public void TimerData()
        {
            float timerValue = (Time.time - startTime);

            if (timerArcadeMode)
            {
                seconds = ((int)timerValue).ToString();
                timerTextObject.text = seconds;
            }
            else
            {
                minutes = ((int)timerValue / 60).ToString();
                seconds = (timerValue % 60).ToString("f0");
                timerTextObject.text = minutes + ":" + seconds;
            }

            currentTimerValue = minutes + "." + seconds;
        }

        public void SetUpElement(GameObject prefab)
        {
            instance = this;
            startTime = Time.time;
            timerArcadeMode = ui_manager.arcadeMode;

            GameObject timerObject = MonoBehaviour.Instantiate(prefab, new Vector2(-6.0f, 0.0f), Quaternion.identity) as GameObject;
            timerTextObject = timerObject.GetComponent<Text>();
            timerObject.GetComponent<RectTransform>().anchorMin = new Vector2(1.0f, 1.0f);
            timerObject.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 1.0f);
            timerObject.transform.SetParent(ui_manager.transform);

            timerTextObject.transform.localScale = Vector3.one;
            timerTextObject.transform.localEulerAngles = Vector3.zero;
            timerTextObject.transform.localPosition = Vector3.zero;
            timerTextObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-6.0f, 0.0f);
        }
    }
}
