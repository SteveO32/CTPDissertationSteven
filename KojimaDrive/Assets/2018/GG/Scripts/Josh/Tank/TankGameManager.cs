using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Josh Fenlon
// Purpose:		Game manager for the tank game
// Namespace:	GG
//
//============================================================================//

namespace GG
{

    public class TankGameManager : KojimaParty.GameMode
    {

       
        [System.Serializable]
        public class PlayerData
        {
            public GameObject player;
            public float damageDone;
            public int kills;
            public int deaths;
        }

        [System.Serializable]
        public class levels
        {
            public int map;
            public List<GameObject> obj;
            public List<SpawnPointData> spawnLocations;
        }

        [SerializeField]
        private float maxTime = 60;
        [SerializeField]
        private float currentTime = 0;
        [SerializeField]
        private float resetTimer = 0;
        [SerializeField]
        private bool gamePlaying = false;
        [SerializeField]
        private int players = 4;
        [SerializeField]
        public List<PlayerData> pd = new List<PlayerData>();
        [SerializeField]
        private bool spawnCloserToAction = false;
        [SerializeField]
        private float minimumDistanceToSpawn = 0;
        [SerializeField]
        private int lives = 10;
        [SerializeField]
        private int maxKills = 10;
        [SerializeField]
        private int map = 0;
        [SerializeField]
        private List<levels> maps = new List<levels>();
        [SerializeField]
        private int forceMap = -1;
        private bool gameModeStart = false;

        public override void StartGameMode()
        {
            gameModeStart = true;
            currentTime = resetTimer;

            SpawnPointData[] tempAllSpawns = GameObject.FindObjectsOfType<SpawnPointData>();

            for (int a = 0; a < tempAllSpawns.Length; a++)
            {
                for (int b = 0; b < maps.Count; b++)
                {
                    if (maps[b].map == tempAllSpawns[a].spawnPointLevel)
                    {
                        maps[b].spawnLocations.Add(tempAllSpawns[a]);
                        break;
                    }
                }
            }

            for (int b = 0; b < maps.Count; b++)
            {
                for (int a = 0; a < maps[map].obj.Count; a++)
                {
                    maps[b].obj[a].SetActive(false);
                }
            }
        }

        void Update()
        {
            if (gameModeStart == true)
            {
                //checks if the game is playing and whether the game should end
                if (gamePlaying)
                {
                    for (int a = 0; a < pd.Count; a++)
                    {
                        if (pd[a].kills > maxKills)
                        {
                            gamePlaying = false;
                        }
                    }
                    if (currentTime >= maxTime)
                    {
                        gamePlaying = false;
                        currentTime = 0;
                    }
                    else
                    {
                        currentTime += Time.deltaTime;
                    }
                }
                else
                {
                    if (currentTime >= resetTimer)
                    {
                        gamePlaying = true;
                        currentTime = 0;
                        for (int a = 0; a < maps[map].obj.Count; a++)
                        {
                            maps[map].obj[a].SetActive(false);
                        }
                        for (int a = 0; a < pd.Count; a++)
                        {
                            pd[a].player.GetComponent<BasicHealthTest>().controlParts(true);
                            pd[a].player.GetComponent<BasicHealthTest>().enabled = true;
                            pd[a].player.GetComponent<BasicHealthTest>().health = pd[a].player.GetComponent<BasicHealthTest>().maxHealth;
                            pd[a].player.GetComponent<TankController>().enabled = true;
                            pd[a].player.GetComponent<AllowTurretDifference>().enabled = true;
                        }
                        if (forceMap == -1)
                        {
                            map = Random.Range(0, maps.Count);
                        }
                        else
                        {
                            map = forceMap;
                        }
                        for (int a = 0; a < maps[map].obj.Count; a++)
                        {
                            maps[map].obj[a].SetActive(true);
                        }
                        for (int a = 0; a < players; a++)
                        {

                            pd[a].deaths = 0;
                            pd[a].damageDone = 0;
                            pd[a].kills = 0;
                            FindNewSpawn(a, true);
                        }
                    }
                    else
                    {
                        currentTime += Time.deltaTime;
                    }
                }
            }
        }

        void OnGUI()
        {
            if (gameModeStart == true)
            {
                //displays the time left above players if the game is playing
                if (gamePlaying)
                {

                    HUDcontrol[] huds = GameObject.FindObjectsOfType<HUDcontrol>();
                    for (int a = 0; a < players; a++)
                    {
                        foreach (HUDcontrol hud in huds)
                        {
                            if (hud.Tank == pd[a].player)
                            {
                                hud.Score.text = pd[a].kills.ToString();
                            }
                        }
                    }

                    //calculate time left in displayable format
                    int timeLeft = (int)(maxTime - currentTime);
                    int minutes = 0;
                    while (timeLeft > 60)
                    {
                        timeLeft -= 60;
                        minutes++;
                    }
                    string timeRemaining = minutes.ToString();
                    timeRemaining += ":";
                    if (timeLeft < 10)
                    {
                        timeRemaining += "0";
                    }
                    timeRemaining += timeLeft.ToString();

                    foreach (HUDcontrol hud in huds)
                    {
                        hud.Timer.text = timeRemaining;
                    }

                }
                else
                {

                    HUDcontrol[] huds = GameObject.FindObjectsOfType<HUDcontrol>();
                    //calculate time left in displayable format
                    int timeLeft = (int)(resetTimer - currentTime);
                    int minutes = 0;
                    while (timeLeft > 60)
                    {
                        timeLeft -= 60;
                        minutes++;
                    }
                    string timeRemaining = minutes.ToString();
                    timeRemaining += ":";
                    if (timeLeft < 10)
                    {
                        timeRemaining += "0";
                    }
                    timeRemaining += timeLeft.ToString();

                    foreach (HUDcontrol hud in huds)
                    {
                        hud.Timer.text = timeRemaining;
                    }

                    //display whos the winner
                    string[] details = new string[players];
                    float winnerScore = 0;
                    bool draw = false;
                    //calculate the highest score
                    foreach (PlayerData _pd in pd)
                    {
                        if (_pd.kills == winnerScore)
                        {
                            draw = true;
                        }
                        if (_pd.kills > winnerScore)
                        {
                            winnerScore = _pd.kills;
                            draw = false;
                        }
                    }
                    //set details to loser,winner or draw
                    for (int a = 0; a < players; a++)
                    {
                        if (pd[a].kills < winnerScore)
                        {
                            details[a] = "Loser";
                        }
                        else
                        {
                            if (draw)
                            {
                                details[a] = "Draw";
                            }
                            else
                            {
                                details[a] = "Winner";
                            }
                        }
                        details[a] += "\nKills: " + pd[a].kills.ToString();
                        details[a] += "\nDeaths: " + pd[a].deaths.ToString();
                        details[a] += "\nDamage Done: " + pd[a].damageDone.ToString();
                    }
                    //display the details to the screen
                    for (int a = 0; a < 2; a++)
                    {
                        for (int b = 0; b < 2; b++)
                        {
                            Rect pos = new Rect((Screen.width / 2) * a - (Screen.width / 4) - 60 + Screen.width / 2, (Screen.height / 2) * b - (Screen.height / 4) - 15 + Screen.height / 2, 120, 70);
                            GUI.TextField(pos, details[(a * 2) + b]);
                        }
                    }
                }
            }
        }

        //searches through all spawns and finds the best position to spawn the player
        public bool FindNewSpawn(int playerID, bool free = false)
        {
            GameObject bestSpawn = null;
            float bestSpawnScore = 0;

            if (spawnCloserToAction)
            {
                bestSpawnScore = float.MaxValue;
            }

            if (pd[playerID].deaths >= lives)
            {
                pd[playerID].player.GetComponent<BasicHealthTest>().controlParts(false);
                pd[playerID].player.GetComponent<BasicHealthTest>().enabled = false;
                pd[playerID].player.GetComponent<TankController>().enabled = false;
                pd[playerID].player.GetComponent<AllowTurretDifference>().enabled = false;
                return false;
            }

            //loops through all spawns and tests distance to all players
            foreach (SpawnPointData spawn in maps[map].spawnLocations)
            {
                float spawnScore = 0;
                foreach (PlayerData _pd in pd)
                {
                    if (_pd.player != null)
                    {
                        float distance = Vector3.Distance(_pd.player.transform.position, spawn.transform.position);
                        spawnScore += distance;
                        if (distance < minimumDistanceToSpawn)
                        {
                            if (spawnCloserToAction)
                            {
                                spawnScore += 1000;
                            }
                            else
                            {
                                spawnScore -= 1000;
                            }
                        }

                    }
                }
                spawnScore /= pd.Count;
                if (spawnCloserToAction)
                {
                    if (spawnScore < bestSpawnScore)
                    {
                        bestSpawnScore = spawnScore;
                        bestSpawn = spawn.gameObject;
                    }
                }
                else
                {
                    if (spawnScore > bestSpawnScore)
                    {
                        bestSpawnScore = spawnScore;
                        bestSpawn = spawn.gameObject;
                    }
                }

            }
            //spawns the player at the best spawn
            if (bestSpawn != null)
            {
                if (!free)
                {
                    pd[playerID].deaths++;
                }
                pd[playerID].player.transform.position = bestSpawn.transform.position;

                Vector3 tempRotation = pd[playerID].player.transform.rotation.eulerAngles;
                tempRotation.y = Random.Range(0.0f, 360.0f);
                pd[playerID].player.transform.eulerAngles = tempRotation;

                return true;
            }
            return false;
        }

        public void addDamageToPD(float damage, int player)
        {
            pd[player].damageDone += damage;
        }

        public void addKillToPD(int player)
        {
            pd[player].kills++;
        }
        public void addSuicideToPD(int player)
        {
            pd[player].kills--;
        }

    }

}