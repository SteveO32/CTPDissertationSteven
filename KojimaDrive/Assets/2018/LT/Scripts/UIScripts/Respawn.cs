using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Respawn Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class Respawn
    {
        public static Respawn instance;
        private Text respawnText;
        private int startValue;
        private int respawnStartValue;
        GameObject respawnObject;
        UIManager ui_manager;

        float respawnTimerValue;
        float respawnSecondsValue;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Respawn(int respawnInitialValue, UIManager manager)
        {
            instance = this;
            respawnStartValue = respawnInitialValue + 1;
            ui_manager = manager;
        }

        public void InitTestMode()
        {
            respawnStartValue = 5;
        }

        public void RespawnData()
        {
            respawnTimerValue = (startValue - Time.time);
            respawnSecondsValue = (int)respawnTimerValue;

            if (respawnSecondsValue == 0)
            {
                respawnText.gameObject.SetActive(false);
                // respawn player -- instansiate prefab or set coordinates?
            }
            else
            {
                respawnText.text = "Respawn in: " + respawnSecondsValue;
            }
        }

        public void SetUpElement(GameObject prefab)
        {
            respawnObject = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;
            respawnObject.transform.SetParent(ui_manager.transform);
            respawnObject.transform.localPosition = new Vector2(0.0f, 0.0f);
            respawnText = respawnObject.GetComponent<Text>();
        }
    }
}