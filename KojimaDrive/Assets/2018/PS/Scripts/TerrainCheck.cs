using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    /// <summary>
    /// WATER needs trigger collider, tag water, layer Water
    /// LAND needs mesh collider, tag LEVEL, layer Land
    /// </summary>
    public class TerrainCheck : MonoBehaviour
    {
        [Range(0, 100)]
        public int areaRadius = 5;

        [Range(0, 30)]
        public int rayNum = 5; // no. of raycast returns

        [Range(0, 100)]
        public float waterThreshold = 0; //max % of water

        [Range(40, 200)]
        public float thresholdHeightDiff = 2.0f; //Max height difference to make viable area

        public LayerMask includeMask; //should contain Water and Land

        public GameObject arena = null;

        public GameObject[] defaultSpawnPoints;

        public LayerMask watermask;

        public Vector3 hitCenter = Vector3.zero;
        //SMALL ISLAND
        private int maxZ = 3700; // Top Z boundary of land
        private int minZ = -3700; // bottom Z boundary of land
        private int maxX = 3700; // Top X boundary of land
        private int minX = -3700; // bottom X boundary of land

        private float maxDist;
        private float minDist;
        private float randX;
        private float randZ;
        private int landNum;
        private int waterNum;
        private Vector3 arenaCenter;

        private bool validLocation = false;

        private int seedNum = 0;

        //hitCenter made public in order for GM Manager to respawn cars between rounds

        private System.Random rnd = new System.Random();

        private int DefaultLocationCounter = 0;
        private GamemodeManager _GM;

      
        /// <summary>
        //GROUND tag == LEVEL
        //Ground Layer == DEFAULT
        //Use this for initialization
        //Water tagged Water
        private void Start()
        {
            _GM = GameObject.Find("GameModeManager").GetComponent<GamemodeManager>();
            maxDist = 0.0f;
            minDist = 10000.0f;
        }

        /// First function to call to set new location
        /// </summary>
        public void SelectArenaLocation()
        {
            seedNum = rnd.Next(0, 5);

            switch (seedNum)
            {
                case 0:
                    arena = Instantiate(arena, defaultSpawnPoints[0].transform.localPosition, Quaternion.identity);
                    break;
                case 1:
                    arena = Instantiate(arena, defaultSpawnPoints[1].transform.localPosition, Quaternion.identity);
                    break;
                case 2:
                    arena = Instantiate(arena, defaultSpawnPoints[2].transform.localPosition, Quaternion.identity);
                    break;
                case 3:
                    arena = Instantiate(arena, defaultSpawnPoints[3].transform.localPosition, Quaternion.identity);
                    break;
                case 4:
                    arena = Instantiate(arena, defaultSpawnPoints[4].transform.localPosition, Quaternion.identity);
                    break;
                case 5:
                    arena = Instantiate(arena, defaultSpawnPoints[5].transform.localPosition, Quaternion.identity);
                    break;
                default:
                    break;
            }
        }

        //Main loop to return valid location
        void Update()
        {
            if (!validLocation)
            {
                SetRandomArena();
                CheckGround(arenaCenter);
            }
        }

        private void SetRandomArena()
        {
            randX = rnd.Next(minX, maxX);
            randZ = rnd.Next(minZ, maxZ);
            transform.position = new Vector3(randX, 1000.0f, randZ);
            arenaCenter = new Vector3(randX, 1000.0f, randZ);
        }

        private bool CheckGround(Vector3 center)
        {
            Vector3 origin = center;
            landNum = 0;
            waterNum = 0;
            maxDist = 0.0f;
            minDist = 10000.0f;
            //Raycast Down
            for (int i = 0; i < rayNum; i++)
            {

                RaycastHit hit;

                if (i > 0)
                {
                    origin = RandomCircle(center, (float)i / (float)rayNum - 1);
                }

              
                DrawRays(origin);

                if (Physics.Raycast(origin, -Vector3.up, out hit, Mathf.Infinity, includeMask))
                {
                    
                    DistanceCheck(hit.distance);
                    WaterCheck(hit.collider.tag);

                    if (i == 0) // ray 0 s in the center
                    {
                        hitCenter = hit.point;
                    }
     
                }
               // }

            }
            Debug.Log("Min: " + minDist + " Max:  " + maxDist + " \nLand %: " + (landNum / rayNum) * 100 + " Water %: " + (waterNum / rayNum) * 100);

            //CHECK MINIMUM VS MAXIMUM
            if (maxDist - minDist < thresholdHeightDiff && (landNum / rayNum) * 100 > waterThreshold) //&& water.fillpercent > threshold landValue
            {
                //if takes more than x tries
                Debug.Log("Valid location");
                validLocation = true;
                Debug.Log(hitCenter);
                _GM.SpawnCars(hitCenter);
                arena = Instantiate(arena, hitCenter, Quaternion.identity);
                return true;
            }
            else
            {
                DefaultLocationCounter++;
                if (DefaultLocationCounter >= 100)
                {
                    Debug.Log("Default pos spawned");
                    SelectArenaLocation();
                    _GM.SpawnCars(hitCenter);
                    validLocation = true;
                    return true;
                }
                else
                {
                    //Debug.Log("Invalid location");
                    validLocation = false;
                    return false;
                }
            }
        }

        private void DistanceCheck(float hitDistance)
        {
            if (hitDistance > maxDist)
            {
                maxDist = hitDistance;
            }
            if (hitDistance < minDist)
            {
                minDist = hitDistance;
            }
        }

        private void WaterCheck(string hitTag)  //need to edit tags or layers for this to work
        {
            if (hitTag == "LEVEL")
            {
                landNum++;
            }
            if (hitTag == "Water")
            {
                waterNum++;
            }
        }

        private void DrawRays(Vector3 origin)
        {
            Debug.DrawRay(origin, -Vector3.up, Color.green, 30.0f);
        }

        //returns points around a circle at spacing multiplier
        private Vector3 RandomCircle(Vector3 center, float multiplier)
        {
            // create random angle between 0 to 360 degrees
            float ang = multiplier * 360;
            Vector3 rayPos;
            rayPos.x = center.x + areaRadius * Mathf.Sin(ang * Mathf.Deg2Rad);
            rayPos.y = center.y;
            rayPos.z = center.z + areaRadius * Mathf.Cos(ang * Mathf.Deg2Rad);
            return rayPos;
        }
    }
}