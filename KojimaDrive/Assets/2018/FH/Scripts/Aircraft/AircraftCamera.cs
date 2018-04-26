using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO: Put a black boarder around the split screen camera.

//===================== Kojima Drive - FluffyHedgehog 2018 ====================//
//
// Author:		Dudley
// Purpose:		Follow any object which has a VehiclePhycics base class script.
// Namespace:	FH
//
//===============================================================================//


namespace FH
{
    public class AircraftCamera : MonoBehaviour
    {
        private LandVehicle vehicle;


        [Header("Aircraft Specific")]
        [SerializeField]
        private Transform target;
        [SerializeField]
        private AircraftParameters aircraftPhysics;
        [SerializeField]
        private float followDistance = 3f;
        [SerializeField]
        private float cameraElevation = 3f;
        [SerializeField]
        private float followTightness = 5f;
        [SerializeField]
        private float rotationTightness = 10f;
        [SerializeField]
        private float afterburnShakeAmount = 2f;
        [SerializeField]
        private float yawMultiplier = 0.005f;


        private void Start()
        {
            //aircraftPhysics = FindObjectOfType<AircraftPhysics>();
            //if(!aircraftPhysics)
            //{
            //    Debug.LogWarning("(Flight Controls) Flight controller is null on camera!");
            //}

            vehicle = FindObjectOfType<LandVehicle>();
            if (!vehicle)
            {
                Debug.LogWarning("vehicle is null on camera!");
            }

            //target = GameObject.FindGameObjectWithTag("Aircraft Camera Target").transform;
            //if(!target)
            //{
            //    Debug.LogError("(Flight Controls) Camera target is null!");
            //    return;
            //}
        }


        private void FixedUpdate()
        {
            // TODO: Add warning
            if(!target) return;

            var yRot = 0f;
            // HACK
            //Calculate where we want the camera to be.
             yRot = aircraftPhysics.Rudder * yawMultiplier;

             Vector3   newPosition = target.TransformPoint(yRot, cameraElevation, -followDistance);

            //Get the difference between the current location and the target's current location.
            Vector3 positionDifference = target.position - transform.position;

            //Move the camera towards the new position.
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * followTightness);

            Quaternion newRotation;
			transform.LookAt (target);
            // TODO: Camera shake
            //if (aircraftPhysics != null)
            //{
            //    if(aircraftPhysics.AfterBurnerActive)
            //    {
            //        newRotation = Quaternion.LookRotation(positionDifference + new Vector3(
            //            Random.Range(-afterburnShakeAmount, afterburnShakeAmount),
            //            Random.Range(-afterburnShakeAmount, afterburnShakeAmount),
            //            Random.Range(-afterburnShakeAmount, afterburnShakeAmount)),
            //            target.up);
            //    }
            //    else
            //    {
            //        // HACK
            //        newRotation = Quaternion.LookRotation(positionDifference, target.up);
            //    }
            //}
            //else
            //{
            //    // HACK
            //    newRotation = Quaternion.LookRotation(positionDifference, target.up);
            //}
            //transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * rotationTightness);
        }
    }

}
