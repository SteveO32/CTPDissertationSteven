using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Lives/Ammo Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class LivesOrAmmo
    {
        public static LivesOrAmmo instance;
        private int initialLivesAmmo;
        private int numLivesAmmo;
        private int maxLivesAmmo;
        private List<GameObject> livesAmmoList;
        GameObject livesAmmoImage;
        Vector3 pos;
        UIManager ui_manager;

        public bool takenDamage;
        public bool addHealth;

        // Use this for initialization
        void Start()
        {

        }

        public LivesOrAmmo(int startValue, UIManager manager)
        {
            instance = this;
            livesAmmoList = new List<GameObject>();
            maxLivesAmmo = 11;
            initialLivesAmmo = startValue;
            pos = new Vector3(26.6f, 34.0f, 0.0f);
            ui_manager = manager;

        }

        public void InitTestMode()
        {
            initialLivesAmmo = 3;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int GetLives()
        {
            return numLivesAmmo;
        }

        public void ChangeLives(int valueToChangeBy)
        {
            numLivesAmmo += valueToChangeBy;
            if (numLivesAmmo > maxLivesAmmo)
            {
                numLivesAmmo = maxLivesAmmo;
            }
        }

        public void LivesData()
        {
            int temp = 0;
            foreach (GameObject lives in livesAmmoList)
            {
                if (lives.activeSelf)
                {
                    temp++;
                }
            }

            if (numLivesAmmo > 0)
            {              
                if (temp != numLivesAmmo)
                {
                    if (temp < numLivesAmmo)
                    {
                        for (int i = 0; i < numLivesAmmo; i++)
                        {
                            livesAmmoList[i].SetActive(true);
                        }
                    }
                    else if (temp > numLivesAmmo)
                    {
                        for (int i = 0; i < livesAmmoList.Count; i++)
                        {
                            livesAmmoList[i].SetActive(false);
                        }
                    }
                }
            } 
        }

        public void SetUpElement(GameObject prefab)
        {
            for (int i = 0; i < maxLivesAmmo; i++)
            {
                livesAmmoImage = MonoBehaviour.Instantiate(prefab, pos, Quaternion.identity) as GameObject;
                livesAmmoImage.transform.localPosition = pos;
                livesAmmoImage.GetComponent<RectTransform>().anchoredPosition3D = pos;
                livesAmmoImage.transform.SetParent(ui_manager.transform);
                livesAmmoList.Add(livesAmmoImage);

                livesAmmoImage.transform.localScale = Vector3.one;
                livesAmmoImage.transform.localEulerAngles = Vector3.zero;
                livesAmmoImage.GetComponent<RectTransform>().anchoredPosition3D = pos;
                pos.x += livesAmmoImage.transform.GetComponent<Image>().rectTransform.rect.width;
            }

            foreach(GameObject lives in livesAmmoList)
            {
                lives.SetActive(false);
            }

            numLivesAmmo = initialLivesAmmo;
        }
    }
}