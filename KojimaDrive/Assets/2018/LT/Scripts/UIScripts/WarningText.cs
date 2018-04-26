using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Warning Text Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class WarningText
    {
        public static WarningText instance;
        private Text warningText;
        private Image buttonImage;
        private float flashInterval;
        private string warningString;
        private bool flashText;
        private bool activate;
        private bool isRunning;
        private bool hasImage;
        GameObject warningTextObject;
        UIManager ui_manager;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ActivateWarning()
        {
            if(!activate)
            {
                activate = true;
                ActivateWarningText();
            }
        }

        public void DeactivateWarning()
        {
            if(activate)
            {
                activate = false;
                ActivateWarningText();
            }
        }

        private void ActivateWarningText()
        {
            if (activate)
            {
                flashText = true;
                warningText.gameObject.SetActive(true);
                buttonImage.gameObject.SetActive(true);
                ui_manager.ShowWarningText();
            }
            else
            {
                flashText = false;
                warningText.gameObject.SetActive(false);
                buttonImage.gameObject.SetActive(false);
            }
        }

        public WarningText(UIManager manager, bool button)
        {
            instance = this;
            flashInterval = 0.5f;
            activate = false;
            warningString = "Leaving Area!";
            ui_manager = manager;
            hasImage = button;
        }

        public void SetWarningText(string newString)
        {
            warningString = newString;
            warningTextObject.GetComponent<Text>().text = warningString;
        }

        public bool IsFlashing()
        {
            return activate;
        }

        public float GetInterval()
        {
            return flashInterval;
        }

        public string GetWarningText()
        {
            return warningString;
        }

        public void SetFlashInterval(float newInerval)
        {
            flashInterval = newInerval;
        }

        public void WarningTextData()
        {
            //Debug.Log(activate);
        }

        public void InitTestMode()
        {
            if (warningTextObject != null)
            {
                ui_manager.ShowWarningText();
            }
        }

        public void SetUpElement(GameObject prefab)
        {
            warningTextObject = MonoBehaviour.Instantiate(prefab, new Vector2(-6.0f, 0.0f), Quaternion.identity) as GameObject;
            warningTextObject.transform.SetParent(ui_manager.transform);
            warningText = warningTextObject.GetComponent<Text>();
            buttonImage = warningTextObject.GetComponentInChildren<Image>();
            warningTextObject.transform.localScale = Vector3.one;
            warningTextObject.transform.localEulerAngles = Vector3.zero;
            warningTextObject.transform.localPosition = Vector3.zero;
            warningTextObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            flashText = true;
            warningText.gameObject.SetActive(false);
            buttonImage.gameObject.SetActive(false);
        }

        public IEnumerator Flash()
        {
            while (flashText)
            {
                warningText.text = "";

                if(hasImage)
                {
                    buttonImage.gameObject.SetActive(false);
                }
                yield return new WaitForSeconds(flashInterval);

                warningText.text = warningString;
                if (hasImage)
                {
                    buttonImage.gameObject.SetActive(true);
                }
                yield return new WaitForSeconds(flashInterval);
            }
        }
    }
}
