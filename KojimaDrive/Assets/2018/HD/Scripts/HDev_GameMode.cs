using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/*
 * Kojima Party - Hairy Devs 2018
 * Author: Owen Jackson
 * Purpose: Controls the game logic for the HDevs gamemode
 * Namespace: HDev
 * Last Edited: Owen Jackson @ 05/03/2018
*/

namespace HDev
{
    public struct ScoreBoardInfo
    {
        public int Position { get; set; }
        public int PlayerID { get; set; }
        public float Score { get; set; }
    }

    
    public enum PlayArea
    {
        BEACHTOWN = 0,
        DAMCITY,
        HILLSIDEVILLAGE
    }
    
    public class HDev_GameMode : KojimaParty.GameMode
    {
        //variables for the rounds
        private int numberOfRounds = 1;
        private int currentRoundNumber = 1;

        private bool isRoundPlaying = false;    //used for counting down the round timer and letting the players move
        private bool displayScoresOneShot = false;  //prevents the EndRound function from being called multiple times

        Vector2[] CameraPositions = { new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 0f), new Vector2(0.5f, 0f) };   //presets for each player's camera
        Vector2 CameraViewportSize = new Vector2(0.5f, 0.5f);   //each camera takes a quarter of the screen

        [SerializeField]
        private float roundTimer = 0;   //current timer for this round

        public float maxRoundTimer;     //what the timer starts at for each round
        [SerializeField]
        private float preStartCountdown;    //this counts down "3, 2, 1, GO" to start the round
        public float maxPreStartCountdown;

        [SerializeField]
        List<FH.LandVehicle> players;

        [SerializeField]
        private PlayArea whereWePlaying;

        public float[] playerScores;
        public ScoreBoardInfo[] finalScoresInfo;

        public ScoreBoardInfo[] currentScoresInfo;

        public List<Vector3> playerStarts;   //where each car starts the round from
        public List<Quaternion> initialRotations;   //the rotation of each player at the start of the game

        public Canvas scoreBoardCanvas;  //temporary since I think a scoreboard class is being worked on
        public Canvas timerTextCanvas;
        private Text timerText;
        public HDev_Scoreboard scoreboard;

        public override void StartGameMode()
        {
            //players = FindObjectsOfType<Kojima.CarScript>().ToList();   //possibly change later for optimisation
            players = FindObjectsOfType<FH.LandVehicle>().ToList();
            //players.OrderByDescending(x => x.ControllerID); //sort player controller ids to be in order
            if (playerStarts == null)
            {
                playerStarts = new List<Vector3>();
            }
            //scoreBoardCanvas = GetComponentInChildren<Canvas>();
            if (GetComponentInChildren<Text>())
            {
                timerText = timerTextCanvas.GetComponentInChildren<Text>();
            }

            //assign each player's camera and start spawn
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponentInChildren<Camera>().rect = new Rect(CameraPositions[players[i].ControllerID], CameraViewportSize);
                playerStarts.Add(players[i].transform.position); //also temporary but add where they are on startup as their restart positions
                initialRotations.Add(players[i].transform.rotation);
            }

            //init scores array            
            playerScores = new float[players.Count];

            //initialise values for a new round to start
            BeginNewRound();
        }

        //resets all values to start the next round
        public void BeginNewRound()
        {
            if(scoreBoardCanvas.isActiveAndEnabled)
            {
                scoreBoardCanvas.gameObject.SetActive(false);
                timerTextCanvas.gameObject.SetActive(true);
            }

            roundTimer = maxRoundTimer;
            preStartCountdown = maxPreStartCountdown;
            isRoundPlaying = false;

            //restart the player positions
            for(int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<Rigidbody>().Sleep();
                players[i].transform.position = playerStarts[i];
                players[i].transform.rotation = initialRotations[i];
                //players[i].SetCanMove(false);
                players[i].CanMove = false;
                //players[i].GetComponentInChildren<HDev_ScoreCalculation>().SetScore(0);
            }

            displayScoresOneShot = false;
        }

        private void Update()
        {
            //conitnue the round logic while the timer is still going
            if (roundTimer > 0)
            {
                timerText.text = ((int)roundTimer).ToString();
                //pre-round timer check
                if (!isRoundPlaying)
                {
                    
                    //countdown the pre-round timer
                    preStartCountdown -= Time.deltaTime;                    
                    if (preStartCountdown <= 0)
                    {
                        isRoundPlaying = true;
                        //let the players move
                        for(int i = 0; i < players.Count; i++)
                        {
                            //players[i].SetCanMove(true);
                            players[i].CanMove = true;
                        }
                        //start the zone managers
                        PickupZoneManager.Instance.Begin();
                        DropoffZoneManager.Instance.Begin();
                    }
                    else
                    {
                        //stop the players from moving
                        for(int i = 0; i < players.Count; i++)
                        {
                            if(players[i].CanMove)
                            {
                                players[i].CanMove = false;
                            }
                        }
                    }
                }
                else
                {
                    roundTimer -= Time.deltaTime;


                    PlayersCurrentPlacesInScore();
                    //when timer runs out
                    if (roundTimer <= 0)
                    {
                        //stop all players from being able to move and get final scores
                        for(int i = 0; i < players.Count; i++)
                        {
                            //players[i].SetCanMove(false);
                            players[i].CanMove = false;
                            playerScores[i] = players[i].GetComponentInChildren<HDev_ScoreCalculation>().GetScore();
                        }
                    }
                }
            }
            //end the round because the timer has finished counting down
            else if(!displayScoresOneShot)
            {
                EndRound();
                displayScoresOneShot = true;
            }
            if(displayScoresOneShot)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    BeginNewRound();
                }
            }
        }

        //called when the round is over, logic for getting the winner and bringing up the options menu/start next round
        public void EndRound()
        {
            //stop everyone
            for(int i = 0; i < players.Count; i++)
            {
                //players[i].SetCanMove(false);
                players[i].CanMove = false;
            }
            //work out everyone's positions
            WorkOutPositions();

            ShowEndStats();

            SetRanks();
        }


        private void SetRanks()
        {
            finalScoresInfo = finalScoresInfo.OrderBy(x => x.PlayerID).ToArray();

            GameModeFinished(finalScoresInfo[0].Position, finalScoresInfo[1].Position, finalScoresInfo[2].Position, finalScoresInfo[3].Position);
        }

        public void WorkOutPositions()
        {
            finalScoresInfo = new ScoreBoardInfo[playerScores.Length];

            for(int i = 0; i < finalScoresInfo.Length; i++)
            {
                //finalScoresInfo[i].PlayerID = players[i].m_nControllerID;
                finalScoresInfo[i].PlayerID = players[i].ControllerID;
                finalScoresInfo[i].Score = playerScores[i];
            }

            for(int i = 0; i < finalScoresInfo.Length; i++)
            {
                int position = 1;
                for(int j = 0; j < finalScoresInfo.Length; j++)
                {
                    if(finalScoresInfo[i].Score < finalScoresInfo[j].Score)
                    {
                        position++;
                    }
                }
                finalScoresInfo[i].Position = position;
            }
            //finalScoresInfo = finalScoresInfo.OrderBy(x => x.PlayerID).ToArray();
            finalScoresInfo = finalScoresInfo.OrderBy(x => x.Position).ToArray();
            /*
            for(int i = 0; i < finalScoresInfo.Length; i++)
            {
               Debug.Log("positions: " + finalScoresInfo[i].Position);
            }
            */

        }

        void PlayersCurrentPlacesInScore()
        {
            //Debug.Log("PlayersCurrentPlacesInScore()");
            currentScoresInfo = new ScoreBoardInfo[playerScores.Length];

            for (int i = 0; i < currentScoresInfo.Length; i++)
            {
                //currentScoresInfo[i].Score = players[i].GetComponentInChildren<HDev_ScoreCalculation>().GetScore();
                playerScores[i] = players[i].GetComponentInChildren<HDev_ScoreCalculation>().GetScore();
            }

            for (int i = 0; i < currentScoresInfo.Length; i++)
            {
                //finalScoresInfo[i].PlayerID = players[i].m_nControllerID;
                currentScoresInfo[i].PlayerID = players[i].ControllerID;
                currentScoresInfo[i].Score = playerScores[i];
            }

            for (int i = 0; i < currentScoresInfo.Length; i++)
            {
                int position = 1;
                for (int j = 0; j < currentScoresInfo.Length; j++)
                {
                    if (currentScoresInfo[i].Score < currentScoresInfo[j].Score)
                    {
                        position++;
                    }
                }
                currentScoresInfo[i].Position = position;
            }
            currentScoresInfo = currentScoresInfo.OrderBy(x => x.PlayerID).ToArray();
            currentScoresInfo = currentScoresInfo.OrderBy(x => x.Position).ToArray();

            for (int i = 0; i < currentScoresInfo.Length; i++)
            {
                players[i].GetComponentInChildren<HDev_changeText>().SetPlace(currentScoresInfo[players[i].ControllerID].Position);
                //Debug.Log(i +"place"+ currentScoresInfo[i].Position);
            }

        }

        //displays the final scores and lists who came where
        public void ShowEndStats()
        {
            timerTextCanvas.gameObject.SetActive(false);
            scoreBoardCanvas.gameObject.SetActive(true);
            scoreboard.SetPlayerScores(finalScoresInfo);            
        }

        private IEnumerable WaitForOtherSetups(float time)
        {
            yield return new WaitForSeconds(time);
        }
    }
}
