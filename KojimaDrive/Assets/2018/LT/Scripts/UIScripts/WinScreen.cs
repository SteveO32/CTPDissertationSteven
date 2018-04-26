using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Win Screen Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class WinScreen
    {
        GameObject winScreenObject;
        public static WinScreen instance;
        Text winScreenText;
        UIManager ui_manager;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public WinScreen(UIManager manager)
        {
            instance = this;
            ui_manager = manager;
        }

        public void InitTestMode()
        {
            if (winScreenObject != null)
            {

            }
        }

        public void WinScreenData()
        {
            RectTransform rt = ui_manager.GetComponent(typeof(RectTransform)) as RectTransform;
            winScreenObject.GetComponent<RectTransform>().sizeDelta = rt.sizeDelta;
        }

        public void SetUpElement(GameObject prefab)
        {
            winScreenObject = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;
            winScreenObject.transform.SetParent(ui_manager.transform);
            winScreenObject.transform.localPosition = new Vector2(0.0f, 0.0f);
            winScreenText = winScreenObject.GetComponent<Text>();
        }
    }
}