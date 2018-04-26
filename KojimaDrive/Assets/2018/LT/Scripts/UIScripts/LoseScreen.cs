using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Lose Screen Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class LoseScreen
    {
        GameObject loseScreenObject;
        public static LoseScreen instance;
        Text loseScreenText;
        UIManager ui_manager;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public LoseScreen(UIManager manager)
        {
            instance = this;
            ui_manager = manager;
        }

        public void InitTestMode()
        {
            if (loseScreenObject != null)
            {

            }
        }

        public void LoseScreenData()
        {
            RectTransform rt = ui_manager.GetComponent(typeof(RectTransform)) as RectTransform;
            loseScreenObject.GetComponent<RectTransform>().sizeDelta = rt.sizeDelta;
        }

        public void SetUpElement(GameObject prefab)
        {
            loseScreenObject = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;
            loseScreenObject.transform.SetParent(ui_manager.transform);
            loseScreenObject.transform.localPosition = new Vector2(0.0f, 0.0f);
            loseScreenText = loseScreenObject.GetComponent<Text>();
        }
    }
}