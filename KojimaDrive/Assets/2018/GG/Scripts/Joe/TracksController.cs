using UnityEngine;
using System.Collections;
using Rewired;


//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Joe Plant
// Purpose:		Creates the tank tracks on the ground
// Namespace:	GG
//
//============================================================================//

namespace GG
{
    public class TracksController : MonoBehaviour
    {
        private float offsetL = 0f;
        private float offsetR = 0f;
        public Renderer trackLeft;
        public Renderer trackRight;
        public Rigidbody Rig;
        private Vector3 vel;
        private float speed;
        private bool Front = false;
        private bool Back = false;
        private bool turn = true;

        Player controller;

        private void Start()
        {
            //get the controller
            controller = GetComponentInParent<TurretRotation>().GetPlayer();
        }

        void pressFunc()
        {
            if (controller.GetAxis("Gas") > 0.0f)
            {
                if (speed < 0.3f)
                {
                    Front = true;
                    Back = false;
                }
            }
            if (controller.GetAxis("Gas") < 0.0f)
            {
                if (speed < 0.3f)
                {
                    Back = true;
                    Front = false;
                }
            }
        }

        void Update()
        {
            pressFunc();

            // Tracks rotation
            if (Rig.angularVelocity.magnitude > 0.1f && speed < 1.5f)
            {
                if (controller.GetAxis("Steer") < 0)
                {
                    offsetL = offsetL + speed - 100.00f;
                    offsetR = offsetR - speed + 100.00f;
                }
                if (controller.GetAxis("Steer") > 0)
                {
                    offsetL = offsetL - speed + 100.00f;
                    offsetR = offsetR + speed - 100.00f;
                }
                turn = true;

            }
            else
            {
                turn = false;
            }

            // Tracks move, depends on current speed
            if (speed > 0 || !turn)
            {
                if (Front)
                {
                    offsetL = offsetL - speed / 125;
                    offsetR = offsetR - speed / 125;
                }


                if (Back)
                {
                    offsetL = offsetL + speed / 125;
                    offsetR = offsetR + speed / 125;
                }
            }
            // Speed 
            vel = Rig.velocity;
            speed = vel.magnitude;
            // scrolling
            trackLeft.material.SetTextureOffset("_MainTex", new Vector2(offsetL, 0));
            trackRight.material.SetTextureOffset("_MainTex", new Vector2(offsetR, 0));
        }
    }
}
