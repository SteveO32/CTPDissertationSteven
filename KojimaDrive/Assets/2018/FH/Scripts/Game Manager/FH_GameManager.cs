using System.Collections.Generic;
using UnityEngine;


namespace FH
{
    public class FH_GameManager : KojimaParty.GameMode
    {
        /// <summary>
        /// Starts half way between MaxScore and zero. This value goes goes up one for each fire
        ///   which is creaded and down one when a fire is put out.
        /// </summary>
        public static ushort Score { get; set; }


        [SerializeField]
        private List<GameObject> m_allSceneObjects = new List<GameObject>();
        [SerializeField]
        private List<GameObject> m_players = new List<GameObject>();
        [SerializeField]
        private GameObject Boundary;


        //start time in seconds
        [SerializeField]
        private static float StartTime = 120;
        private static float RemainingTime;
        private static ushort MaxScore = 10;

        private bool GameConcluded = false;

        [SerializeField]
        private GameObject Location;
        public static GameObject CityLocation { get; set; }

        // 4 - player count.
        // TODO: set these based on player ranks.
        private int[] scores = new int[4];



        public override void StartGameMode()
        {
            Init();

            foreach(var elem in m_allSceneObjects)
                if(elem)
                    elem.SetActive(true);

            foreach(GameObject player in m_players)
            {
                player.GetComponentInChildren<LT.UIManager>().SetUpUIManager();
                player.GetComponentInChildren<LT.UIManager>().countdown_reference.ActivateCountdown();
            }
        }



        private void EndGameMode()
        {
            GameModeFinished(scores[0], scores[1], scores[2], scores[3]);
        }



        // Use this for initialization
        private void Init()
        {
            Instantiate(Boundary, new Vector3(50, 0, 0), Quaternion.identity);
            CityLocation = Instantiate(Location, new Vector3(-458, 330, -2315), Quaternion.identity);
            Score = (System.UInt16)(MaxScore / 2);
            RemainingTime = StartTime;
            GameConcluded = false;
        }


        private void Update()
        {
            if(!GameConcluded)
            {
                if(RemainingTime > 0)
                {
                    RemainingTime -= Time.deltaTime;

                    foreach(GameObject player in m_players)
                    {
                        player.GetComponentInChildren<LT.UIManager>().timer_reference.SetTimerValue((int)RemainingTime);
                    }
                    //Debug.Log("Log: Remaining time -  " + (int)RemainingTime);
                }
                else
                {
                    CalculateWinner();
                    ShowScore();
                }
                //Debug.Log("Log: Score - " + Score);
            }
        }



        private void CalculateWinner()
        {
            if(Score == MaxScore / 2)
            {
                //.Log("Log: Teams draw!");
                print("Everybody wins");

                scores[0] = 1;
                scores[1] = 1;
                scores[2] = 1;
                scores[3] = 1;
            }
            else if(Score > MaxScore / 2)
            {
                //Debug.Log("Log: Team 1 wins!");
                print("Areoplane wins");

                scores[0] = 1;
                scores[1] = 2;
                scores[2] = 2;
                scores[3] = 2;

            }
            else if(Score < MaxScore / 2)
            {
                //Debug.Log("Log: Team 2 wins!");
                print("Fire Engines Win");

                scores[1] = 1;
                scores[2] = 1;
                scores[3] = 1;
                scores[0] = 2;
            }
            else
            {
                //ERROR!!!!
            }

            GameConcluded = true;
        }


        private void ShowScore()
        {
            EndGameMode();

        }
    }
}
