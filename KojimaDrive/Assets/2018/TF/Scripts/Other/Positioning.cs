using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//===================== Kojima Party - Team Frivolous 2018 ====================//
//
// Author:      Harry McAlpine
// Purpose:     Massive rehash of the Race mode from 2017 for Alpha testing.
// Namespace:   TF
//
//===============================================================================//

namespace TF
{
    public class Positioning : MonoBehaviour
    { 
        public bool positionAboveHead = false;
        public LT.HumanCharacterControl[] players;
        public List<KeyValuePair<int, int>> playerPositionsSorted;
        public TF.FroggerModeTemp froggerController;
        public Dictionary<int, int> m_currentWaypoint;
        public List<GameObject> m_lRacePointClones;
        // Use this for initialization

        void Start()
        {
            m_currentWaypoint = new Dictionary<int, int>();

            foreach (LT.HumanCharacterControl player in players)
            {
                if (player != null)
                {
                    m_currentWaypoint.Add(player.playerId, 0);
                }
            }

        }

        // Update is called once per frame
        void Update()
        {
            updatePostions();
        }

        private void updatePostions()
        {
            List<KeyValuePair<int, int>> playerRacePositions = new List<KeyValuePair<int, int>>();

            foreach (LT.HumanCharacterControl player in players)
            {
                if (player != null)
                {
                    playerRacePositions.Add(new KeyValuePair<int, int>(player.playerId, m_currentWaypoint[player.playerId]));
                }
            }

            playerPositionsSorted = playerRacePositions.OrderBy(x => x.Value).ToList();
            playerPositionsSorted.Reverse();

            List<KeyValuePair<int, int>> samePosSort = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, float>> samePosDists = new List<KeyValuePair<int, float>>();

            for (int i = 0; i < playerPositionsSorted.Count; i++)
            {
                //Clear list
                samePosSort.Clear();

                //Add this player
                samePosSort.Add(playerPositionsSorted[i]);

                //Loop through the rest of the players
                for (int j = i + 1; j < playerPositionsSorted.Count; j++)
                {
                    if (playerPositionsSorted[j].Value == playerPositionsSorted[i].Value)
                    {
                        samePosSort.Add(playerPositionsSorted[j]);
                    }
                    else
                    {
                        break;
                    }
                }

                //If multiple are in the same place
                if (samePosSort.Count > 1)
                {
                    //Work out order of players with same waypoint
                    samePosDists.Clear();
                    for (int h = 0; h < samePosSort.Count; h++)
                    {
                        //Get gameobjects
                        int playerIndex = samePosSort[h].Key;
                        LT.HumanCharacterControl playerCont = players[playerIndex];

                        GameObject playerWaypoint = m_lRacePointClones[m_currentWaypoint[samePosSort[h].Key]];
                        GameObject playerMan = playerCont.gameObject;

                        //Calculate Distance
                        float distance = Vector3.Distance(playerMan.transform.position, playerWaypoint.transform.position);

                        samePosDists.Add(new KeyValuePair<int, float>(samePosSort[h].Key, distance));
                    }

                    //Sort distance list by distance
                    samePosDists = samePosDists.OrderBy(x => x.Value).ToList();

                    //Store current waypoint
                    int waypoint = samePosSort[0].Value;

                    //Add sorted distance list back in to unsorted same position list using waypoint
                    for (int h = 0; h < samePosSort.Count; h++)
                    {
                        samePosSort[h] = new KeyValuePair<int, int>(samePosDists[h].Key, waypoint);
                    }

                    //Update old list
                    for (int k = 0; k < samePosSort.Count; k++)
                    {
                        playerPositionsSorted[i + k] = samePosSort[k];
                    }
                }
            }

            for (int i = 0; i < playerPositionsSorted.Count; i++)
            {

                Debug.Log("Player (" + playerPositionsSorted[i].Key + ") is at position: " + i);

                if (!positionAboveHead)
                {
                    int playerNo = playerPositionsSorted[i].Key;
                    LT.HumanCharacterControl playerCont = players[playerNo];
                    TypogenicText playerCarText = playerCont.gameObject.GetComponentInChildren<TypogenicText>();
                    playerCarText.Text = (i + 1).ToString();
                }
            }
        }

    }
}
