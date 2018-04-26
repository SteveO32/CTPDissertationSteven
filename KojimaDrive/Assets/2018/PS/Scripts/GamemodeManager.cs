using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public class GamemodeManager : MonoBehaviour
    {

        public GameObject rightBumper;
        public GameObject leftBumper;
       // GameObject playerManager;
        PlayerPrefabManager playerManager;
        TerrainCheck terrainCheck;
        public GameObject player1;
        int roundcount;
        public int player1Score;
        public int player2Score;
        public int player3Score;
        public int player4Score;
        public GameObject[] connectedPlayers;

        private void Start()
        {
            roundcount = 0;
            connectedPlayers = new GameObject[4];
            //GameObject tempGameObj = Instantiate(player1, this.transform.position, player1.transform.rotation);
            //tempGameObj.AddComponent<BumperControls>();


            //tempGameObj.GetComponent<BumperControls>().Init(rightBumper, leftBumper);

            // playerManager = GameObject.Find("PlayerManager");
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerPrefabManager>();
            terrainCheck = GameObject.Find("Raycaster").GetComponent<TerrainCheck>();

            playerManager.ConnectPlayer(0);
            playerManager.ConnectPlayer(1);
            playerManager.ConnectPlayer(2);
            playerManager.ConnectPlayer(3);

            

            playerManager.SelectThemeForPlayer(0, playerManager.GetComponent<ThemeManager>().GetTheme(ThemeStyle.RED));
            playerManager.SelectThemeForPlayer(1, playerManager.GetComponent<ThemeManager>().GetTheme(ThemeStyle.RED));
            playerManager.SelectThemeForPlayer(2, playerManager.GetComponent<ThemeManager>().GetTheme(ThemeStyle.RED));
            playerManager.SelectThemeForPlayer(3, playerManager.GetComponent<ThemeManager>().GetTheme(ThemeStyle.RED));

           // SpawnCars(terrainCheck.hitCenter);


        }

        void Update()
        {
            //Check each player for if they're alive, if iterator is at 1 then give the remaining player points and restart round, if its zero just restart the round
            if(CheckForWin())
            {
               for(int i = 0; i < connectedPlayers.Length; i++)
                {
                    if(connectedPlayers[i].activeSelf)
                    {
                        //Give players points

                        switch (i)
                        {
                            case 0:
                                player1Score++;
                                Debug.Log("Player 1: " + player1Score);
                                break;
                            case 1:
                                player2Score++;
                                Debug.Log("Player 2: " + player2Score);
                                break;
                            case 2:
                                player3Score++;
                                Debug.Log("Player 3: " + player3Score);
                                break;
                            case 3:
                                player4Score++;
                                Debug.Log("Player 4: " + player4Score);
                                break;
                            default:
                                break;
                        }
                    }
                }

                roundcount++;
                if (roundcount < 3)
                {
                    RestartRound();
                }
                else
                {
                    //Show results screen if there is one
                    //Pass scores to tranmission manager, load back to board game scene
                    Application.Quit();
                }
                
            }
        }

        public void SpawnCars(Vector3 center)
        {
            connectedPlayers[0] = Instantiate(playerManager.GetPlayer(0, Avatar.CAR)) as GameObject;
            connectedPlayers[1] = Instantiate(playerManager.GetPlayer(1, Avatar.CAR)) as GameObject;
            connectedPlayers[2] = Instantiate(playerManager.GetPlayer(2, Avatar.CAR)) as GameObject;
            connectedPlayers[3] = Instantiate(playerManager.GetPlayer(3, Avatar.CAR)) as GameObject;

            for (int i = 0; i < playerManager.players.Count; i++)
            {
                connectedPlayers[i].AddComponent<BoxCollider>();
                BoxCollider coll = connectedPlayers[i].GetComponent<BoxCollider>();
                connectedPlayers[i].GetComponent<FH.LandVehicle>().ControllerID = i;
                connectedPlayers[i].GetComponent<FH.LandVehicle>().rewiredPlayer = Rewired.ReInput.players.GetPlayer(playerManager.players[i].ControllerID);

                BumperControls bmper = connectedPlayers[i].AddComponent<BumperControls>();
                bmper.Init(rightBumper, leftBumper);

                connectedPlayers[i].GetComponentInChildren<AudioListener>().enabled = false;

                Camera cam = connectedPlayers[i].GetComponentInChildren<Camera>();
                GameObject canvasManager = new GameObject();
                canvasManager.name = "canvas";
                Canvas canvas = canvasManager.AddComponent<Canvas>();
                canvasManager.transform.SetParent(connectedPlayers[i].transform);
                canvasManager.AddComponent<CanvasRenderer>();
               // bmper.Init(rightBumper, leftBumper);

                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = cam;
                canvas.planeDistance = 1;
                //Image loseScreen = canvas.gameObject.AddComponent<Image>();
                // Sprite failSprite = Resources.Load("Fail") as Sprite;
                //loseScreen.sprite = failSprite;

                //connectedPlayers[i].GetComponent<FH.LandVehicle>().ControllerID = i;

                canvasManager.SetActive(false);

                coll.size = new Vector3(1.6f, 0.8f, 4f);
                coll.center = new Vector3(0, 0.6f, 0);

                switch (i)
                {
                    case 0:
                        connectedPlayers[i].GetComponentInChildren<Camera>().rect = new Rect(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
                        break;
                    case 1:
                        connectedPlayers[i].GetComponentInChildren<Camera>().rect = new Rect(new Vector2(0.0f, 0.5f), new Vector2(0.5f, 0.5f));
                        break;
                    case 2:
                        connectedPlayers[i].GetComponentInChildren<Camera>().rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 0.5f));
                        break;
                    case 3:
                        connectedPlayers[i].GetComponentInChildren<Camera>().rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 0.5f));
                        break;
                    default:
                        break;
                }



            }
            Debug.Log(playerManager.players.Count);
            Debug.Log(center);
            for (int i = 0; i < playerManager.players.Count; i++)
            {

                if (i == 0)
                {
                    connectedPlayers[i].transform.position = new Vector3(center.x - 30, center.y + 10, center.z);

                }
                else if (i == 1)
                {
                    connectedPlayers[i].transform.position = new Vector3(center.x + 30, center.y + 10, center.z);
                }
                else if (i == 2)
                {
                    connectedPlayers[i].transform.position = new Vector3(center.x, center.y + 10, center.z - 30);
                }
                else if (i == 3)
                {
                    connectedPlayers[i].transform.position = new Vector3(center.x, center.y + 10, center.z + 30);
                }

                connectedPlayers[i].transform.LookAt(center);
            }
            //For each player in player manager
            //Instantiate the player, then get their info and theme stuff from the player manager and apply it
            //Then add bumper controls to the player

            //Then set the location to wherever the ring is (When ring logic is correctly applied to big island)
        }
   
        void RestartRound()
        {
            
            //Reset each player to alive, put them back at spawn positions, reset ring to original size
            for(int i = 0; i < connectedPlayers.Length; i++)
            {
                //connectedPlayers[i].SetActive(true);
                // connectedPlayers[i].GetComponent<PlayerInfo>().isAlive = true;
                Destroy(connectedPlayers[i].gameObject);
                connectedPlayers[i] = null;
                
            }

            
            SpawnCars(terrainCheck.hitCenter);
            terrainCheck.arena.transform.localScale = new Vector3(80, 100, 80);

            //Countdown to round start?
        }

        bool CheckForWin()
        {
            //Go through each player and check if they're alive
            int playersAlive = 0;
            for(int i = 0; i < connectedPlayers.Length; i++)
            {
                if(connectedPlayers[i].activeSelf)
                {
                    playersAlive++;
                }
            }
            //if there's more than 1 player alive, the round isn't over
            if(playersAlive > 1)
            {
                return false;
            }
            else
            {
                return true;
            }

            
        }
     
    }
}
