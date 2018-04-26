using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kojima Party - Team Hairy Devs 2018
// Author: Piotr Lubinski
// Purpose: Rotating vehicle camera using a controller
// Namespace: HDev
// Script Created: 26/03/2018 11:00
// Last Edited by Piotr Lubinski  30/03/2018 15:00
// INSTRUCTIONS: 
                //To get this script working on your vehicles, all you need to do is assign a dummy (offset) object on top of your vehicle and then set its transform values
                // to whatever you want (e.g. increase the Y value for the camera to be above your player, and decrease Z for it to be beind the palyer)
                // add this script to each vehicle (could be placed on the offset object for easy tracking) and adjust values to suit your needs

public class HDev_CameraRotation : MonoBehaviour
{
    public Transform vehicleOffset;                 // assign your dummy gameObject (which is parented to your vehicle)
    public Transform vehicleCamera;                 // assing the main camera of your vehicle

    public Rewired.Player rewiredPlayer;            // assign player manually (if your game mode assigns players dynamically on start, feel free to edit this to fit your system)
    [SerializeField]
    private FH.ControllerID controllerID;

    private float rotateInputX = 0f;                // input from controller on X axis
    private float rotateInputY = 0f;                // input from controller on Y axis

    [Range(0,10)]
    public float turnSpeed      = 5.0f;             // speed at which you rotate the camera
    [Range(0, 10)]
    public float resetSpeed     = 1.0f;             // speed at which the camera rotates back to original position
    [Range(0, 10)]
    public float timeToReset    = 1.0f;             // time it takes before camera starts moving back to its original position

    private float resetTimer    = 0.0f;             // this needs to reach timeToReset to initialize reset funciton

    [Range(0, 180)]
    public float angleMax = 30f;                    // the maximum angle at which you can tilt your camera (up)
    [Range(-180, 0)]
    public float angleMin = -30f;                   // the minimum angle at which you can tilt your camera (down)

    float rotXAxis;                                 // temporary variable which holds info regarding the camera rotation around X axis


    public int ControllerID                         // takes the player/controller ID and assigns it to the camera
    {
        get
        {
            return (int)controllerID;
        }
        set
        {
            controllerID = (FH.ControllerID)value;
        }
    }

    void Start()
    {
        rewiredPlayer = Rewired.ReInput.players.GetPlayer((int)controllerID);

        if (vehicleOffset == null || vehicleCamera == null)                     // checks if you assigned target/camera
        {
            Debug.LogWarning("Missing target ref !", this);                     // and warns you if you didn't (yellow warning in console)

            return;
        }
    }

    void Update()
    {
        rotateInputX = rewiredPlayer.GetAxis("RotateCameraX");      // takes input from controller
        rotateInputY = rewiredPlayer.GetAxis("RotateCameraY");      // -----------''--------------

        if (rotateInputX == 0.0f && rotateInputY == 0.0f)           // if there is no 'rotate input' (player is not moving the right stick)
        {
            resetTimer += Time.deltaTime;                           // start the timer to check how long user is idling the camera controlls
            if (resetTimer > timeToReset)                           // and if the reset timer exeeced the idle wait time
            {
                vehicleCamera.rotation = Quaternion.RotateTowards(vehicleCamera.rotation, vehicleOffset.rotation, Time.time * resetSpeed); // return camera to original position
                //vehicleCamera.rotation = vehicleOffset.rotation;  // disable the line above and enable this if you want the camera to snap back to original position instantly
                vehicleCamera.position = vehicleOffset.position;    // this just ensures camera follows the car at right distance and height (bound to the offset)
            }
            if (vehicleCamera.rotation == vehicleOffset.rotation)   // if the camera position has been successfully reset, restart the timer
            {
                reset();
            }
        }
        else
        {
            vehicleCamera.transform.RotateAround(vehicleOffset.position, Vector3.up, rotateInputX * turnSpeed);         // rotates the camera left/right based on user input
            //vehicleCamera.transform.RotateAround(vehicleOffset.position, Vector3.forward, rotateInputY * turnSpeed);  // rotates the camera up/down with no limits

            rotXAxis = vehicleCamera.transform.eulerAngles.x;   // not gonna explain this entire section but basically it rotates the camera up/down with min/max limits
            rotXAxis += rotateInputY * turnSpeed;
            rotXAxis = ClampAngle(rotXAxis, angleMin, angleMax);
            Quaternion toRotation = Quaternion.Euler(rotXAxis, vehicleCamera.transform.eulerAngles.y, vehicleCamera.transform.eulerAngles.z);
            Quaternion rotation = toRotation;
            vehicleCamera.transform.rotation = rotation;

            vehicleCamera.position = vehicleOffset.position;     // ensure the camera is at the offsets position at all times
            vehicleCamera.LookAt(vehicleOffset);                 // and that it is looking at the vehicleOffset

            resetTimer = 0.0f;
        }
    }

    public static float ClampAngle(float rotXAxis, float angleMin, float angleMax) // this handles up/down rotation angles, need to convert it several times because... unity.
    {
        if (rotXAxis < 90 || rotXAxis > 270)
        {
            if (rotXAxis > 180) rotXAxis -= 360;
            if (angleMax > 180) angleMax -= 360;
            if (angleMin > 180) angleMin -= 360;
        }
        rotXAxis = Mathf.Clamp(rotXAxis, angleMin, angleMax);
        if (rotXAxis < 0) rotXAxis += 360;
        return rotXAxis;
    }

    private void reset()
    {
        resetTimer = 0.0f;
    }
}
