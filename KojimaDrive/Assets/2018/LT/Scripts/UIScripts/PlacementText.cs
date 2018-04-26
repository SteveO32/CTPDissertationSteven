using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Daniel Donaldson
// Purpose:		Manager for the UI Placement Text Element
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class PlacementText
    {
        private Text placementText;
        public static PlacementText instance;
        private int numPlayers;
        private List<string> positions = new List<string>();
        private List<int> playerScores = new List<int>();
        private List<int> placementscores = new List<int>();
        private List<Canvas> playerCanvases = new List<Canvas>();
        GameObject placementTextObject;
        string currentPosition = null;
        UIManager ui_manager;

        int player1_score;
        int player2_score;
        int player3_score;
        int player4_score;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public PlacementText(int playerAmount, UIManager manager)
        {
            instance = this;
            ui_manager = manager;
            numPlayers = playerAmount;
        }

        public void InitTestMode()
        {
            numPlayers = 3;
        }

        public void SetUpCanvases()
        {
            playerCanvases = ui_manager.canvases;
        }

        public string GetCurrentPosition(int ID)
        {
            return playerCanvases[ID - 1].gameObject.transform.Find("Placement Text(Clone)").GetComponent<Text>().text;
        }

        public void PlacementTextData()
        {
            player1_score = playerCanvases[0].GetComponent<UIManager>().score_reference.GetScore();
            player2_score = playerCanvases[1].GetComponent<UIManager>().score_reference.GetScore();
            player3_score = playerCanvases[2].GetComponent<UIManager>().score_reference.GetScore();
            player4_score = playerCanvases[3].GetComponent<UIManager>().score_reference.GetScore();

            // if player score or player position > player2's score/position etc
            playerScores[0] = player1_score;
            playerScores[1] = player2_score;
            playerScores[2] = player3_score;
            playerScores[3] = player4_score;

            placementscores[0] = player1_score;
            placementscores[1] = player2_score;
            placementscores[2] = player3_score;
            placementscores[3] = player4_score;

            placementscores.Sort();
            placementscores.Reverse();

            for (int i = 0; i < playerScores.Count; i++)
            {
                if (playerScores[i] == placementscores[0])
                {
                    playerCanvases[i].gameObject.transform.Find("Placement Text(Clone)").GetComponent<Text>().text = positions[0];
                }
                else if (playerScores[i] == placementscores[1])
                {
                    playerCanvases[i].gameObject.transform.Find("Placement Text(Clone)").GetComponent<Text>().text = positions[1];
                }
                else if (playerScores[i] == placementscores[2])
                {
                    playerCanvases[i].gameObject.transform.Find("Placement Text(Clone)").GetComponent<Text>().text = positions[2];
                }
                else if (playerScores[i] == placementscores[3])
                {
                    playerCanvases[i].gameObject.transform.Find("Placement Text(Clone)").GetComponent<Text>().text = positions[3];
                }
            }

            //Debug.Log(playerScores.ToString());
        }

        public void SetUpElement(GameObject prefab)
        {
            placementTextObject = MonoBehaviour.Instantiate(prefab, new Vector2(0.0f, 0.0f), Quaternion.identity) as GameObject;
            placementTextObject.transform.SetParent(ui_manager.transform, true);
            placementTextObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 1.0f);
            placementTextObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.0f, 1.0f);
            placementText = placementTextObject.GetComponent<Text>();

            placementTextObject.transform.localScale = Vector3.one;
            placementTextObject.transform.localEulerAngles = Vector3.zero;
            placementTextObject.transform.localPosition = Vector3.zero;
            placementTextObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    currentPosition = "1 st";
                }
                else if (i == 1)
                {
                    currentPosition = "2 nd";
                }
                else if (i == 2)
                {
                    currentPosition = "3 rd";
                }
                else if (i == 3)
                {
                    currentPosition = "4 th";
                }

                positions.Add(currentPosition);
            }

            playerScores.Add(player1_score);
            playerScores.Add(player2_score);
            playerScores.Add(player3_score);
            playerScores.Add(player4_score);

            placementscores.Add(player1_score);
            placementscores.Add(player2_score);
            placementscores.Add(player3_score);
            placementscores.Add(player4_score);
        }
    }
}