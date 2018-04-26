using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Drive - FluffyHedgehog 2018 ====================//
//
// Author:		Viv 
// Purpose:		Visual display for the aircraft gyroscope
// Namespace:	FH
//
//===============================================================================//


namespace FH
{
    public class AircraftGyroscope : MonoBehaviour {

        public GameObject player;
        public GameObject gyroscope;

        // Use this for initialization
        void Start()
        {
            //player = GameObject.FindGameObjectWithTag("Player");
            //gyroscope = GameObject.FindGameObjectWithTag("Gyroscope");
        }

        // Update is called once per frame
        void Update()
        {
            // TODO: Add a waring message.
            if(!player) return;
            if(!gyroscope) return;

            float euler_z = player.transform.eulerAngles.z;
            gyroscope.transform.eulerAngles = new Vector3(gyroscope.transform.eulerAngles.x, gyroscope.transform.eulerAngles.y, euler_z);
        }
    }
}
