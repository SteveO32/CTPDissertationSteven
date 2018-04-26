using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Connor Rhone, Josh Fenlon
// Purpose:		Set up a camera to follow an object
// Namespace:	GG
//
//============================================================================//

namespace GG
{
    public class TankCamera : MonoBehaviour
    {
        public TurretRotation tank;
		public bool oneTank = false;
        private GameObject objectToFollow = null;
        public Vector3 offset = new Vector3(1, 1, 1);
        public bool useForward = false;
        public Vector3 forwardOffset = new Vector3(1, 1, 1);
        public float moveSmooth = 1.0f;
        public float rotateSmooth = 1.0f;

        private float initMoveSmooth = 0.0f;
        private float initRotateSmooth = 0.0f;
        private float pauseTimer = 0.35f;
        private float accel = 0.0f;

        // Use this for initialization
        void Start()
        {
            objectToFollow = tank.getAimPointer();
            int playerID = tank.GetPlayerID();
            Camera cam = GetComponent<Camera>();

            int player1 = 1 << 31;
            int player2 = 1 << 30;
            int player3 = 1 << 29;
            int player4 = 1 << 28;

            //set up the camera for splitscreen stuff
			if (!oneTank)
            {
				switch (playerID)
                {
				    case 0:
					    cam.rect = new Rect (0.0f, 0.5f, 0.5f, 0.5f);
                        cam.cullingMask = ~(player2 | player3 | player4);
                        transform.parent.GetComponentInChildren<LineRenderer>().gameObject.layer = 31;
					    break;
				    case 1:
					    cam.rect = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
                        cam.cullingMask = ~(player1 | player3 | player4);
                        transform.parent.GetComponentInChildren<LineRenderer>().gameObject.layer = 30;
                        break;
				    case 2:
					    cam.rect = new Rect (0.0f, 0.0f, 0.5f, 0.5f);
                        cam.cullingMask = ~(player1 | player2 | player4);
                        transform.parent.GetComponentInChildren<LineRenderer>().gameObject.layer = 29;
                        break;
				    case 3:
					    cam.rect = new Rect (0.5f, 0.0f, 0.5f, 0.5f);
                        cam.cullingMask = ~(player1 | player2 | player3);
                        transform.parent.GetComponentInChildren<LineRenderer>().gameObject.layer = 28;
                        break;
				}
			}
            else
            {
				cam.rect = new Rect (0, 0, 1, 1);
			}
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(initMoveSmooth < moveSmooth && pauseTimer <= 0.0f)
            {
                accel += Time.deltaTime * 2.0f;
                initMoveSmooth += Time.deltaTime * accel;
                if(initMoveSmooth > moveSmooth)
                {
                    initMoveSmooth = moveSmooth;
                }
            }

            if (initRotateSmooth < rotateSmooth && pauseTimer <= 0.0f)
            {
                accel += Time.deltaTime * 2.0f;
                initRotateSmooth += Time.deltaTime * accel;
                if (initRotateSmooth > rotateSmooth)
                {
                    initRotateSmooth = rotateSmooth;
                }
            }

            if(pauseTimer > 0.0f)
            {
                pauseTimer -= Time.deltaTime;
            }

            if (objectToFollow == null) return;
            if (useForward)
            {
                //transform.rotation = objectToFollow.transform.rotation;
                Vector3 newPos = objectToFollow.transform.position + offset +
                    new Vector3(forwardOffset.x * objectToFollow.transform.forward.x,
                    forwardOffset.y * objectToFollow.transform.forward.y, forwardOffset.z * objectToFollow.transform.forward.z);
                //Vector3 distance = newPos - transform.position;

                transform.position = Vector3.Lerp(transform.position, newPos, initMoveSmooth * Time.deltaTime);
                //transform.position = GG.Functions.vec3SmoothStep(transform.position, newPos, moveSmooth);
                //transform.position += distance / moveSmooth;
                //transform.LookAt(transform.position - objectToFollow.transform.forward);

                Quaternion oldRot = transform.rotation;
                transform.LookAt(transform.position - objectToFollow.transform.forward);

                Quaternion newRot = transform.rotation;
                transform.rotation = oldRot;

                newRot = Quaternion.Euler(0.0f, newRot.eulerAngles.y, 0.0f);

                transform.rotation = Quaternion.Lerp(oldRot, newRot, initRotateSmooth * Time.deltaTime);
            }
            else
            {
                transform.position = objectToFollow.transform.position + offset;
            }
        }
    }
}
