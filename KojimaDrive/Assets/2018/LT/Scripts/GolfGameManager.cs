using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{

    public class GolfGameManager : KojimaParty.GameMode
    {
        [SerializeField]
        CourseManager courseManager;

        [SerializeField]
        float roundMaxTime;

        [SerializeField]
        bool chain;

        [SerializeField]
        int numberOfPlayers;

        List<GameObject> playersReferences;

        static Vector3[] playerDisplacement = { new Vector3(-7.5f, 0, 0),
                                                new Vector3(-2.5f, 0, 0),
                                                new Vector3( 2.5f, 0, 0),
                                                new Vector3( 7.5f, 0, 0)};

        List<int> scores;
        int playersScored = 0;

        [SerializeField]
        float linkDistance;
        [SerializeField]
        float linksNum;

        public GameObject Link;
        public GameObject Ball;
        public GameObject Player;

        [SerializeField]
        List<int> cameraLayers;
        [SerializeField]
        List<LayerMask> cameraCullingMasks;
        [SerializeField]
        List<Rect> cameraViewports;

        [SerializeField]
        List<GameObject> PowerUps;
        [SerializeField]
        List<Transform> PowerUpSpawnPoints;

        [SerializeField]
        GameObject powerUpSpawnEffect;

        float startTime;

        private List<int> playerIDS;

        public override void StartGameMode()
        {
            playersReferences = new List<GameObject>();
            playerIDS = new List<int>();
            courseManager.StartGame();
            SpawnPlayers(courseManager.currentStart.transform, courseManager.currentHole.transform);
            SetUpUIElements();
            scores = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                scores.Add(0);
            }

            startTime = Time.time;

        }

        void SetUpUIElements()
        {
            foreach (GameObject player in playersReferences)
            {
                player.GetComponentInChildren<UIManager>().SetNumberOfPlayers(numberOfPlayers);
                player.GetComponentInChildren<UIManager>().SetUpUIManager();
                player.GetComponentInChildren<UIManager>().warningText_reference.SetWarningText("Stand Up!");
            }
            StartCoroutine(startGameCounter());
        }
        
        public void BallInHole(GameObject ball)
        {
            int playerScoring = ball.transform.root.GetComponentInChildren<HumanCharacterControl>().playerId;

            LT.Score.instance.ChangeScore(4-playersScored, playerScoring);

            playersScored++;
            scores[playerScoring]+= playersScored;


            if (playersScored == 3)
            {
                GameEnd();
            }
        }

        public void GameEnd()
        {
            GameModeFinished(scores[0], scores[1], scores[2], scores[3]);
        }

        public void Update()
        {
            if (Time.time - startTime >= roundMaxTime)
            {
                GameEnd();
            }
        }

        IEnumerator startGameCounter()
        {
            yield return new WaitForSeconds(7.0f);

            foreach (GameObject player in playersReferences)
                player.GetComponentInChildren<UIManager>().countdown_reference.ActivateCountdown();

			yield return new WaitForSeconds(3.0f);

			foreach (GameObject player in playersReferences) 
				player.GetComponentInChildren<HumanCharacterInput> ().enabled = true;

        }

        IEnumerator spawningCounter()
        {
            while (true)
            {
                yield return new WaitForSeconds(10.0f);
                spawnPowerUp();
            }
            yield return null;
        }

        void spawnPowerUp()
        {
            Vector3 pos = PowerUpSpawnPoints[CourseManager.holeNum].GetChild(Random.Range(0, PowerUpSpawnPoints[CourseManager.holeNum].childCount)).position;

            GameObject powerUp = Instantiate(PowerUps[Random.Range(0, PowerUps.Count)], pos, Quaternion.identity);
            Destroy(Instantiate(powerUpSpawnEffect, pos, Quaternion.identity), 5.0f);
        }

        //This is moved here because i wanted to have the player references in the manager script and have the Course only manage the holes and spawn point
        //then it will also handle all the aesthetics that go on the course (Windmill etc.)
        void SpawnPlayers(Transform startPoint, Transform hole)
        {
            //Make sure there are no residual players (Allows to go directly between  holes without reloading the scene)
            if (playersReferences.Count != 0)
            {
                foreach (var p in playersReferences)
                {
                    DestroyImmediate(p);
                }
            }
            playersReferences = new List<GameObject>();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                //--- Position and Balls and Chains ----

                //Create the spawn point
                GameObject spawnPoint = new GameObject();
                spawnPoint.name = "Player " + i;
                spawnPoint.transform.position = startPoint.position;
                spawnPoint.transform.LookAt(new Vector3(hole.position.x, spawnPoint.transform.position.y, hole.position.z));

                Vector3 displacement = new Vector3(0, 0, linkDistance);

                //spawn player
                GameObject tempObj = Instantiate(Player, playerDisplacement[i], Quaternion.identity, spawnPoint.transform);

                tempObj.GetComponentInChildren<PlayerMaterialsApplier>().playerIndex = i;
				tempObj.GetComponentInChildren<HumanCharacterInput> ().enabled = false; 
                tempObj.transform.localPosition = playerDisplacement[i];
                tempObj.transform.localRotation = Quaternion.identity;
                playersReferences.Add(tempObj);
                //Give Player the right ID
                tempObj.GetComponentInChildren<HumanCharacterControl>().playerId = i;

                if (chain)
                {
                    //Spawn Chain
                    for (int j = 0; j < linksNum; j++)
                    {
                        GameObject link = Instantiate(Link, Vector3.zero, Quaternion.identity, spawnPoint.transform);
                        link.transform.localPosition = playerDisplacement[i] + displacement;
                        link.transform.localRotation = Quaternion.identity;
                        //TODO: This needs changing
                        link.layer = 4;
                        link.tag = "IgnoreMe";
                        if (j == 0)
                        {
                            //firstLink = link;
                            link.GetComponent<CharacterJoint>().connectedBody =
                            tempObj.gameObject.GetComponentInChildren<LegTag>().GetComponent<Rigidbody>();
                        }
                        else
                        {
                            link.GetComponent<CharacterJoint>().connectedBody = tempObj.GetComponent<Rigidbody>();
                        }
                        //This bit isn't enough, needs to ignore all collisions with GameCharacter & its children
                        //Physics.IgnoreCollision(link.GetComponent<Collider>(), playerRef.transform.Find("GameCharacter").GetComponent<Collider>());
                        tempObj = link;
                        displacement += new Vector3(0, 0, linkDistance);
                    }
                }
                displacement += new Vector3(0, 1f, linkDistance * 8);
                //Spawn ball and link to last link 
                GameObject ball = Instantiate(Ball, Vector3.zero, Quaternion.identity, spawnPoint.transform);


                ball.GetComponent<PlayerMaterialsApplier>().playerIndex = i;

                ball.GetComponent<GolfBallIDS>().SetID(i);
                ball.transform.localPosition = playerDisplacement[i] + displacement;
                ball.transform.localRotation = Quaternion.identity;
                if (chain)
                {
                    ball.AddComponent<CharacterJoint>();
                    ball.GetComponent<CharacterJoint>().connectedBody = tempObj.GetComponent<Rigidbody>();
                }


                //--- Cameras ----
                playersReferences[i].GetComponentInChildren<Camera>().gameObject.layer = cameraLayers[i];
                playersReferences[i].GetComponentInChildren<Camera>().cullingMask = cameraCullingMasks[i];
                playersReferences[i].GetComponentInChildren<Camera>().rect = cameraViewports[i];
                playersReferences[i].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject.layer = cameraLayers[i];

            }
        }
    }
}