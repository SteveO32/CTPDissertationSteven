using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public class BumperControls : MonoBehaviour
    {
        public GameObject leftBumper;
        public GameObject rightBumper;
        private FH.LandVehicle carControls;

        public float turnVelocity = 500;
        public float limit = 0;
        private Vector3 leftEndRotation = new Vector3(0, 0, 0);
        private Vector3 rightEndRotation = new Vector3(0, 359, 0);
        private Vector3 leftStartRotation = new Vector3(0, 60, 0);
        private Vector3 rightStartRotation = new Vector3(0, -60, 0);
        private string playerID;



        private BumperAction leftAction = BumperAction.BUMPER_IN;
        private BumperAction rightAction = BumperAction.BUMPER_IN;


        // Use this for initialization

       public enum BumperAction
        {
            BUMPER_OUT,
            BUMPER_IN,
            BUMPER_IDLE
        }

        public void Init(GameObject rightBumperPrefab, GameObject leftBumperPrefab)
        {
            leftBumper = Instantiate(leftBumperPrefab, transform.position, Quaternion.identity) as GameObject;
            rightBumper = Instantiate(rightBumperPrefab, transform.position, Quaternion.identity) as GameObject;

            leftBumper.transform.parent = transform;
            rightBumper.transform.parent = transform;

            leftBumper.transform.localPosition = new Vector3(-0.92f, 0.478f, 1.77f);
            rightBumper.transform.localPosition = new Vector3(0.92f, 0.478f, 1.77f);

            carControls = this.GetComponent<FH.LandVehicle>(); ;

            switch(carControls.ControllerID)
            {
                case 0: playerID = "Joy1";
                    break;
                case 1: playerID = "Joy2";
                    break;
                case 2: playerID = "Joy3";
                    break;
                case 3: playerID = "Joy4";
                    break;
                default:
                    break;
            }




            Debug.Log(carControls.ControllerID + "    helloooooo");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown(playerID + "Button4"))
            {
                leftAction = BumperAction.BUMPER_OUT;
            }
            if (Input.GetButtonDown(playerID + "Button5"))
            {
                rightAction = BumperAction.BUMPER_OUT;
            }

            leftBumperPress();
            rightBumperPress();
        }


        public void setBumperStateOff(string _flipperName)
        {
            if(_flipperName == "LeftCollider")
            {
                leftAction = BumperAction.BUMPER_IN;
            }

            if(_flipperName == "RightCollider")
            {
                rightAction = BumperAction.BUMPER_IN;
            }
        }



		public bool checkBumperState(string _flipperName)
		{
			if (_flipperName == "LeftCollider" && leftAction == BumperAction.BUMPER_OUT) {
				return true;
			} else if (_flipperName == "RightCollider" && rightAction == BumperAction.BUMPER_OUT) {
				return true;
			} else {
				return false;
			}

		}


        void leftBumperPress()
        {
            if (leftAction == BumperAction.BUMPER_OUT)
            {
                if (leftBumper.transform.localRotation.y > 0)
                {
                    leftBumper.transform.Rotate(-Vector3.up * Time.deltaTime * turnVelocity);
                }
                else
                {
                    leftBumper.transform.localEulerAngles = leftEndRotation;
                    leftAction = BumperAction.BUMPER_IN;
                }
            }

            if (leftAction == BumperAction.BUMPER_IN)
            {
                if (leftBumper.transform.localEulerAngles.y < 60)
                {
                    leftBumper.transform.Rotate(Vector3.up * Time.deltaTime * turnVelocity);
                }
                else
                {
                    leftAction = BumperAction.BUMPER_IDLE;
                    leftBumper.transform.localEulerAngles = leftStartRotation;
                }

            }
        }

        void rightBumperPress()
        {
            if (rightAction == BumperAction.BUMPER_OUT)
            {
                if (rightBumper.transform.localRotation.y < 0)
                {
                    rightBumper.transform.Rotate(Vector3.up * Time.deltaTime * turnVelocity);
                }
                else
                {
                    rightBumper.transform.localEulerAngles = rightEndRotation;
                    rightAction = BumperAction.BUMPER_IN;
                }
            }

            if (rightAction == BumperAction.BUMPER_IN)
            {
                //Debug.Log(rightBumper.transform.localEulerAngles.y);

                if (rightBumper.transform.localEulerAngles.y > 300)
                {
                    rightBumper.transform.Rotate(-Vector3.up * Time.deltaTime * turnVelocity);
                }
                else
                {
                    rightAction = BumperAction.BUMPER_IDLE;
                    rightBumper.transform.localEulerAngles = rightStartRotation;
                }

            }
        }

    }
}
