using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//===================== Kojima Drive - FluffyHedgehog 2018 ====================//
//
// Author:		Dudley 
// Purpose:		Handle aircraft physics calculations. The values used for the calculations are passed in via
//              a manager or other class.
// Namespace:	FH
//
//===============================================================================//




namespace FH
{
    public class AircraftPhysics : MonoBehaviour
    {
        [Header("Default Values")]
        [SerializeField]
        private float defaultSpeed = 100f;
        [SerializeField]
        private float defaultAfterburnerSpeed = 350f;
        [SerializeField]
        private float defualtSlowSpeed = 50f;
        [SerializeField]
        private float defaultGravityScale = 15f;
        [SerializeField]
        private float defaultTurnSpeed = 50f;

        [Header("Current Values")]
        [SerializeField]
        private float currentSpeed = 100f; //"Base Speed", "Primary flight speed, without afterburners or brakes"
        [SerializeField]
        private float currentAfterburnerSpeed = 350f; //Afterburner Speed", "Speed when the button for positive thrust is being held down"
        [SerializeField]
        private float currentSlowSpeed = 50f;  //"Brake Speed", "Speed when the button for negative thrust is being held down"
        [SerializeField]
        private float currentTurnSpeed = 50f; //"Turn/Roll Speed", "How fast turns and rolls will be executed "
        [SerializeField]
        private float currentGravityScale = 15f;  //"Gravity", "A downwards force effecting the aircraft"

        [Header("Modifiers")]
        [SerializeField]
        private float rollSpeedModifier = 3f;  //"Roll Speed", "Multiplier for roll speed. Base roll is determined by turn speed"
        [SerializeField]
        private float pitchModifier = 2f;   //"Pitch Multiplier", "Controls the intensity of pitch and yaw inputs"
        [SerializeField]
        private float yawModifier = 2f;     //"Yaw Multiplier", "Controls the intensity of pitch and yaw inputs"
        [SerializeField]
        private float gravitationalModifier = 15f;  //"Gravitational Multiplier", "Controls the speed of the aircraft when lifting and dipping its nose"
        [SerializeField]
        private float bankRotationMultiplier = 1f;   //"Bank Rotation Multiplier", "Bank amount along the Z axis when yaw is applied."
        [SerializeField]
        private float heightLimitModifier = 0f;     //"Height Limit Multiplier", "USe this value to stop the plane when they reach the MAX_HEIGHT"
        
        [Header("Banking Visual Effect")]
        [SerializeField]
        private float bankAngleClamp = 360f; //"Bank Angle Clamp", "Maximum angle the spacecraft can rotate along the Z axis."
        [SerializeField]
        private float bankRotationSpeed = 3f;   //"Bank Rotation Speed", "Rotation speed along the Z axis when yaw is applied. Higher values will result in snappier banking."

        [Header("Misc")]
        [SerializeField]
        private float thrustTransitionSpeed = 5f;  //Thrust Transition Speed", "How quickly afterburners/brakes will reach their maximum effect"
        [SerializeField]
        private float maxHeight = 2000f;
        [SerializeField]
        private bool enginesOffline = false;

        [SerializeField]
        private bool inBoundary = true;





        private GameObject aircraftObject;
        private Rigidbody m_rb;

        private float thrustValue = 0f;
        private float pitchValue  = 0f;
        private float yawValue    = 0f;
        private float rollValue   = 0f;

        public bool  AfterBurnerActive { get; private set; }
        public float Yaw               { get; private set; }
        public float CurrentMagnitude  { get; private set; }



        private void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            if(!m_rb)
            {
                Debug.Log("ERROR: Rigidbody Component cannot be found");
                return;
            }

            aircraftObject = m_rb.gameObject;
            if(!aircraftObject)
            {
                Debug.LogError("(AircraftPhysics) Aircraft GameObject is null.");
                return;
            }

            AfterBurnerActive = false;
            CurrentMagnitude = 0f;

            EngineOnline();
        }


        private void FixedUpdate()
        {
            //if(inBoundary)
            //    Debug.Log("IN BOUNDARY!");
            //else
            //    Debug.Log("TURN ROUND!");

            UpdateForces();
        }


        /// <summary>
        /// Set thrust/ forward value.
        /// </summary>
        /// <param name="value"></param>
        public void ThrustData(float value)
        {
            if(enginesOffline)
                value = 0f;

            thrustValue = value;
        }

        /// <summary>
        /// Set Pitch data.
        /// </summary>
        /// <param name="value"></param>
        public void PitchData(float value)
        {
            pitchValue = value;
        }

        /// <summary>
        /// Set Yaw Data.
        /// </summary>
        /// <param name="value"></param>
        public void YawData(float value)
        {
            yawValue = value;
        }

        /// <summary>
        /// Set Roll Data
        /// </summary>
        /// <param name="value"></param>
        public void RollData(float value)
        {
            rollValue = value;
        }




        public void EngineShutdown()
        {
            enginesOffline = true;

            currentSpeed = 10f;
            currentSlowSpeed = 0f;
            currentAfterburnerSpeed = 0f;
            currentGravityScale = 30f;
            currentTurnSpeed = 25f;

        }

        /// <summary>
        /// Set all speed variables to defualt values. 
        /// </summary>
        public void EngineOnline()
        {
            enginesOffline = false;

            currentSpeed = defaultSpeed;
            currentSlowSpeed = defualtSlowSpeed;
            currentAfterburnerSpeed = defaultAfterburnerSpeed;
            currentGravityScale = defaultGravityScale;
            currentTurnSpeed = defaultTurnSpeed;
        }
 
        public void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Border")
            {
                inBoundary = true;
            }
        }

        public void OnTriggerExit(Collider col)
        {
            if(col.gameObject.tag == "Border")
            {
                inBoundary = false;
            }
        }
        /// <summary>
        /// Update the forces applied to the Aircraft. Use fixedDeltaTime.
        /// </summary>
        private void UpdateForces()
        {
            var roll = rollValue * -rollSpeedModifier;
            var pitch = pitchValue * pitchModifier;

            Yaw = yawValue * yawModifier;
            CurrentMagnitude = m_rb.velocity.magnitude;


            if(thrustValue > 0)
            {
                //If input on the thrust axis is positive, activate afterburners.
                AfterBurnerActive = true;
                CurrentMagnitude = Mathf.Lerp(CurrentMagnitude, currentAfterburnerSpeed, thrustTransitionSpeed * Time.fixedDeltaTime);
            }
            else if(thrustValue < 0)
            {
                //If input on the thrust axis is negatve, activate brakes.
                AfterBurnerActive = false;
                CurrentMagnitude = Mathf.Lerp(CurrentMagnitude, currentSlowSpeed, thrustTransitionSpeed * Time.fixedDeltaTime);
            }
            else
            {
                //Otherwise, hold normal speed.
                AfterBurnerActive = false;
                CurrentMagnitude = Mathf.Lerp(CurrentMagnitude, currentSpeed, thrustTransitionSpeed * Time.fixedDeltaTime);
            }

            m_rb.AddRelativeTorque(
                (pitch * currentTurnSpeed * Time.fixedDeltaTime),
                (Yaw * currentTurnSpeed * Time.fixedDeltaTime),
                (roll * currentTurnSpeed * (rollSpeedModifier / 2f) * Time.fixedDeltaTime));

            
            
            // HACK: stop plane from flying past a certain height.
            heightLimitModifier = (maxHeight - transform.position.y) / CurrentMagnitude;
            if(heightLimitModifier > 1f) heightLimitModifier = 1f;
            
            if(heightLimitModifier < 0f) gravitationalModifier = 50f;
            else gravitationalModifier = 15f; // Magic number / HACK.


            // Add speed based on gravity/ height
            CurrentMagnitude -= transform.forward.y * gravitationalModifier;
            m_rb.AddRelativeTorque(Vector3.right * currentGravityScale * Time.fixedDeltaTime);
            

            // Apply new magnitude
            m_rb.velocity = transform.forward * CurrentMagnitude;




           // Debug.Log("currentGravityScale - " + currentGravityScale);




           // UpdateBanking();
        }


        private void UpdateBanking()
        {
            //Load rotation information.
            Quaternion newRotation = transform.rotation;
            Vector3 newEulerAngles = newRotation.eulerAngles;

            //Basically, we're just making it bank a little in the direction that it's turning.
            newEulerAngles.z += Mathf.Clamp((-Yaw * currentTurnSpeed * Time.fixedDeltaTime) * bankRotationMultiplier, -bankAngleClamp, bankAngleClamp);
            newRotation.eulerAngles = newEulerAngles;

            //Apply the rotation to the gameobject that contains the model.
            aircraftObject.transform.rotation = Quaternion.Slerp(aircraftObject.transform.rotation, newRotation, bankRotationSpeed * Time.fixedDeltaTime);
        }



    }
}