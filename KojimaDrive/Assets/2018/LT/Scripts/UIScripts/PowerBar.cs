using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Power Bar Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class PowerBar
    {
        private float powerValue;
        GameObject powerBarObject;
        public static PowerBar instance;
        UIManager ui_manager;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public PowerBar(UIManager manager)
        {
            instance = this;
            powerValue = 0;
            ui_manager = manager;
        }

        public void InitTestMode()
        {
            powerValue = 50.0f;
        }

        public float GetCurrentPower()
        {
            return powerValue;
        }

        public void ChangePowerValueBy(float powerChangeBy)
        {
            powerValue += powerChangeBy;
        }

        public void SetPowerValue(float valueEquals)
        {
            powerValue = valueEquals;
        }

        public void PowerBarData()
        {
            powerBarObject.GetComponent<Slider>().value = powerValue;
        }

        public void SetUpElement(GameObject prefab)
        {
            powerBarObject = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;
            powerBarObject.transform.SetParent(ui_manager.transform);

            powerBarObject.transform.localScale = Vector3.one;
            powerBarObject.transform.localEulerAngles = new Vector3();
            powerBarObject.transform.localPosition = Vector2.one;
            powerBarObject.GetComponent<RectTransform>().transform.Rotate(0.0f, 0.0f, 90.0f);
            powerBarObject.GetComponent<RectTransform>().localPosition = new Vector2(400.0f, 0.0f);
            powerBarObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 300.0f);
        }
    }

}