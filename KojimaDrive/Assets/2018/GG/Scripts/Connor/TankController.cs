using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Connor Rhone
// Purpose:		Handles the tank's movement
// Namespace:	GG
//
//============================================================================//

namespace GG
{
    public class TankController : MonoBehaviour
    {
        public Rigidbody tank;
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public WheelCollider frontWheel;
        public WheelCollider backWheel;
        public Transform flipper;
        public float maxMotorTorque;
        public float maxSteerTorque;
		public bool canDrive = true;

        float maxSteeringAngle;
        float movementDir = 0.0f;
        float flipping = 0.0f;
        float flipThreshold = 0.0f;
        bool startFlip = false;
        Player controller;

        float threshold = 0.05f;
        float rightOff, leftOff;

        public ParticleSystem leftTread, rightTread;

        private void Start()
        {
            controller = GetComponent<TurretRotation>().GetPlayer();
            rightOff = 0.0f;
            leftOff = 0.0f;
        }

        public void Update()
        {
            CleanupTrails();

            flipper.localScale = new Vector3(0.2f + flipping * 4.0f, 0.2f + flipping * 6.0f, 0.2f + flipping * 4.0f);
            flipper.localPosition = new Vector3(0.0f, 0.9f + flipping * 3.0f, 0.0f);

            if(flipping > 0.0f && !startFlip)
            {
                flipping -= Time.deltaTime;
            }

            if(startFlip)
            {
                flipping += Time.deltaTime;
                if(flipping >= 0.25f)
                {
                    flipping = 0.25f;
                    startFlip = false;
                }
            }

            float motor = maxMotorTorque * controller.GetAxis("Gas");
            float steering = controller.GetAxis("Steer");

			if (!canDrive) motor = 0;
			if (!canDrive) steering = 0;

            movementDir += motor * 3.0f * Time.deltaTime;

            if (Vector3.Angle(Vector3.up, transform.up) >= 120.0f)
            {
                flipThreshold += Time.deltaTime;
                if (flipping <= 0.0f && !startFlip && flipThreshold >= 1.0f)
                {
                    flipping = 0.0f;
                    startFlip = true;
                }
            }
            else if(flipThreshold > 0.0f)
            {
                flipThreshold -= Time.deltaTime;
            }

            tank.AddRelativeForce(new Vector3(0.0f, 0.0f, motor * 5.0f));

            if (movementDir < -maxMotorTorque) movementDir = -maxMotorTorque;
            if (movementDir > maxMotorTorque) movementDir = maxMotorTorque;

            frontWheel.brakeTorque = 0.0f;
            backWheel.brakeTorque = 0.0f;

            if (motor == 0)
            {
                leftWheel.wheelDampingRate = 1.0f;
                rightWheel.wheelDampingRate = 1.0f;
                frontWheel.wheelDampingRate = 1.0f;
                backWheel.wheelDampingRate = 1.0f;
                motor = Mathf.Abs(steering) * maxSteerTorque;
                maxSteeringAngle = 90.0f;
                if (motor == 0)
                {
                    frontWheel.brakeTorque = 1.0f;
                    backWheel.brakeTorque = 1.0f;
                }
            }
            else
            {
                maxSteeringAngle = 10.0f;
                leftWheel.wheelDampingRate = 0.5f;
                rightWheel.wheelDampingRate = 0.5f;
                frontWheel.wheelDampingRate = 0.5f;
                backWheel.wheelDampingRate = 0.5f;
            }

            if (motor > maxMotorTorque)
            {
                motor = maxMotorTorque;
            }
            if (motor < -maxMotorTorque)
            {
                motor = -maxMotorTorque;
            }

            frontWheel.motorTorque = motor;
            backWheel.motorTorque = motor;

            frontWheel.steerAngle = steering * maxSteeringAngle;
            backWheel.steerAngle = -steering * maxSteeringAngle;

            leftWheel.motorTorque = motor;
            leftWheel.brakeTorque = 0.0f;
            leftWheel.brakeTorque = 0;

            rightWheel.motorTorque = motor;
            rightWheel.brakeTorque = 0.0f;
            rightWheel.brakeTorque = 0;

            if ((motor > 0 && movementDir < 0) || (motor < 0 && movementDir > 0))
            {
                frontWheel.brakeTorque = 10000.0f;
                backWheel.brakeTorque = 10000.0f;
                leftWheel.brakeTorque = 10000.0f;
                rightWheel.brakeTorque = 10000.0f;
            }

            //Debug.Log(movementDir);

            if (movementDir > 0 && controller.GetAxis("Reverse") == 0) movementDir -= Time.deltaTime * maxMotorTorque;
            if (movementDir < 0 && controller.GetAxis("Reverse") == 0) movementDir += Time.deltaTime * maxMotorTorque;

            if (steering < 0)
            {
                //rightWheel.motorTorque *= 2.0f;
                if (controller.GetAxis("Reverse") == 0)
                {
                    leftWheel.motorTorque *= -1.0f;
                }
                else
                {
                    leftWheel.motorTorque *= 0.0f;
                }
            }
            if (steering > 0)
            {
                //leftWheel.motorTorque *= 2.0f;
                if (controller.GetAxis("Reverse") == 0)
                {
                    rightWheel.motorTorque *= -1.0f;
                }
                else
                {
                    rightWheel.motorTorque *= 0.0f;
                }
            }
        }

        void CleanupTrails()
        {
            //leftTread.startRotation3D = new Vector3(-90.0f, 90.0f, 0.0f) + transform.rotation.eulerAngles;
            //rightTread.startRotation3D = new Vector3(-90.0f, 90.0f, 0.0f) + transform.rotation.eulerAngles;

            WheelHit hit;
            bool front, back;
            front = frontWheel.GetGroundHit(out hit);
            back = backWheel.GetGroundHit(out hit);
            if (!rightWheel.GetGroundHit(out hit) && !(back || front))
            {
                if (rightOff >= threshold)
                {
                    rightTread.Pause();
                    rightOff = threshold;
                }
                rightOff += Time.deltaTime;
            }
            else
            {
                rightTread.Play();
                if(rightOff > 0.0f)
                {
                    rightOff -= Time.deltaTime;
                    if (rightOff < 0.0f) rightOff = 0.0f;
                }
            }
            if(!leftWheel.GetGroundHit(out hit) && !(back || front))
            {
                if (leftOff >= threshold)
                {
                    leftTread.Pause();
                    leftOff = threshold;
                }
                leftOff += Time.deltaTime;
            }
            else
            {
                leftTread.Play();
                if (leftOff > 0.0f)
                {
                    leftOff -= Time.deltaTime;
                    if (leftOff < 0.0f) leftOff = 0.0f;
                }
            }
        }
    }
}