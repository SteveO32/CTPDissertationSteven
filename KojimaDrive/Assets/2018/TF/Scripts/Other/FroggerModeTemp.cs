using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//===================== Kojima Party - Team Frivolous 2018 ====================//
//
// Author:      Harry McAlpine
// Purpose:     Temporary game mode player holder.
// Namespace:   TF
//
//===============================================================================//

namespace TF
{

    public class FroggerModeTemp : MonoBehaviour
    {
        public enum States
        {
            Setup,
            PreRace,
            PreRound,
            Round,
            PostRound,
            GameOver
        }


        [Header("PlayerControl")]
        public static int s_nMaxPlayers = 4;
        public int s_ncurrentPlayers = 0;
        public GameObject[] players;
        public GameObject PlayerCharacter;
        private bool playersSetup = false;
        public List<List< int> > trophyPerPlayer;

        [Header("RacePoints")]
        public GameObject[] spawnPoints;
        public bool positionAboveHead = false;
        public List<KeyValuePair<int, int>> playerPositionsSorted;
        public Dictionary<int, int> m_currentWaypoint;
        public List<GameObject> m_lRacePointClones;

        [Header("RacePosistions")]
        [SerializeField]
        public List<KeyValuePair<int, int>> playerRacePositions;
        [SerializeField]
        public List<KeyValuePair<int, int>> samePosSort;
        [SerializeField]
        public List<KeyValuePair<int, float>> samePosDists;

        [Header("RaceControls")]
        public int maxRounds;
        public int currentRound;
        public States activeState = States.Setup;
        public Camera camera;

        [Header("Timer")]
        [SerializeField]
        float timer;
        [SerializeField]
        float timeLeft = 60f;
        public float roundMaxTime = 60f;

        [Header("Camera")]
        public GameObject CameraSplines;
        [SerializeField]
        GameObject cameraMount;
        [SerializeField]
        GameObject cameraViewTarget;
        [SerializeField]
        Bird.SplineFollower CSF;
        public float camflybyLength;
        public GameObject trophieCameraSpline;
        public GameObject trophieTargetSpline;

        public GameObject trophyholderPre;
        Vector3 screenPos;
        Vector2 onScreenPos;
        float max;
        [SerializeField]
        Transform[] allChildren;
        [SerializeField]
        bool isStateCoroutineStarted = false;
        private bool built;
        private bool doOnce;
        private bool isXCoroutineStarted;
        private bool changed;
        

        private void Start()
        {
            SetUpUIElements();
            
        }
        
        void SetUpUIElements()
        {
                GetComponentInChildren<LT.UIManager>().SetUpUIManager();
                
        }

        void Update()
        {
            updateTimer();
            switch (activeState)
            {
                case States.Setup:
                    SetupLocation();
                    SetupPlayers();
                    //Set up ai cars should go here
                    TurnOffPlayerControllers();
                    cameraMount = new GameObject();
                    cameraMount.name = "CameraMount";
                    BuildFlyCam(0);
                    
                   // Invoke("BuildFlyCam(1)", camflybyLength);
                    cameraMount.SetActive(true);
                    activeState += 1;
                    StartCoroutine(SecondWait(6));
                    
                    break;
                case States.PreRace:
                   
                    //Camera spline controller
                    TurnOffPlayerControllers();
                    if (!isStateCoroutineStarted)
                    {
                        isStateCoroutineStarted = true;
                        StartCoroutine(IncreateActiveState(10, 2));
                    }
                    
                    currentRound = 0;
                    break;

                case States.PreRound:
                   
                    if (!doOnce)
                    {

                        ResetPosistions();
                        TurnOffPlayerControllers();
                        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Trophy_Prefab(Clone)");
                        Debug.Log(objects.ToList());
                        if (objects.Count() == 0)
                        {
                            gameObject.GetComponent<IntersectionBuilder>().TrophySpawner(2);
                          
                        }
                        doOnce = true;
                        timeLeft = roundMaxTime;
                        activeState = States.Round;
                    }
                    break;
                case States.Round:
                    
                    cameraMount.SetActive(false);
                    TurnOnPlayerControllers();
                    updatePostions();
                    IsViewable();
                    timeLeft -= Time.deltaTime;
                    doOnce = false;
                    if (timeLeft < 0)
                    {
                        
                        activeState = States.PostRound;
                        currentRound = 1 + currentRound;
                    }
                    isStateCoroutineStarted = false;
                    break;
                case States.PostRound:
                   if(currentRound >= maxRounds)
                    {
                        if (!isStateCoroutineStarted)
                        {
                            StartCoroutine(IncreateActiveState(5, 5));
                        }
                    }
                   else
                    {
                        var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Trophy_Prefab(Clone)");
                        foreach (GameObject go in objects)
                        {
                            Destroy(go);
                        }
                        activeState = States.PreRound;
                       
                        
                    }
                    doOnce = false;
                    //Find what trophies the players have
                    break;
                case States.GameOver:
                    //scene management to board game with score
                    TurnOffPlayerControllers();

                    break;
                default:
                    Debug.Log("Default State");
                    break;
            }
            //Debug.Log(activeState);
        }
        void LateUpdate()
        {
            if (built)
            {
                cameraMount.GetComponent<Bird.SimpleLookAt>().tick();
            }
        }
        private void FindObjects(string name)
        {

        }
        private void BuildFlyCam(int _in)
        {
            if (!built)
            {

              
                cameraMount.AddComponent<Bird.SplineFollower>();
                cameraMount.AddComponent<Camera>();
                cameraMount.AddComponent<Bird.SimpleLookAt>();

                CSF = cameraMount.GetComponent<Bird.SplineFollower>();
                cameraViewTarget = new GameObject();
                cameraViewTarget.name = "ViewTarget";

                cameraViewTarget.AddComponent<Bird.SplineFollower>();

                cameraViewTarget.GetComponent<Bird.SplineFollower>().duration = camflybyLength;
                cameraMount.GetComponent<Bird.SimpleLookAt>().target = cameraViewTarget.transform;

                CSF.resetProgress();
                CSF.duration = camflybyLength;
                setSpline(_in);
                built = true;
               
            }
        }
        void ChangeFlyCam(int _in, float timeLength)
        {
            if(!changed)
            {
                CSF.resetProgress();
                CSF.duration = timeLength;
                setSpline(_in);
                changed = true;

            }
        }
        void setSpline(int _in)
        {
            trophieCameraSpline.transform.localPosition = new Vector3(GameObject.Find("Frogger/RaceEndPoint").transform.localPosition.x, 1.5f, GameObject.Find("Frogger/RaceEndPoint").transform.localPosition.y);
           

            cameraViewTarget.GetComponent<Bird.SplineFollower>().spline = CameraSplines.transform.GetChild(_in).gameObject.transform.GetChild(0).gameObject.GetComponent<Bird.BezierSpline>();
            CSF.spline = CameraSplines.transform.GetChild(_in).gameObject.transform.GetChild(1).GetComponent<Bird.BezierSpline>();
        }
        private void ResetPosistions()
        {
            var i = 0;
            foreach (GameObject player in players)
            {
                if (player != null)
                {

                    player.transform.position = spawnPoints[i].transform.position;
                    i++;
                }
            }
        }

        private void TurnOffPlayerControllers()
        {
            foreach (GameObject player in players)
            {
                if (player != null)
                {
                    if (player.GetComponent<LT.HumanCharacterInput>().enabled == true)
                    {
                        player.GetComponent<LT.HumanCharacterInput>().enabled = false;
                    }
                }
            }
        }

        private void TurnOnPlayerControllers()
        {
            foreach (GameObject player in players)
            {
                if (player != null)
                {
                    if (player.GetComponent<LT.HumanCharacterInput>().enabled != true)
                    {
                        player.GetComponent<LT.HumanCharacterInput>().enabled = true;
                    }
                }
            }
        }
        private void SetupLocation()
        {

        }
        private void IsViewable()
        {
            foreach (GameObject player in players)
            {
                if (player != null)
                {
                    if (player.GetComponentInChildren<Renderer>().IsVisibleFrom(camera))
                    {
                      //  Debug.Log("Player " + player.GetComponent<LT.HumanCharacterControl>().playerId + " is visible");
                    }
                  
                    else
                    {
                      //  Debug.Log("Player " + player.GetComponent<LT.HumanCharacterControl>().playerId + " is not visible");
                    }
                }
            }
        }
        private void SetupPlayers()
        {
            if (playersSetup != true)
            {
                for (var i = 1; i <= s_ncurrentPlayers; i++)
                {
                    GameObject player = (GameObject)Instantiate(PlayerCharacter, spawnPoints[i - 1].transform.position, Quaternion.identity);
                    player.GetComponentInChildren<LT.HumanCharacterControl>().playerId = i - 1;
                    player.GetComponentInChildren<LT.PlayerMaterialsApplier>().playerIndex = i - 1;
                    //player.GetComponent<LT.HumanCharacterControl>().speedMultiplier = 1;
                    players[i - 1] = player.GetComponentInChildren<LT.HumanCharacterControl>().gameObject;
                }

                m_currentWaypoint = new Dictionary<int, int>();
                foreach (GameObject player in players)
                {
                    if (player != null)
                    {
                        m_currentWaypoint.Add(player.GetComponentInChildren<LT.HumanCharacterControl>().playerId, 0);
                        GameObject gameObject = Instantiate(trophyholderPre, player.transform);
                        allChildren = player.GetComponentsInChildren<Transform>();
                        List<GameObject> childObjects = new List<GameObject>();
                        foreach (Transform child in allChildren)
                        {
                            child.gameObject.tag = "Player";
                        }
                    }
                }
                playersSetup = true;
            }
        }

        private void updatePostions()
        {
            playerRacePositions = new List<KeyValuePair<int, int>>();

            foreach (GameObject player in players)
            {
                if (player != null)
                {
                    playerRacePositions.Add(new KeyValuePair<int, int>(player.GetComponent<LT.HumanCharacterControl>().playerId,
                        m_currentWaypoint[player.GetComponent<LT.HumanCharacterControl>().playerId]));
                }
            }

            playerPositionsSorted = playerRacePositions.OrderBy(x => x.Value).ToList();
            playerPositionsSorted.Reverse();

            samePosSort = new List<KeyValuePair<int, int>>();
            samePosDists = new List<KeyValuePair<int, float>>();

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
                        LT.HumanCharacterControl playerCont = players[playerIndex].GetComponent<LT.HumanCharacterControl>();

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

                Debug.Log("Player (" + playerPositionsSorted[i].Key + ") is at position: " + (i));

                if (!positionAboveHead)
                {
                    int playerNo = playerPositionsSorted[i].Key;
                    LT.HumanCharacterControl playerCont = players[playerNo].GetComponent<LT.HumanCharacterControl>();
                    TypogenicText playerCarText = playerCont.gameObject.GetComponentInChildren<TypogenicText>();
                    playerCarText.Text = (i + 1).ToString();
                }
            }
        }
    
       private void updateTimer()
        {
            timer += Time.deltaTime;
        }

        IEnumerator IncreateActiveState(float yieldTime, int state)
        {
            isStateCoroutineStarted = true;
            if (true)
            {
                yield return new WaitForSeconds(yieldTime);
                activeState = (States)state;
            }
           
        }
        IEnumerator WaitForX(float yieldTime,int cam, int time)
        {
            isXCoroutineStarted = true;
            changed = false;
            while (true)
            {
                yield return new WaitForSeconds(yieldTime);
                isXCoroutineStarted = false;
                ChangeFlyCam(cam, time);
            }

        }
        IEnumerator SecondWait(int i)
        {
            yield return new WaitForSeconds(i);
            LT.Countdown.instance.ActivateCountdown();
        }
    }
}

