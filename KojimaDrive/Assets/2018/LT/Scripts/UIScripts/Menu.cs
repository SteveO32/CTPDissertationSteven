using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Menu Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class Menu
    {
        public static Menu instance;
        public bool isActive;
        private GameObject menuObject;
        UIManager ui_manager;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenuData();
            }
        }

        public Menu(UIManager manager)
        {
            instance = this;
            ui_manager = manager;
        }

        public void InitTestMode()
        {
            if (menuObject != null)
            {

            }
        }

        public void ChangeMenuState()
        {
            if (menuObject.gameObject.activeSelf)
            {
                menuObject.gameObject.SetActive(false);
                // GameManager.isGamePaused = false;
            }
            else
            {
                menuObject.gameObject.SetActive(true);
                // GameManager.isGamePaused = true;
            }
        }

        public void PauseMenuData()
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeMenuState();
            }
        }

        public void SetUpElement(GameObject prefab)
        {
            menuObject = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;

            menuObject.transform.localScale = Vector3.one;
            menuObject.transform.localEulerAngles = Vector3.zero;
            menuObject.transform.localPosition = Vector2.one;
        }
    }

}