using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Scoreboard Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class ScoreboardManager
    {
        private GameObject scoreboard;
        private List<int> scoreValues;
        private List<Text> scoreTextObjects;
        private static ScoreboardManager instance;
        private List<Canvas> playerCanvases = new List<Canvas>();
        private UIManager ui_manager;

        int player1_score;
        int player2_score;
        int player3_score;
        int player4_score;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public ScoreboardManager(UIManager manager)
        {
            instance = this;

            scoreValues = new List<int>();
            scoreTextObjects = new List<Text>();
            playerCanvases = new List<Canvas>();
            ui_manager = manager;
        }

        public void InitTestMode()
        {
            if (scoreboard != null)
            {

            }
        }

        public void SetUpCanvases()
        {
            playerCanvases = ui_manager.canvases;
        }

        public int GetScore(int player_id)
        {
            return scoreValues[player_id];
        }

        public void ChangeScore(int changeValue, int player_id)
        {
            scoreValues[player_id] += changeValue;
        }

        public void ScoreManagerData()
        {
            scoreboard.GetComponent<RectTransform>().localScale = Vector3.one;

            player1_score = playerCanvases[0].GetComponent<UIManager>().score_reference.GetScore();
            player2_score = playerCanvases[1].GetComponent<UIManager>().score_reference.GetScore();
            player3_score = playerCanvases[2].GetComponent<UIManager>().score_reference.GetScore();
            player4_score = playerCanvases[3].GetComponent<UIManager>().score_reference.GetScore();

            scoreValues[0] = player1_score;
            scoreValues[1] = player2_score;
            scoreValues[2] = player3_score;
            scoreValues[3] = player4_score;

            scoreValues.Reverse();

            for (int i = 0; i < 4; i++)
            {
                scoreTextObjects[i].text = scoreValues[i].ToString();
            }
        }

        public void SetUpElement(GameObject prefab)
        {
            scoreboard = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;
            scoreboard.transform.SetParent(ui_manager.transform);

            scoreboard.transform.localScale = Vector3.one;
            scoreboard.transform.localEulerAngles = Vector3.zero;
            scoreboard.transform.localPosition = new Vector2(154.0f, 0.0f);

            scoreTextObjects.Add(scoreboard.GetComponent<ScoreboardObjects>().player1_score);
            scoreTextObjects.Add(scoreboard.GetComponent<ScoreboardObjects>().player2_score);
            scoreTextObjects.Add(scoreboard.GetComponent<ScoreboardObjects>().player3_score);
            scoreTextObjects.Add(scoreboard.GetComponent<ScoreboardObjects>().player4_score);

            scoreValues.Add(player1_score);
            scoreValues.Add(player2_score);
            scoreValues.Add(player3_score);
            scoreValues.Add(player4_score);
        }
    }
}