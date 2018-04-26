using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Nicolas Smith
// Purpose:		fire function of the tank.
// Namespace:	GG
//
//============================================================================//


namespace GG
{
    public class FireProjectile : MonoBehaviour
    {
        public float projectileForce = 0f;
        public GameObject shell;
        public float fireRate = 0f;
        private float fireRateTimeStamp = 0f;
        private TurretRotation turretRotation;
		public GameObject parentObj;
        public ParticleSystem Explosion;

        Player controller;


        void Start()
        {
            turretRotation = GetComponent<TurretRotation>();

            //get the controller
            controller = turretRotation.GetPlayer();

			parentObj = gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (controller.GetButton("Shoot"))
            {
                if (GetComponent<BasicHealthTest>().isShielded == false)
                {
                    if (Time.time > fireRateTimeStamp)
                    {
                        Explosion.Emit(100);
                        GameObject newShell = (GameObject)Instantiate(shell, turretRotation.getAimPointer().transform.position + turretRotation.getActualAimForward(), turretRotation.getAimPointer().transform.rotation);

                        newShell.GetComponent<ProjectileBehaviour>().parent = this;

                        newShell.transform.LookAt(turretRotation.getActualAimForward() * 1.5f + turretRotation.getAimPointer().transform.position);
                        newShell.GetComponent<Rigidbody>().velocity = turretRotation.getActualAimForward() * projectileForce;
                        fireRateTimeStamp = Time.time + fireRate;
                    }
                }
            }
        }
    }

}