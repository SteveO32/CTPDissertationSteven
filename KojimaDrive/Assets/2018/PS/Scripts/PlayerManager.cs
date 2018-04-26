﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PS
{
    [System.Obsolete("Old", true)]
    public enum playerType
    {
        PLAYER1,
        PLAYER2,
        PLAYER3,
        PLAYER4
    }
    [System.Obsolete("Old", true)]
    public enum gameMode
    {
        SUMO,
        BOARD,
        TANK,
        FIRE,
        FROG,
        GOLF,
        BOAT,
        TAXI
    }
    [System.Obsolete("Old", true)]
    public class PlayerManager : MonoBehaviour
    {

        public GameObject cam;
        //public script;
        public GameObject player1;
        public GameObject player2;
        public GameObject player3;
        public GameObject player4;


        public List<GameObject> players;

        public int noOfPlayers = 4;


        // Use this for initialization
        void Start()
        {

            players.Capacity = 4;

            players.Add(player1);
            players.Add(player2);
            players.Add(player3);
            players.Add(player4);

            Debug.Log(players);

            LoadPlayersIntoGame(gameMode.SUMO);

        }

        // Update is called once per frame
        void Update()
        {

        }

        //public void SetAsPlayer(GameObject target, PlayerScript script, playerType playertype)
        //{


        //    switch (playertype)
        //    {
        //        case playerType.PLAYER1:
        //            player1 = target;
        //            //ApplyPlayer(player1, script);
        //            break;
        //        case playerType.PLAYER2:
        //            player2 = target;
        //            // ApplyPlayer(player2, script);
        //            break;
        //        case playerType.PLAYER3:
        //            player3 = target;
        //            //ApplyPlayer(player3, script);
        //            break;
        //        case playerType.PLAYER4:
        //            player4 = target;
        //            //ApplyPlayer(player4, script);
        //            break;
        //    }

        //}

        void ApplyPlayer(GameObject player/*, PlayerScript script*/)
        {
            GameObject tempCam = Instantiate(cam, player.transform.position, cam.transform.rotation);
            tempCam.transform.parent = player.transform;
            // AddGameModeScript(player, script);

        }



        //void AddGameModeScript(GameObject player, PlayerScript script)
        //{
        //    if (script is PlayerScript_Sumo)
        //    {
        //        player.AddComponent<PlayerScript_Sumo>();
        //    }

        //    if (script is PlayerScript_Boat)
        //    {
        //        player.AddComponent<PlayerScript_Boat>();
        //    }

        //    if (script is PlayerScript_Tank)
        //    {
        //        player.AddComponent<PlayerScript_Tank>();
        //    }

        //    if (script is PlayerScript_Golf)
        //    {
        //        player.AddComponent<PlayerScript_Golf>();
        //    }

        //    if (script is PlayerScript_Fire)
        //    {
        //        player.AddComponent<PlayerScript_Fire>();
        //    }

        //    if (script is PlayerScript_Frog)
        //    {
        //        player.AddComponent<PlayerScript_Frog>();
        //    }

        //    if (script is PlayerScript_Taxi)
        //    {
        //        player.AddComponent<PlayerScript_Taxi>();
        //    }

        //    if (script is PlayerScript_Board)
        //    {
        //        player.AddComponent<PlayerScript_Board>();
        //    }
        //}

        void SpawnPlayer(int playerNumber,/* PlayerScript script,*/ Vector3 position, Quaternion rotation)
        {
            //  if (script is PlayerScript_Sumo)
            // {
            //Player[playernumber] gameobject = whatever the prefab we want it to be is
            //Instantiate that prefab
            //Addgamemodescript to it

            //players[playerNumber] = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Car.prefab", typeof(GameObject)) as GameObject;
            Instantiate(players[playerNumber], position, rotation);
            // AddGameModeScript(players[playerNumber], script);

            Debug.Log(players[playerNumber]);

            // }




            /* if (script is PlayerScript_Boat)
             {

             }

             if (script is PlayerScript_Tank)
             {

             }

             if (script is PlayerScript_Golf)
             {

             }

             if (script is PlayerScript_Fire)
             {

             }

             if (script is PlayerScript_Frog)
             {

             }

             if (script is PlayerScript_Taxi)
             {

             }

             if (script is PlayerScript_Board)
             {

             }*/

        }

        void LoadPlayersIntoGame(gameMode mode)
        {
            int iterator = 0;
            Vector3 pos = new Vector3(0, 0, 0);
            Quaternion rot = new Quaternion(0, 0, 0, 0);

            if (mode == gameMode.SUMO)
            {
                //script = AssetDatabase.LoadAssetAtPath("Assets/Scripts/PlayerManagement/PlayerScript_Sumo.cs", typeof(PlayerScript)) as PlayerScript;
                pos = new Vector3(25, 26, 5);
                rot = new Quaternion(0, 0, 0, 0);
            }
            //Do the same thing for each other game mode


            //Debug.Log(script);


            foreach (GameObject player in players)
            {
                if (iterator == 0)
                {
                    pos = new Vector3(25, 26, 5);
                    rot = new Quaternion(0, 0, 0, 0);

                }
                if (iterator == 1)
                {
                    pos = new Vector3(33, 26, 8);
                    rot = new Quaternion(0, 0, 0, 0);
                }
                if (iterator == 2)
                {
                    pos = new Vector3(40, 26, 3);
                    rot = new Quaternion(0, 0, 0, 0);
                }
                if (iterator == 3)
                {
                    pos = new Vector3(33, 26, -4);
                    rot = new Quaternion(0, 0, 0, 0);
                }
                SpawnPlayer(iterator, /*script,*/ pos, rot);
                iterator++;
            }
        }

    }
}