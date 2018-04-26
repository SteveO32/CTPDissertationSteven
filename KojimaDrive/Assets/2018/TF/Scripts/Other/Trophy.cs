using System.Collections;
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
    public class Trophy : MonoBehaviour
    {
        public int place;
        public Material first;
        public Material second;
        public Material third;
        public Material otherwise;

        public GameObject Cup;
        public GameObject Spoon;
        private GameObject playerhand;
        public GameObject GodRayPre;
        // Use this for initialization
        void Start()
        {
            //Spawning in various trophy types.
            switch (place)
            {
                case 1:
                    gameObject.transform.Find("Handle/Trophy").gameObject.SetActive(true);
                    gameObject.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
                    gameObject.transform.Find("Handle/Trophy").gameObject.GetComponent<Renderer>().material = first;
                    gameObject.GetComponent<Rigidbody>().mass = 5;
                    gameObject.transform.position = GameObject.Find("TrophyStand (0)").transform.position;
                    gameObject.tag = "IgnoreMe";

                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.AddComponent<Spinning>();
                    Instantiate(GodRayPre, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                    GodRayPre.tag = "IgnoreMe";
                    break;
                case 2:
                    gameObject.transform.Find("Handle/Trophy").gameObject.SetActive(true);
                    gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    gameObject.transform.Find("Handle/Trophy").gameObject.GetComponent<Renderer>().material = second;
                    gameObject.GetComponent<Rigidbody>().mass = 3;
                    gameObject.transform.position = GameObject.Find("TrophyStand (1)").transform.position;
                    gameObject.tag = "IgnoreMe";
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.AddComponent<Spinning>();
                    Instantiate(GodRayPre, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                    break;
                case 3:
                    gameObject.transform.Find("Handle/Trophy").gameObject.SetActive(true);
                    gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    gameObject.transform.Find("Handle/Trophy").gameObject.GetComponent<Renderer>().material = third;
                    gameObject.GetComponent<Rigidbody>().mass = 1;
                    gameObject.transform.position = GameObject.Find("TrophyStand (2)").transform.position;
                    gameObject.tag = "IgnoreMe";
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.AddComponent<Spinning>();
                    Instantiate(GodRayPre, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                    break;
                default:
                    gameObject.transform.Find("Handle/WoodenSpoon").gameObject.SetActive(true);
                    gameObject.transform.Find("Handle/WoodenSpoon").gameObject.GetComponent<Renderer>().material = otherwise;
                    gameObject.GetComponent<Rigidbody>().mass = 0;
                    gameObject.transform.position = GameObject.Find("TrophyStand (3)").transform.position;
                    gameObject.tag = "IgnoreMe";
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.AddComponent<Spinning>();
                    gameObject.GetComponent<SphereCollider>().radius = 0.05f;
                   // Instantiate(GodRayPre, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
                    break;
            }
        }
        void OnTriggerEnter(Collider other)
        {
            gameObject.GetComponent<Spinning>().Destroy();
            gameObject.layer = 4;
            foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = 4;
            }
        }
    }
}