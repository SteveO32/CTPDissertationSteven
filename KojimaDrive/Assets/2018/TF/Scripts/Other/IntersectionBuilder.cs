using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - Team Frivolous 2018 ====================//
//
// Author:      Chloe Goument
// Purpose:     Handles the picking of trophies as they spawn.
// Namespace:   TF
//
//===============================================================================//

namespace TF
{
    public class IntersectionBuilder : MonoBehaviour
    {
        private float arena_length = 5;

        private float road_length = 1;
        private float road_width = 1;

        public float arena_width;
        public int number_of_lanes;
        public int number_of_players;

        public Material grass_mat;
        public Material road_mat;

        public GameObject Trophy_Prefab;

        public List<GameObject> cars;

        private List<GameObject> Splines;
        private int maxCars;

        // Use this for initialization
        private void Start()
        {
            Splines = new List<GameObject>();
            //// Build floor
            //GameObject ArenaFloor = GameObject.CreatePrimitive((PrimitiveType.Plane));
            //ArenaFloor.transform.SetParent(GameObject.Find("Frogger").transform);
            //ArenaFloor.transform.localScale = new Vector3(arena_width, 1.0f, arena_length);
            //ArenaFloor.transform.localPosition = new Vector3(0.0f, 0.0f, (arena_length/2.0f*8.0f));
            //ArenaFloor.GetComponent<Renderer>().material = grass_mat;
            //ArenaFloor.name = "Arena Floor";
            road_length = arena_width;

            // Add Roads
            //RoadBuilder(number_of_roads);

            // Set Ending point
            GameObject.Find("Frogger/RaceEndPoint").transform.localPosition = new Vector3(0.0f, 0.0f, arena_length * 6.0f);

            // Add players

            // Spline Generator
            SplineGenerator();

            // Add Vehicles.
            InvokeRepeating("CarSpawner", 2.0f, 2.5f);
            //CarSpawner();

            // Trophy Spawning
            TrophySpawner(number_of_players);
        }

        private void RoadBuilder(int roads)
        {
            for (int i = 0; i < number_of_lanes; i++)
            {
                GameObject Arena = GameObject.Find("Frogger");
                GameObject Road = GameObject.CreatePrimitive((PrimitiveType.Plane));
                Road.name = "Road " + i;
                Road.transform.SetParent(Arena.transform);
                Road.transform.localScale = new Vector3(road_length, 1.0f, road_width / 2);
                Road.transform.localPosition = new Vector3(0.0f, 0.01f, ((i * 10) + road_width));
                Road.GetComponent<Renderer>().material = road_mat;
                Road.GetComponent<Collider>().Destroy();
            }
        }

        private void SplineGenerator()
        {
            for (int i = 0; i < number_of_lanes; i++)
            {
                GameObject currentspline = new GameObject("Track " + i);
                currentspline.transform.SetParent(GameObject.Find("Frogger").transform);
                for (int x = 0; x < 6; x++)
                {
                    GameObject currenttrack = new GameObject("Point " + x);
                    currenttrack.transform.eulerAngles = new Vector3(0, 0, 0);
                    currenttrack.transform.SetParent(GameObject.Find("Track " + i).transform);

                    if (x == 0)
                        currenttrack.transform.localPosition = GameObject.Find("TrafficStart").transform.position + new Vector3 (6 * i, 0, 0);

                    if (x == 1)
                        currenttrack.transform.localPosition = GameObject.Find("TrafficCornerLeft").transform.position + new Vector3(6 * i, 0, 0);

                    if (x == 2)
                        currenttrack.transform.localPosition = GameObject.Find("TrafficTurnLeft").transform.position + new Vector3(6 * i, 0, 0);

                    if (x == 3)
                         currenttrack.transform.localPosition = GameObject.Find("TrafficTurnRight").transform.position + new Vector3(6 * i, 0, 0);

                    if (x == 4)
                        currenttrack.transform.localPosition = GameObject.Find("TrafficCornerRight").transform.position + new Vector3(6 * i, 0, 0);

                    if (x == 5)
                        currenttrack.transform.localPosition = GameObject.Find("TrafficEnd").transform.position + new Vector3(6 * i, 0, 0);
                }

                Splines.Add(currentspline);
            }
        }

        private void CarSpawner()
        {

            //for (int i = 0; i < cars.Count; i++)
            //{
            //    Vector3 position = GameObject.Find("TrafficStart").transform.position;
            //    GameObject currentcar = Instantiate(cars[i], new Vector3 (position.x + 5 * i, position.y, position.z), Quaternion.Euler(0, 0, 0));
            //    currentcar.transform.SetParent(GameObject.Find("Frogger").transform);
            //    currentcar.name = "Car";
            //    currentcar.tag = "FroggerCar";
            //    currentcar.GetComponent<CarEngine>().path = Splines[i].transform;
            //}
            if (maxCars < 3)
            {
                Vector3 position = GameObject.Find("TrafficStart").transform.position;
                GameObject currentcar = Instantiate(cars[0], new Vector3(position.x, position.y, position.z), Quaternion.Euler(0, 0, 0));
                currentcar.transform.SetParent(GameObject.Find("Frogger").transform);
                currentcar.name = "Car";
               // currentcar.tag = "FroggerCar";
                currentcar.GetComponent<CarEngine>().path = Splines[0].transform;
                maxCars++;
            }

        }

        public void TrophySpawner(int playernum)
        {
            for (int i = 0; i != playernum; i++)
            {
                GameObject Trophy = Instantiate(Trophy_Prefab, GameObject.Find("RaceEndPoint").transform.position, Quaternion.Euler(0, 0, 0));
                Trophy.GetComponent<Trophy>().place = i;
                //Trophy.transform.SetParent(GameObject.Find("Frogger").transform);
                //switch (playernum)
                //{
                //    case 1:
                //        Trophy.transform.localPosition += new Vector3(-10.0f, 5.0f, 0.0f);
                //        break;

                //    case 2:
                //        Trophy.transform.localPosition += new Vector3(-5.0f, 5.0f, 0.0f);
                //        break;

                //    case 3:
                //        Trophy.transform.localPosition += new Vector3(5.0f, 5.0f, 0.0f);
                //        break;

                //    default:
                //        Trophy.transform.localPosition += new Vector3(10.0f, 5.0f, 0.0f);
                //        break;
                //}
            }
        }
    }
}