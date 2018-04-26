using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Josh Fenlon
// Purpose:		Rotate an object around another to create a turret
// Namespace:	GG
//
//============================================================================//

namespace GG
{

    public class TurretRotation : MonoBehaviour
    {

        [Header("Controller Info")]
        [SerializeField]
        private int controllerID = -1;

        [Header("Basic Turret Data")]
        [SerializeField]
        private bool enableTurret = false;
        [SerializeField]
        private GameObject turretChair = null;
        [SerializeField]
        private bool grabStartingRot = false;
        [SerializeField]
        private bool usingLocalYawRot = false;
        [SerializeField]
        private bool usingLocalPitchRot = false;
        [SerializeField]
        private bool addYawToPitch = false;
        [SerializeField]
        private bool grabStartingRotReset = false;

        [Header("Yaw")]
        [SerializeField]
        private GameObject yaw = null;
        [SerializeField]
        private float yawSpeed = 0;
        [SerializeField]
        private turretRotationAxis yawRotType = turretRotationAxis.none;

        [Space(10)]
        [Header("Limit Yaw")]

        [SerializeField]
        private bool enableYawLimit = false;
        [SerializeField]
        private Vector2 minMaxYaw = Vector2.zero;

        [Space(10)]
        [Header("Reset Yaw")]

        [SerializeField]
        private bool enableResetYaw = false;
        [SerializeField]
        private Vector3 resetRotYaw = Vector3.zero;
        [SerializeField]
        private bool useYawSpeedForReset = false;
        [SerializeField]
        private float resetYawTime = 0;
        [SerializeField]
        private Vector3 resetRotYawSpeed = Vector3.zero;

        [Space(20)]
        [Header("Pitch")]

        [SerializeField]
        private GameObject pitch = null;
        [SerializeField]
        private float pitchSpeed = 0;
        [SerializeField]
        private turretRotationAxis pitchRotType = turretRotationAxis.none;

        [Space(10)]
        [Header("Limit Pitch")]

        [SerializeField]
        private bool enablePitchLimit = false;
        [SerializeField]
        private Vector2 minMaxPitch = Vector2.zero;

        [Space(10)]
        [Header("Reset Pitch")]

        [SerializeField]
        private bool enableResetPitch = false;
        [SerializeField]
        private Vector3 resetRotPitch = Vector3.zero;
        [SerializeField]
        private bool usePitchSpeedForReset = false;
        [SerializeField]
        private float resetPitchTime = 0;
        [SerializeField]
        private Vector3 resetRotPitchSpeed = Vector3.zero;

        [Space(20)]
        [Header("Variables")]
        [SerializeField]
        private Vector3 yawRot = Vector3.zero;
        private Vector3 prevYawRot = -Vector3.one;
        [SerializeField]
        private Vector3 pitchRot = -Vector3.zero;
        private Vector3 prevPitchRot = -Vector3.one;

        [Space(20)]
        [Header("Aim pointer")]
        [SerializeField]
        private bool enableAimPointer = false;
        [SerializeField]
        private bool enableDebugView = false;
        [SerializeField]
        private bool enableDebugDirectionView = false;
        [SerializeField]
        private bool pointerSolid = false;
        [SerializeField]
        private float pointerStartingForce = 0;
        [SerializeField]
        private Vector3 pointerGravity = Vector3.zero;
        [SerializeField]
        private bool useUnityGlobalGravity = false;
        [SerializeField]
        private List<Vector3> aimPointerPoints = new List<Vector3>();
        [SerializeField]
        private GameObject aimPointer = null;
        [SerializeField]
        private turretPointerDirection aimDirection = turretPointerDirection.forward;
        [SerializeField]
        private float maxPointerDistance = 1;
        [SerializeField]
        private float recheckPointer = 0;
        private float currentRecheckPointer = 0;
        private RaycastHit pointerHitObject = new RaycastHit();

        [SerializeField]
        private string yawInputAxis = "Turret Yaw";
        [SerializeField]
        private string pitchInputAxis = "Turret Pitch";

        private LineRenderer m_line;

        private Rewired.Player rewiredPlayer;

        //completes all controls for turret
        void controlTurret()
        {

            //====== Basic Variables =====

            bool goBackToReset = true;
            float yawRotChange = 0;
            float pitchRotChange = 0;


            //======= YAW ======


            if (rewiredPlayer != null)
            {
                //check inputs and apply variable changes to yaw
                if (rewiredPlayer.GetAxis(yawInputAxis) != 0.0f)
                {
                    yawRotChange += Time.deltaTime * yawSpeed * rewiredPlayer.GetAxis(yawInputAxis);
                    goBackToReset = false;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    yawRotChange -= Time.deltaTime * yawSpeed;
                    goBackToReset = false;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    yawRotChange += Time.deltaTime * yawSpeed;
                    goBackToReset = false;
                }
            }

            //check if yaw exists
            if (yaw != null)
            {

                yawRot = TurretFunctions.testRotationTypeAndApplyChange(yawRotType, yawRot, yawRotChange, true, minMaxYaw, enableYawLimit);

                //final set for yaw control
                if (usingLocalYawRot)
                {
                    yaw.transform.localEulerAngles = yawRot;
                }
                else
                {
                    yaw.transform.eulerAngles = pitchRot;
                }
            }


            //======= PITCH ======


            if (rewiredPlayer != null)
            {
                //check inputs and apply variable changes to pitch
                if (rewiredPlayer.GetAxis(pitchInputAxis) != 0.0f)
                {
                    pitchRotChange += Time.deltaTime * pitchSpeed * rewiredPlayer.GetAxis(pitchInputAxis);
                    goBackToReset = false;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    pitchRotChange += Time.deltaTime * pitchSpeed;
                    goBackToReset = false;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    pitchRotChange -= Time.deltaTime * pitchSpeed;
                    goBackToReset = false;
                }
            }

            //check if pitch exists
            if (pitch != null)
            {

                pitchRot = TurretFunctions.testRotationTypeAndApplyChange(pitchRotType, pitchRot, pitchRotChange, true, minMaxPitch, enablePitchLimit);

                //final set for pitch control
                if (usingLocalPitchRot)
                {
                    pitch.transform.localEulerAngles = pitchRot;
                }
                else
                {
                    if (addYawToPitch)
                    {
                        pitch.transform.eulerAngles = yawRot + pitchRot;
                    }
                    else
                    {
                        pitch.transform.eulerAngles = pitchRot;
                    }
                }
            }


            //====== Rot Checks ====


            yawRot = RotationFunctions.checkWithin360(yawRot);
            pitchRot = RotationFunctions.checkWithin360(pitchRot);

            //===== Reset Rot ====

            //if reset has been broken calculate new speed of reset
            if (!goBackToReset)
            {
                if (enableResetYaw)
                {
                    if (useYawSpeedForReset)
                    {
                        resetRotYawSpeed = TurretFunctions.returnRotationSpeed(yawRotType, yawSpeed, yawRot, resetRotYaw);
                    }
                    else
                    {
                        resetRotYawSpeed = RotationFunctions.calculateSmoothStepAngle(yawRot, resetRotYaw, resetYawTime);
                    }
                }
                if (enableResetPitch)
                {
                    if (usePitchSpeedForReset)
                    {
                        resetRotPitchSpeed = TurretFunctions.returnRotationSpeed(pitchRotType, pitchSpeed, pitchRot, resetRotPitch);
                    }
                    else
                    {
                        resetRotPitchSpeed = RotationFunctions.calculateSmoothStepAngle(pitchRot, resetRotPitch, resetPitchTime);
                    }
                }
            }

            //move back to origin rotation
            if (goBackToReset)
            {
                if (enableResetYaw)
                {
                    if (TurretFunctions.returnRotationCheck(yawRotType, yawRot, resetRotYaw))
                    {
                        resetRotYawSpeed = Vector3.zero;
                    }
                    else
                    {
                        yawRot += RotationFunctions.checkResetDir(yawRot, resetRotYaw, resetRotYawSpeed) * Time.deltaTime;
                        yawRot = RotationFunctions.checkResetMarginAngle(yawRot, resetRotYaw, resetRotYawSpeed * Time.deltaTime);
                    }
                }
                if (enableResetPitch)
                {
                    if (TurretFunctions.returnRotationCheck(pitchRotType, pitchRot, resetRotPitch))
                    {
                        resetRotPitchSpeed = Vector3.zero;
                    }
                    else
                    {
                        pitchRot += RotationFunctions.checkResetDir(pitchRot, resetRotPitch, resetRotPitchSpeed) * Time.deltaTime;
                        pitchRot = RotationFunctions.checkResetMarginAngle(pitchRot, resetRotPitch, resetRotPitchSpeed * Time.deltaTime);
                    }
                }
            }

            //===== Turret Pointer Check ====

            if (enableAimPointer)
            {
                if (yawRot != prevYawRot || pitchRot != prevPitchRot || currentRecheckPointer < 0)
                {
                    updateTurretPointer();
                    currentRecheckPointer = recheckPointer;
                }
                else
                {
                    currentRecheckPointer -= Time.deltaTime;
                }
            }

            //==== previous variable set ====
            prevPitchRot = pitchRot;
            prevYawRot = yawRot;

        }


        //function deals with handling the visual turret pointer
        void updateTurretPointer()
        {

            //====== Move pointer to towards bounds ======
            aimPointerPoints.Clear();
            pointerHitObject = new RaycastHit();

            Vector3 originalDir = getActualAimForward();

            originalDir *= pointerStartingForce;

            if (useUnityGlobalGravity)
            {
                pointerGravity = Physics.gravity;
            }

            if (enableDebugView)
            {
                //add original point to line positions
                aimPointerPoints.Add(aimPointer.transform.position);
            }

            if (pointerGravity == Vector3.zero)
            {

                //find the cloesest object within max range
                if (Physics.Raycast(aimPointer.transform.position, originalDir, out pointerHitObject, maxPointerDistance))
                {
                    if (testAimHit())
                    {
                        if (enableDebugView)
                        {
                            //add current position to line view
                            aimPointerPoints.Add(pointerHitObject.point);
                        }
                        return;
                    }
                }
                else
                {

                    if (enableDebugView)
                    {
                        aimPointerPoints.Add(aimPointer.transform.position + (originalDir * maxPointerDistance));
                    }

                }

            }
            else
            {

                Vector3 startPos = aimPointer.transform.position;
                Vector3 currentPos = startPos;

                //loop through finding next distance until distance is too far
                while (Vector3.Distance(startPos, currentPos) < maxPointerDistance)
                {

                    //0.005f is to guarantee a semi-accurate pointer calulcation
                    currentPos += originalDir * 0.016f;

                    //apply gravity
                    originalDir += pointerGravity * 0.016f;

                    if (enableDebugView)
                    {
                        //add current position to line view
                        aimPointerPoints.Add(currentPos);
                    }

                    //check if next position is blocked
                    if (Physics.Raycast(currentPos, originalDir, out pointerHitObject, (originalDir * 0.016f).magnitude))
                    {
                        if (testAimHit())
                        {
                            if (enableDebugView)
                            {
                                //add current position to line view
                                aimPointerPoints.Add(pointerHitObject.point);
                            }
                            return;
                        }
                    }
                }
            }
        }

        //check the object hit by raycast aim to this tank
        bool testAimHit()
        {
            if (pointerHitObject.transform.tag == "Tank")
            {
                Transform highestParent = pointerHitObject.transform;
                TurretRotation tempRot = null;

                while (highestParent.parent != null && tempRot == null)
                {
                    tempRot = highestParent.GetComponent<TurretRotation>();
                    highestParent = highestParent.parent;
                }

                if (tempRot == null)
                {
                    tempRot = highestParent.GetComponentInChildren<TurretRotation>();
                }

                if (tempRot != this)
                {
                    return true;
                }
            }
            else
            {
                if (enableDebugView)
                {
                    //add current position to line view
                    aimPointerPoints.Add(pointerHitObject.point);
                }
                return true;
            }
            return false;
        }

        void OnDrawGizmos()
        {
            //draw line to hit point
            for (int a = 0; a < aimPointerPoints.Count - 1; a++)
            {
                Gizmos.DrawLine(aimPointerPoints[a], aimPointerPoints[a + 1]);
            }

            //draw sphere at hit point
            if (aimPointerPoints.Count > 0)
            {
                if (pointerSolid)
                {
                    Gizmos.DrawSphere(aimPointerPoints[aimPointerPoints.Count - 1], 1);
                }
                else
                {
                    Gizmos.DrawWireSphere(aimPointerPoints[aimPointerPoints.Count - 1], 1);
                }
            }

            if (enableAimPointer)
            {
                if (enableDebugDirectionView)
                {
                    if (aimPointer)
                    {
                        //debug draw line of turret
                        Vector3 originalDir = Vector3.zero;

                        //find direction based on aimDir input
                        switch (aimDirection)
                        {
                            case turretPointerDirection.forward:
                                originalDir = aimPointer.transform.forward;
                                break;
                            case turretPointerDirection.back:
                                originalDir = -aimPointer.transform.forward;
                                break;
                            case turretPointerDirection.right:
                                originalDir = aimPointer.transform.right;
                                break;
                            case turretPointerDirection.left:
                                originalDir = -aimPointer.transform.right;
                                break;
                            case turretPointerDirection.up:
                                originalDir = aimPointer.transform.up;
                                break;
                            case turretPointerDirection.down:
                                originalDir = -aimPointer.transform.up;
                                break;
                        }

                        Gizmos.DrawLine(aimPointer.transform.position, aimPointer.transform.position + (originalDir * maxPointerDistance));
                    }
                }
            }
        }

        void Start()
        {
            //attempt to get the line renderer
            m_line = GetComponentInChildren<LineRenderer>();

            //get the controller
            if (controllerID < 0)
            {
                Debug.LogWarning("ERROR: Initiailising rewired player with an ID of " + controllerID + " is invalid");
            }
            else
            {
                rewiredPlayer = Rewired.ReInput.players.GetPlayer(controllerID);
            }
            //rewiredPlayer = null;



            //check if grab starting pos is enabled
            if (grabStartingRot || grabStartingRotReset)
            {
                //check if yaw gameobject exists
                if (yaw != null)
                {
                    //set tempRot to yaw rotation
                    Vector3 tempRot = yaw.transform.eulerAngles;

                    if (usingLocalYawRot)
                    {
                        tempRot = yaw.transform.localEulerAngles;
                    }

                    //force value to be within 0-360 due to unity rotations
                    if (tempRot.x < 0)
                    {
                        tempRot.x += 360;
                    }
                    if (tempRot.y < 0)
                    {
                        tempRot.y += 360;
                    }
                    if (tempRot.z < 0)
                    {
                        tempRot.z += 360;
                    }

                    if (grabStartingRot)
                    {
                        yawRot = tempRot;
                    }
                    if (grabStartingRotReset)
                    {
                        resetRotYaw = tempRot;
                    }
                }
                //check if pitch gameobject exists
                if (pitch != null)
                {
                    //set tempRot to pitch rotation
                    Vector3 tempRot = pitch.transform.eulerAngles;

                    if (usingLocalPitchRot)
                    {
                        tempRot = pitch.transform.localEulerAngles;
                    }

                    //force value to be within 0-360 due to unity rotations
                    if (tempRot.x < 0)
                    {
                        tempRot.x += 360;
                    }
                    if (tempRot.y < 0)
                    {
                        tempRot.y += 360;
                    }
                    if (tempRot.z < 0)
                    {
                        tempRot.z += 360;
                    }

                    if (grabStartingRot)
                    {
                        pitchRot = tempRot;
                    }
                    if (grabStartingRotReset)
                    {
                        resetRotPitch = tempRot;
                    }
                }
            }

        }

        void Update()
        {

            if (enableTurret)
            {
                controlTurret();
            }

            //draw line to hit point
            forceUpdateOnPointer();

            if (m_line != null)
            {
                m_line.positionCount = aimPointerPoints.Count - 1;
                for (int a = 0; a < aimPointerPoints.Count - 1; a++)
                {
                    m_line.SetPosition(a, aimPointerPoints[a]);
                }
            }
        }

        private void FixedUpdate()
        {
            //draw line to hit point
            /*forceUpdateOnPointer();

            if (m_line != null)
            {
                m_line.positionCount = aimPointerPoints.Count - 1;
                for (int a = 0; a < aimPointerPoints.Count - 1; a++)
                {
                    m_line.SetPosition(a, aimPointerPoints[a]);
                }
            }*/
        }

        //take bool and set turret to active/not
        public void toggleTurret(bool input)
        {
            enableTurret = input;
        }

        //grabs the gameobject of turret seat
        public GameObject getTurretSeat()
        {
            if (turretChair)
            {
                return turretChair;
            }
            else
            {
                return null;
            }
        }

        //grabs the gameobject that the turret is aiming at
        public GameObject getAimHit()
        {
            if (pointerHitObject.transform != null)
            {
                return pointerHitObject.transform.gameObject;
            }
            else
            {
                return null;
            }
        }

        //forces update on pointer so you can guarantee its
        //data is accurate and returns hit object
        public GameObject forceUpdateOnPointer()
        {
            currentRecheckPointer = recheckPointer;
            updateTurretPointer();
            return getAimHit();
        }

        //return the forward direction of aim pointer
        //with the users affect applied
        public Vector3 getActualAimForward()
        {

            //find direction based on aimDir input
            switch (aimDirection)
            {
                case turretPointerDirection.forward:
                    return aimPointer.transform.forward;

                case turretPointerDirection.back:
                    return -aimPointer.transform.forward;

                case turretPointerDirection.right:
                    return aimPointer.transform.right;

                case turretPointerDirection.left:
                    return -aimPointer.transform.right;

                case turretPointerDirection.up:
                    return aimPointer.transform.up;

                case turretPointerDirection.down:
                    return -aimPointer.transform.up;
            }

            return Vector3.zero;
        }

        //return the aim pointer of the tank
        public GameObject getAimPointer()
        {
            if (aimPointer != null)
            {
                return aimPointer;
            }
            else
            {
                return null;
            }
        }

        //set the yaw rotation limit
        public void setYawLimit(Vector2 input)
        {
            minMaxYaw = input;
        }

        //set the pitch rotation limit
        public void setPitchLimit(Vector2 input)
        {
            minMaxPitch = input;
        }

        //return the yaw rotation
        public Vector3 getYawRotation()
        {
            return yawRot;
        }

        //return the pitch rotation
        public Vector3 getPitchRotation()
        {
            return pitchRot;
        }

        //return the changeable yaw rotation
        public float getYawRotAngle()
        {
            switch (yawRotType)
            {
                case turretRotationAxis.x:
                    return yawRot.x;
                case turretRotationAxis.y:
                    return yawRot.y;
                case turretRotationAxis.z:
                    return yawRot.z;
            }
            return 0;
        }

        //return the changeable pitch rotation
        public float getPitchRotAngle()
        {
            switch (pitchRotType)
            {
                case turretRotationAxis.x:
                    return pitchRot.x;
                case turretRotationAxis.y:
                    return pitchRot.y;
                case turretRotationAxis.z:
                    return pitchRot.z;
            }
            return 0;
        }

		public Vector2 getRotationSpeed() {
			return new Vector2 (yawSpeed, pitchSpeed);
		}

		public void setRotationSpeed(Vector2 input) {
			yawSpeed = input.x;
			pitchSpeed = input.y;
		}

        public int GetPlayerID()
        {
            return controllerID;
        }

        public Player GetPlayer()
        {
            //make sure the player's been grabbed properly first, otherwise the script ordering will give everything a null
            if(rewiredPlayer == null)
            {
                rewiredPlayer = Rewired.ReInput.players.GetPlayer(controllerID);
            }

            return rewiredPlayer;
        }

    }

    public enum turretRotationAxis
    {
        none, x, y, z
    };

    public enum turretPointerDirection
    {
        forward, back, left, right, up, down
    };


}