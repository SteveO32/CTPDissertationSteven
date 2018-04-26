using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Score Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class Score
    {
        public static Score instance;
        private int score = 0;
        private int startValue;
        private bool scoreArcadeMode;
        GameObject scoreObject;
        UIManager ui_manager;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public Score(int initValue, UIManager manager)
        {
            instance = this;
            startValue = initValue;
            ui_manager = manager;
        }

        public void InitTestMode()
        {
            startValue = 999;
        }

        public int GetScore()
        {
            return score;
        }

        public void ChangeScore(int changeValue, int playerID)
        {
            ui_manager.canvases[playerID].GetComponent<UIManager>().score_reference.score += changeValue;
        }

        public void ScoreData()
        {
            scoreObject.GetComponent<Text>().text = score.ToString();
        }

        public void SetUpElement(GameObject prefab)
        {
            score = startValue;
            scoreObject = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;
            scoreObject.transform.SetParent(ui_manager.transform);

            scoreObject.transform.localScale = Vector3.one;
            scoreObject.transform.localEulerAngles = Vector3.zero;
            scoreObject.transform.localPosition = Vector2.one;
            scoreObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);
        }
    }
}
