using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//===================== Kojima Drive - FluffyHedgehog 2018 ====================//
//
// Author:		TMS 
// Purpose:		[original] Script that controls how a car behaves. 
//              [update]   Trimmed version of the original CarScript. please think about decoupling if 
//                           you add anything. The last CarScript is horribley coupled.
// Namespace:	FH
//
// Last edit:   TMS @ 16/01/2017
//              FH  @ 23/02/2018 
//              HD  @ 25/03/2018
//===============================================================================//



namespace FH
{
    [System.Serializable]
    public struct VehicalInfo
    {
        public float CameraDistance { get; set; }

        //public VehicalInfo(float _camDistance)
        //{
        //    CameraDistance = _camDistance;
        //}
    }

    [System.Serializable]
    public class CarInfo
    {
        public enum driveMode_e { rearWheels, frontWheels, allWheels };
        public driveMode_e m_myDriveMode;

        [Range(0, 100)]
        public float m_health;

        [Range(4, 10)]
        public float m_acceleration;
        [Range(10, 150)]
        public float m_maxSpeed;

        [Range(0.25f, 6)]
        public float m_turnMaxSpeed;
        [Range(0, 0.5f)]
        public float m_extraGrip;

        [Range(0.35f, 0.5f)]
        public float m_wheelSize;

        [Range(7.5f, 10.0f)]
        public float m_cameraDistance;

        public CarSoundPack mySoundPack;

        public bool m_airControl;

        public CarInfo()
        {

        }

        public CarInfo(float _health, float _maxSpeed, float _acceleration, float _turnSpeed, float _wheelSize, float _grip, driveMode_e _driveMode, CarSoundPack _soundPack, float _cameraDistance)
        {
            m_myDriveMode = _driveMode;
            m_health = _health;

            m_acceleration = _acceleration;
            m_maxSpeed = _maxSpeed;

            m_turnMaxSpeed = _turnSpeed;
            m_wheelSize = _wheelSize;

            m_extraGrip = _grip;

            mySoundPack = _soundPack;

            m_airControl = true;
            m_cameraDistance = _cameraDistance;
        }
    }

    [System.Serializable]
    public struct WheelInfo
    {
        public bool m_grounded;
        public bool m_skidding;
        public float m_curSpeed;
    }


    public class LandVehicle : MonoBehaviour
    {
        public bool CanMove { get; set; }

        private List<Vector3>       wheelLocationPositions;
        private List<WheelInfo>     wheelInfos;
        private List<RaycastHit>    wheelRaycasts;

        private Bam.CarSuspensionScript susScript;
        private CapsuleCollider         carCollider;
        public Rewired.Player          rewiredPlayer;
        private Bam.CarSockets          sockets;
        private VehicalInfo             baseInfo;
        private Rigidbody               myRigidbody;
        private Vector3                 skidDirection;
        private Vector3                 driftVelo;

        private float forwardVelocity               = 0f;
        private float normalisedForwardVelocity     = 0f;
        private float wheelTorque                   = 0f;
        private float boostTimer                    = 0f;
        private float skiddingRight                 = 0f;
        private float targetAngularDrag             = 0.015f;
        private float currentSkidIntensity          = 0f;
        private float currentWheelSpin              = 0f;
        private float flipTimer                     = 0f;
        private float respawnTimer                  = 0f;
        private float currentRespawnCooldown        = 0f;
        private float respawnCounter                = 0f;
        private float prevAccelerationInput         = 0f;
        private float cancelHorizontalForce         = 0f;
        private float cancelHorizontalForceTarget   = 5f;

        // Controller Input values.
        private float acceleratorInput              = 0f;
        private float brakeInput                    = 0f;
        private float turnInput                     = 0f;
        private float tiltInput                     = 0f;

        private bool currentlyBraking       = false;
        private bool currentlyReversing     = false;
        private bool currentlyHandbraking   = false;
        private bool handBrake              = false;
        private bool respawnInput           = false;

        //TODO: Set input strings up here.


        [SerializeField]
        private List<Transform> wheels = new List<Transform>();
        [SerializeField]
		private ControllerID controllerID;
        [SerializeField]
        private bool useCarScriptPhysics;



		public int ControllerID 
        {
			get 
            {
				return (int)controllerID;
			}
			set
            {
                controllerID = (ControllerID)value;
			}
		}
	
 // "Controller ID", "This value needs to be manually set for now"

        [SerializeField]
        private CarInfo baseCarInfo;
        [SerializeField]
        private CarInfo surfaceStats;
        [SerializeField]
        private CarInfo boostStats;
        [SerializeField]
        private Transform carBody; // "Car Body", "Drag the BodyPivot child object into this variable"


        #region UnityFunctions

        private void Awake()
        {
            rewiredPlayer = Rewired.ReInput.players.GetPlayer((int)controllerID);


            //wheels                  = new List<Transform>(new Transform[4]); // TODO: Change from magic number to CONST var
            wheelLocationPositions  = new List<Vector3>(new Vector3[4]);
            wheelInfos              = new List<WheelInfo>(new WheelInfo[4]);
            wheelRaycasts           = new List<RaycastHit>(new RaycastHit[4]);

            //for (int i = 0; i < wheelInfos.Count; i++)
            //    wheelInfos[i].m_grounded = true;

            myRigidbody = GetComponent<Rigidbody>();
            carCollider = GetComponent<CapsuleCollider>();
            sockets     = GetComponent<Bam.CarSockets>();
            if(!sockets)
            {
                this.gameObject.AddComponent(typeof(Bam.CarSockets));
            }
            
            //Create suspension stuff
            susScript = gameObject.AddComponent<Bam.CarSuspensionScript>();
            susScript.Initialise(carBody, 1, this);
            
            for(int i = 0; i < wheelLocationPositions.Count; i++)
            {
                //Automatically makes all wheels equal in terms of Y pos
                wheels[i].transform.localPosition = new Vector3(
                    wheels[i].transform.localPosition.x,
                    wheels[0].transform.localPosition.y,
                    wheels[i].transform.localPosition.z);

                wheelLocationPositions[i] = wheels[i].transform.localPosition;
            }
        }


        private void Start()
        {
            ResetCar();
        }


        private void Update()
        {
           
           
            turnInput        = rewiredPlayer.GetAxis("Steer");
            acceleratorInput = rewiredPlayer.GetAxis("Gas");
            brakeInput       = -rewiredPlayer.GetAxis("Reverse");
            tiltInput        = rewiredPlayer.GetAxis("Tilt");
            
            // TODO: Add this to a different Map Category.
            respawnInput     = rewiredPlayer.GetButton("Respawn");

            HandleBoostStats();

            if(Input.GetKeyDown(KeyCode.R))
            {
                ResetCar();
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
               
            }

            if(respawnInput && CanMove)
            {
                if(respawnCounter < 1)
                {
                    respawnCounter += Time.deltaTime * 0.25f;
                }
                else
                {
                    respawnTimer = 1.2f;
                    respawnCounter = 0;
                    currentRespawnCooldown = 2;
                }
            }

            if(respawnTimer > 0)
            {
                respawnTimer -= Time.deltaTime;

                if(respawnTimer <= 0)
                {
                    ResetCar();
                }
            }
			//Debug.Log (CanMove);
        }



        private void FixedUpdate()
        {
            currentlyBraking = false;
            currentlyReversing = false;

            //check for currently reversing
            if(normalisedForwardVelocity < 0 && acceleratorInput < 0)
            {
                currentlyReversing = true;
            }

            //Handles wheel speed
            for(int i = 0; i < 4; i++)
            {
                if(!WheelGrounded(i) || IsWheelSkidding(i))
                {
                    AddSpeedToWheel(i, acceleratorInput * MaxSpeed());
                }
                else
                {
                    var wheelInfo = wheelInfos[i];
                    wheelInfo.m_curSpeed = forwardVelocity;
                    wheelInfo.m_grounded = true;
                    wheelInfos[i] = wheelInfo;
                }
            }

            ManageDrag();
            Movement();

            currentWheelSpin = Mathf.InverseLerp(currentWheelSpin, 0, 1 * Time.deltaTime);

            if (useCarScriptPhysics)
            {
                if (InAir)
                {
                    currentSkidIntensity = 0;

                    if (baseCarInfo.m_airControl && Mathf.Abs(myRigidbody.velocity.y) > 1)
                    {
                        AirControl();
                    }
                }
            }
            else
            {
                if(Grounded)
                {
                    currentSkidIntensity = 0;

                    if (baseCarInfo.m_airControl && Mathf.Abs(myRigidbody.velocity.y) > 1)
                    {
                        AirControl();
                    }
                }
            }

            MaintainMaxSpeed();

            //Apply some manual effects to suspension
            susScript.ApplySteerForce(transform.InverseTransformDirection(GetVelocity()).x * -8);

            prevAccelerationInput = acceleratorInput;

        }

        private void LateUpdate()
        {
            for(int i = 0; i < wheels.Count; i++)
            {
                if(i > 1)
                {
                    float lerpValue = 0.5f + turnInput * 0.5f;

                    float maxWheelAngle = 25;
                    float newY = Mathf.Lerp(-maxWheelAngle, maxWheelAngle, lerpValue);

                    if(wheels[i].transform.localPosition.x < 0)
                    {
                        newY += 180;
                    }

                    wheels[i].transform.localRotation = 
                        Quaternion.Lerp(wheels[i].transform.localRotation,
                        Quaternion.Euler(new Vector3(wheels[i].localEulerAngles.x, newY, 0)),
                        8 * Time.deltaTime);
                }
                RotateWheel(i);
            }
            SuspensionEffects();
        }
        
        #endregion


        #region PublicFunctions

        public Transform GetSocket(Bam.CarSockets.Sockets whichSocket)
        {
            return sockets.GetSocket(whichSocket);
        }
        

        public bool IsWheelSkidding(int index)
        {
            return wheelInfos[index].m_skidding;
        }

        // TODO: Change 0.075f into a private const.
        public bool Moving()
        {
            return Mathf.Abs(normalisedForwardVelocity) > 0.075f;
        }

        public bool UpsideDown()
        {
            return flipTimer > 0f;
        }

        public bool IsCurrentlyBraking()
        {
            return currentlyBraking;
        }

        public bool IsCurrentlyReversing()
        {
            return currentlyReversing;
        }

        public bool WheelGrounded(int index)
        {
            return wheelInfos[index].m_grounded;
        }

        public List<RaycastHit> GetWheelRaycasts()
        {
            return wheelRaycasts;
        }

        public CarInfo GetCarInfo()
        {
            return baseCarInfo;
        }

        public void ApplyNewSurfaceStats(ref CarInfo newSurfaceStats)
        {
            surfaceStats = newSurfaceStats;
        }

        ///<summary>
        ///Causes the car to smoothly boost forwards (for drift boosting or slipstreams etc)
        ///</summary>
        public void AccelerationBoost(float intensity = 1, float time = 1.5f)
        {
            boostStats.m_acceleration += intensity;
            boostTimer = time;
        }

        ///<summary>
        ///Causes the car to smoothly boost forwards (for drift boosting or slipstreams etc)
        ///</summary>
        public void SmallBoost(float intensity = 1, float time = 1.5f)
        {
            boostStats.m_acceleration += intensity;
            boostStats.m_maxSpeed += intensity;

            boostTimer = time;
        }

        ///<summary>
        ///Causes the car to boost forwards with fire coming out of the back
        ///</summary>
        public void NitroBoost(float intensity = 1, float time = 3.5f)
        {
            float nitroIntensity = 3;

            boostStats.m_acceleration += intensity * nitroIntensity;
            boostStats.m_maxSpeed += intensity * nitroIntensity;

            boostTimer = time;
        }


        /// <summary>
        /// Reset the Car to the nearest floor position. Reset the rigidbody.
        /// </summary>
        public void ResetCar()
        {
            RaycastHit floor;
            respawnTimer = 0;

            if(Physics.Raycast(transform.position, -Vector3.up, out floor))
            {
                if(floor.distance < 25f)
                {
                    // Debug.DrawLine(transform.position, floor.point);
                    transform.position = floor.point;

                }
            }

            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = Vector3.zero;

            for(int i = 0; i < wheels.Count; i++)
            {
                var wheelInfo = wheelInfos[i];

                wheelInfo.m_curSpeed = 0;
                wheelInfo.m_skidding = false;

                wheelInfos[i] = wheelInfo;
            }

            CanMove = true;
        }
        
        public GameObject GetCarBody()
        {
            return carBody.gameObject;
        }

        public void RemoveFromScene()
        {
            Destroy(gameObject);
        }


        public void ApplyCarInfo(CarInfo newInfo)
        {
            baseCarInfo = newInfo;
        }

        public Vector3 GetVelocity()
        {
            return myRigidbody.velocity;
        }
        

        public float GetSkidIntensity()
        {
            return currentSkidIntensity;
        }

        /// <summary>
        /// TODO: Move put this in a new script. 
        /// </summary>
        public void Explode()
        {
            respawnTimer = 5;

            var explosion = Instantiate(ParticleBank.singleton.explosion1);
            explosion.transform.SetParent(transform);
            explosion.transform.localPosition = Vector3.zero;
            explosion.transform.localScale = Vector3.one;
        }

        #endregion


        #region PrivateFunctions

        /// <summary>
        /// Returns true if all the wheels are grounded
        /// </summary>
        private bool Grounded
        {
            get
            {
                int groundedWheels = 0;
                for(int i = 0; i < wheelInfos.Count; i++)
                {
                    if(wheelInfos[i].m_grounded)
                    {
                        groundedWheels++;
                    }
                }
                return groundedWheels == wheelInfos.Count;
            }
        }

        /// <summary>
        /// Returns true if the car has not wheels grounded.
        /// </summary>
        private bool InAir
        {
            get
            {
                int groundedWheels = 0;
                for(int i = 0; i < wheelInfos.Count; i++)
                {
                    if(wheelInfos[i].m_grounded)
                    {
                        groundedWheels++;
                    }
                }
                return groundedWheels == 0;
            }

        }

        private void SuspensionEffects()
        {
            Vector3 veloLocal = transform.InverseTransformDirection(myRigidbody.velocity);
            Vector3 veloEuler = veloLocal;

            veloEuler.x = veloLocal.z * 0.015f;
            veloEuler.z = veloLocal.x * 0.4f;
            veloEuler.y = veloLocal.x * 0.5f;

            veloEuler = Vector3.ClampMagnitude(veloEuler, 2);
        }



        private void AddWheelSpin(float amount = 0.25f)
        {
            currentWheelSpin += amount;

            if(currentWheelSpin > 1)
            {
                currentWheelSpin = 1;
            }
        }


        private void AddSpeedToWheel(int wheelIndex, float speed)
        {
            var wheelInfo = wheelInfos[wheelIndex];

            wheelInfo.m_curSpeed += speed * Time.deltaTime;
            wheelInfo.m_curSpeed = Mathf.Clamp(wheelInfos[wheelIndex].m_curSpeed, -MaxSpeed() * 0.65f, MaxSpeed());

            wheelInfos[wheelIndex] = wheelInfo;
        }


        private void ManageDrag()
        {
            float targetDrag = 0.0001f;
            myRigidbody.drag = 0.0f;

            for(int i = 0; i < wheels.Count; i++)
            {
                if(wheelInfos[i].m_grounded)
                {
                    myRigidbody.drag += targetDrag * 0.25f * WheelGrip();
                }
            }
        }


        /// <summary>
        /// Handle movement in the air.
        /// </summary>
        private void AirControl()
        {
            Vector3 controlVelocity = new Vector3(rewiredPlayer.GetAxisRaw("Tilt"), rewiredPlayer.GetAxisRaw("Steer"), 0);
            controlVelocity = transform.TransformDirection(controlVelocity);
            myRigidbody.AddTorque(controlVelocity * 3, ForceMode.Acceleration);
        }



        public void ApplyHandbrake()
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * skiddingRight);
            myRigidbody.AddForce(transform.right * skiddingRight * 15 * normalisedForwardVelocity * myRigidbody.angularVelocity.y, ForceMode.Acceleration);

            //Don't let the car just drive and handbrake
            if(myRigidbody.angularVelocity.magnitude < 2)
            {
                myRigidbody.velocity = Vector3.Lerp(myRigidbody.velocity, Vector3.zero, 1 * Time.deltaTime);
            }

        }

        private void HandleBoostStats()
        {
            float resetSpeed = 1;

            if(boostTimer <= 0)
            {
                boostStats.m_acceleration = Mathf.Lerp(boostStats.m_acceleration, 0, resetSpeed * Time.deltaTime);
                boostStats.m_maxSpeed = Mathf.Lerp(boostStats.m_maxSpeed, 0, resetSpeed * Time.deltaTime);
            }
            else
            {
                boostTimer -= Time.deltaTime;
            }
        }


        private void MakeWheelSkid(int wheelIndex)
        {
            float skidTransitionSpeed = 6.0f;

            var wheelInfo = wheelInfos[wheelIndex];
            wheelInfo.m_skidding = true;
            wheelInfos[wheelIndex] = wheelInfo;


            myRigidbody.drag += 0.07f;
            myRigidbody.angularDrag = 0.0f;
            cancelHorizontalForce = Mathf.Lerp(cancelHorizontalForce, 0, skidTransitionSpeed * Time.deltaTime);

            currentSkidIntensity = Mathf.Abs(GetVelocity().magnitude);
        }

        private void Movement()
        {
            float currentVelocity = myRigidbody.velocity.magnitude;
            forwardVelocity = (currentVelocity * Vector3.Dot(myRigidbody.velocity.normalized, transform.forward));
            normalisedForwardVelocity = forwardVelocity / MaxSpeed();

            myRigidbody.angularDrag = 0.001f;
            if(CanMove)
            {
                bool driftBtnPress = rewiredPlayer.GetButton("Drift");
                if (useCarScriptPhysics)
                {
                    //handBrake = driftBtnPress && !Grounded;
                    handBrake = driftBtnPress && !InAir;
                }
                else
                {
                    handBrake = driftBtnPress && !Grounded;
                }


                //Give each wheel a chance to push the car if grounded
                for (int i = 0; i < wheels.Count; i++)
                {
                    var wheelInfo = wheelInfos[i];
                    wheelInfo.m_grounded = PerformWheelRaycast(wheels[i], i);

                    wheelInfos[i] = wheelInfo;

                    bool thisWheelCanDrive = false;
                    switch(baseCarInfo.m_myDriveMode)
                    {
                        case CarInfo.driveMode_e.allWheels:
                            thisWheelCanDrive = true;
                            break;
                        case CarInfo.driveMode_e.rearWheels:
                            if(i <= 1)
                            {
                                thisWheelCanDrive = true;
                            }
                            break;
                        case CarInfo.driveMode_e.frontWheels:
                            if(i > 1)
                            {
                                thisWheelCanDrive = true;
                            }
                            break;

                    }

                    if(!WheelGrounded(i))
                    {
                        thisWheelCanDrive = false;
                        handBrake = false;
                        
                    }

                    if(WheelGrounded(i))
                    {
                        PreventSkidding(i);
                    }
                    else
                    {
                        //Adds some angular drag to prevent really quick spinning in mid air
                        myRigidbody.angularDrag += 0.2f;
                    }


                    //Steering
                    float steerAmount = 0;
                    float steerControlMultiplier = 1;

                    ////Rotating while handbraking
                    if(currentlyHandbraking)
                    {
                        steerControlMultiplier = 0.975f;
                    }

                    Steering(rewiredPlayer.GetAxisRaw("Steer") * steerControlMultiplier, i, out steerAmount);

                    //Rotates the car to fit the terrain
                    if(WheelGrounded(i) && wheelRaycasts[0].normal.y > 0.4f)
                    {
                        myRigidbody.rotation = Quaternion.RotateTowards(
                            myRigidbody.rotation,
                            Quaternion.LookRotation(GetWheelDirection(i, 1), wheelRaycasts[i].normal),
                            3 * Time.deltaTime);
                    }

                    if(thisWheelCanDrive)
                    {
                        float forwardMultiplier = acceleratorInput + brakeInput;

                        Vector3 wheelDirection = GetWheelDirection(i, forwardMultiplier);
                        Debug.DrawLine(wheels[i].transform.position, wheels[i].transform.position + wheelDirection * 5, Color.green);

                        Vector3 accelerationForce = wheelDirection * Acceleration() * acceleratorInput;
                        Vector3 brakeForce = wheelDirection * Acceleration() * Mathf.Abs(brakeInput);

                        //Allow cars a bit more acceleration when going uphill just incase
                        accelerationForce *= 1 + Mathf.Clamp(Mathf.Abs(wheelDirection.y * 20), 0, 1);

                        //This makes it easier to balance four wheel drive with two wheel drive cars
                        if(baseCarInfo.m_myDriveMode == CarInfo.driveMode_e.allWheels)
                        {
                            accelerationForce *= 0.5f;
                            brakeForce *= 0.5f;
                        }

                        float wheelSpinMultiplier = Mathf.Clamp(Mathf.Abs(1 - currentWheelSpin), 0.1f, 1);

                        if(wheelSpinMultiplier < 0.4f)
                        {
                            MakeWheelSkid(i);
                        }

                        if(!currentlyHandbraking)
                        {
                            float extraStartupSpd = 1 - normalisedForwardVelocity;
                            accelerationForce += accelerationForce.normalized * extraStartupSpd;

                            myRigidbody.AddForce(accelerationForce * wheelSpinMultiplier, ForceMode.Acceleration);
                            myRigidbody.AddForce(brakeForce * wheelSpinMultiplier, ForceMode.Acceleration);
                        }

                        //Brakes rather than reverse
                        if(Mathf.Abs(brakeForce.magnitude) > 0.1f && normalisedForwardVelocity > 0.1f)
                        {
                            currentlyBraking = true;
                            MakeWheelSkid(i);
                        }
                        else if(currentlyBraking)
                        {
                            currentlyBraking = false;
                        }

                        //Slows down the car when turning
                        if(!currentlyHandbraking)
                        {
                            float turnDragScalar = 1.2f;

                            if(baseCarInfo.m_myDriveMode == CarInfo.driveMode_e.frontWheels)
                            {
                                if(!IsWheelSkidding(i))
                                {
                                    turnDragScalar = 0.25f;
                                }
                                else
                                {
                                    turnDragScalar = 0;
                                }
                            }
                            myRigidbody.velocity = Vector3.Lerp(myRigidbody.velocity, myRigidbody.velocity * 0.5f, (steerAmount * turnDragScalar) * 0.001f);
                        }
                    }

                    //Stabiliser forces
                    //This bit helps if you're stuck on a bridge with half of your wheels on and half off
                    if(wheels[i].transform.localPosition.x > 0)
                    {
                        myRigidbody.AddForce(-transform.right * 8, ForceMode.Acceleration);
                    }
                    else
                    {
                        myRigidbody.AddForce(transform.right * 8, ForceMode.Acceleration);
                    }
                }

                if (currentlyHandbraking)
                {
                    if (driftBtnPress)
                    {
                        skiddingRight = Mathf.Clamp(transform.InverseTransformDirection(myRigidbody.velocity).x, -2, 2);
                    }
                    ApplyHandbrake();
                }
            }
        }


        private Vector3 GetWheelDirection(int wheelIndex, float forwardMultiplier)
        {
            Vector3 wheelForward = transform.forward * forwardMultiplier;
            Vector3 direction = Vector3.Cross(wheelRaycasts[wheelIndex].normal, wheelForward);
            direction = Vector3.Cross(direction, wheelRaycasts[wheelIndex].normal);

            CheckForWheelSteepness(ref direction, wheelIndex);

            return direction;
        }


        private Vector3 CheckForWheelSteepness(ref Vector3 direction, int wheelIndex)
        {
            float steepLimit = 0.4f;


            if(direction.y > steepLimit)
            {
                direction.y = -steepLimit;
                MakeWheelSkid(wheelIndex);
            }
            return direction;
        }

        private float Acceleration()
        {
            return Mathf.Clamp((baseCarInfo.m_acceleration + boostStats.m_acceleration + surfaceStats.m_acceleration), 6, 100);
        }

        private float MaxSpeed()
        {
            return Mathf.Clamp((baseCarInfo.m_maxSpeed + boostStats.m_maxSpeed + surfaceStats.m_maxSpeed), 2, 100);
        }

        private float WheelGrip()
        {
            //It's 0.95f here so that cars don't have completely perfect grip by default
            return 0.95f + (baseCarInfo.m_extraGrip + boostStats.m_extraGrip + baseCarInfo.m_extraGrip + surfaceStats.m_extraGrip);
        }

        private void Steering(float dir, int index, out float steerAmount)
        {
            float steerAccelerationSpeed = 6.1915f;
            float steerAccMax = 6.5f;

            float curTorqueSpeed = 0;
            steerAmount = 0;

            //Steering
            if(index > 1)
            {
                float responsiveness = 25;
                responsiveness = 1;

                if(wheelInfos[index].m_skidding)
                    responsiveness *= 930;

                curTorqueSpeed = 0;

                curTorqueSpeed = (Mathf.Abs(wheelInfos[index].m_curSpeed)) * responsiveness;
                curTorqueSpeed = Mathf.Clamp(curTorqueSpeed, -steerAccelerationSpeed, steerAccelerationSpeed);

                if(currentlyReversing)
                {
                    dir = -dir;
                }

                //Slower turning at high speeds
                if(!currentlyHandbraking)
                {
                    float slowTurningAtHighSpeedsScalar = 1;
                    float steerSpeedMinimumNormalised = 0.95f;
                    float steerSpeedMaxReduction = 2.15f;

                    if(baseCarInfo.m_myDriveMode == CarInfo.driveMode_e.frontWheels && normalisedForwardVelocity > 0)
                    {
                        steerSpeedMinimumNormalised = 0.55f;
                        slowTurningAtHighSpeedsScalar = 1.125f;
                    }

                    slowTurningAtHighSpeedsScalar -= Mathf.Abs(normalisedForwardVelocity) * (1 - steerSpeedMinimumNormalised);
                    slowTurningAtHighSpeedsScalar = Mathf.Clamp(slowTurningAtHighSpeedsScalar, steerSpeedMinimumNormalised, 1);

                    float reduction = steerSpeedMaxReduction * (Mathf.Abs(normalisedForwardVelocity) * (1 - steerSpeedMinimumNormalised));
                    curTorqueSpeed -= reduction;
                    curTorqueSpeed = Mathf.Clamp(curTorqueSpeed, 0, steerAccMax);
                }

                if(WheelGrounded(index))
                {
                    if(currentlyHandbraking)
                    {
                        curTorqueSpeed *= 0.5f;
                    }

                    //stringForGizmoDebug = "Torque Speed = " + curTorqueSpeed;
                    Vector3 torque = transform.up * dir * (curTorqueSpeed);

                    myRigidbody.AddTorque(torque, ForceMode.Acceleration);
                    steerAmount = torque.magnitude;
                }
            }
        }


        private void MaintainMaxSpeed()
        {
            float maxSpeed = MaxSpeed();

            if(currentlyReversing)
            {
                maxSpeed *= 0.45f;
            }

            //if(myRigidbody.velocity.magnitude > maxSpeed && !Grounded) //original version
            if (useCarScriptPhysics)
            {
                if (myRigidbody.velocity.magnitude > maxSpeed && Grounded)
                {
                    myRigidbody.velocity = myRigidbody.velocity.normalized * maxSpeed;
                }
            }
            else
            {
                if (myRigidbody.velocity.magnitude > maxSpeed && !Grounded)
                {
                    myRigidbody.velocity = myRigidbody.velocity.normalized * maxSpeed;
                }
            }

            float actualMaxTurnSpeed = (baseCarInfo.m_turnMaxSpeed * (Mathf.Abs(normalisedForwardVelocity) + 0.25f));

            //if(myRigidbody.angularVelocity.magnitude > actualMaxTurnSpeed && !Grounded) //original version
            if (useCarScriptPhysics)
            {
                if (myRigidbody.angularVelocity.magnitude > actualMaxTurnSpeed && Grounded)
                {
                    myRigidbody.angularVelocity = myRigidbody.angularVelocity.normalized * actualMaxTurnSpeed;
                }
            }
            else
            {
                if (myRigidbody.angularVelocity.magnitude > actualMaxTurnSpeed && !Grounded)
                {
                    myRigidbody.angularVelocity = myRigidbody.angularVelocity.normalized * actualMaxTurnSpeed;
                }
            }
        }


        private IEnumerator InitialAccelerationBounce()
        {
            float timer = 0;

            while(timer < 0.1f)
            {
                timer += Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
        }

        private IEnumerator RollBackOver()
        {
            //This timer is here just in case this loop never ends naturally
            float timer = 5;


            while(transform.up.y < 0.995f && timer > 0)
            {
                timer -= Time.deltaTime;
                myRigidbody.AddForce(Vector3.up * 9, ForceMode.Acceleration);
                myRigidbody.rotation = Quaternion.Lerp(myRigidbody.rotation, Quaternion.LookRotation(transform.forward, Vector3.up), 5 * Time.deltaTime);
                yield return new WaitForSeconds(0.01f);
            }

            myRigidbody.useGravity = true;
            flipTimer = 0;
        }


        private void RotateWheel(int index)
        {
            Transform wheel = wheels[index];
            float sideMultiplier = 1;

            if(wheel.localPosition.x > 0)
            {
                sideMultiplier *= -1;
            }

            wheel.Rotate(Vector3.right * wheelInfos[index].m_curSpeed * -1.5f * sideMultiplier);
        }



        private void PreventSkidding(int wheelIndex)
        {
            float skidTransitionSpeed = 2;

            if(IsWheelSkidding(wheelIndex) || currentlyHandbraking)
            {
                MakeWheelSkid(wheelIndex);
            }
            else
            {
                cancelHorizontalForce = Mathf.Lerp(cancelHorizontalForce, cancelHorizontalForceTarget, skidTransitionSpeed * Time.deltaTime);
            }

            Vector3 velo = myRigidbody.velocity;
            Vector3 localVelo = transform.InverseTransformDirection(velo);

            // Basecarinfo changed from currentStats.
            float skidThreshold = 8.95f + baseCarInfo.m_extraGrip;

            //Makes it easier to skid at high speeds and hard to skid at slow speeds
            float lowSpeedExtraThreshold = 18.025f;
            skidThreshold += lowSpeedExtraThreshold - Mathf.Abs((velo.magnitude / MaxSpeed()) * lowSpeedExtraThreshold);


            float rotationSkidMultiplier = 0.15f;
            float rotationSkidThreshold = (skidThreshold * rotationSkidMultiplier) + (WheelGrip() * rotationSkidMultiplier);

            //Debug.Log("ST: " + skidThreshold + " / Cur Spd: " + (localVelo));
            //Debug.Log("RST: " + rotationSkidThreshold + " / Cur Spd: " + Mathf.Abs(m_rb.angularVelocity.y));

            if(baseCarInfo.m_myDriveMode == CarInfo.driveMode_e.frontWheels)
            {
                skidThreshold *= 0.45f;
            }

            if(IsWheelSkidding(wheelIndex))
            {
                rotationSkidThreshold *= 0.25f;
            }

            if((Mathf.Abs(localVelo.x) > skidThreshold || Mathf.Abs(myRigidbody.angularVelocity.y) > rotationSkidThreshold || (currentlyHandbraking && wheelIndex < 2)))
            {
                MakeWheelSkid(wheelIndex);
            }
            else
            {
                MakeWheelStopSkidding(wheelIndex);
                //m_currentlySkidding = false;
                currentSkidIntensity = Mathf.Lerp(currentSkidIntensity, 0, 2.5f * Time.deltaTime);
            }

            //Applies strong horiontal friction unless a grapple is pulling me
            if((wheelIndex > 1))
            {
                wheelTorque = localVelo.z;
                localVelo.x = Mathf.Lerp(localVelo.x, 0, cancelHorizontalForce * (WheelGrip()) * Time.deltaTime * transform.up.y);
                myRigidbody.velocity = transform.TransformDirection(localVelo);
            }
        }

        private void MakeWheelStopSkidding(int index)
        {
            var wheelInfo = wheelInfos[index];
            wheelInfo.m_skidding = false;
            wheelInfos[index] = wheelInfo;
        }


        private bool PerformWheelRaycast(Transform wheel, int index)
        {
            var previouslyGrounded = WheelGrounded(index);
            var wheelRaycast = wheelRaycasts[index];


            var result = Physics.SphereCast(
                wheel.transform.position,
                baseCarInfo.m_wheelSize * 0.15f,
                -transform.up,
                out wheelRaycast,
                baseCarInfo.m_wheelSize * 2,
                LayerMask.GetMask("Default", "Land"),
                QueryTriggerInteraction.Ignore);

            wheelRaycasts[index] = wheelRaycast;

            ///if(ret)
            ///{
            ///    Debug.DrawLine(wheel.transform.position, wheelRaycasts[index].point, Color.red, 0.01f);
            ///}
            ///else
            ///{
            ///    Debug.DrawLine(wheel.transform.position, wheel.transform.position - transform.up * baseCarInfo.m_wheelSize, Color.blue, 0.02f);
            ///}

            if(result && !previouslyGrounded)
            {
                WheelHasLanded(index);
            }

            return result;
        }



        private void WheelHasLanded(int index)
        {
            StopCoroutine("RollBackOver");
        }
    }

    #endregion
}