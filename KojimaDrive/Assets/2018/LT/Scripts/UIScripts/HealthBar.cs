using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Health Bar Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class HealthBar
    {
        public static HealthBar instance;
        private float healthValue;
        GameObject healthBarObject;
        UIManager ui_manager;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public HealthBar(UIManager manager)
        {
            instance = this;
            ui_manager = manager;
        }

        public float GetCurrentHealth()
        {
            return healthValue;
        }

        public void InitTestMode()
        {
            healthValue = 50.0f;
        }

        public void ChangeHealthValueBy(float healthToAdd)
        {
            healthValue += healthToAdd;
        }

        public void SetHealthValue(float valueEquals)
        {
            healthValue = valueEquals;
        }

        public void HealthBarData()
        {
            healthBarObject.GetComponent<Slider>().value = healthValue;
        }

        public void SetUpElement(GameObject prefab)
        {
            healthBarObject = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;
            healthBarObject.transform.SetParent(ui_manager.transform);

            healthBarObject.transform.localScale = Vector3.one;
            healthBarObject.transform.localEulerAngles = Vector3.zero;
            healthBarObject.transform.localPosition = Vector3.zero;
            healthBarObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
        }
    }
}