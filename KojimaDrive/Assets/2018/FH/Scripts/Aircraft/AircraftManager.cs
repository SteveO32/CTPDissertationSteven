using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//===================== Kojima Party - FluffyHedgehog 2018 ====================//
//
// Author:		Dudley 
// Purpose:		This handles the Input, Updates Components and knows who its controlled by. 
// Namespace:	FH
//
//===============================================================================//


namespace FH
{
    public class AircraftManager : MonoBehaviour
    {
        [SerializeField]
        private AircraftPhysics aircraftPhysics;
        [SerializeField]
        private AircraftBombHatch aircraftBombHatch;
        [SerializeField]
        private AircraftFuelTank aircraftFuelTank;
        [SerializeField]
        private AircraftHUD aircraftHUD;
        [SerializeField]
        private Camera aircraftCamera;
        [SerializeField]
        private ControllerID controllerID = ControllerID.Unassigned;
        [SerializeField]
        private ObjectType objectType = ObjectType.Unassigned;

        private Rewired.Player rewiredPlayer;
        // TODO: Add fuel to the aircraft.
        // TODO: Count down Fuel.. Depending on the current velocity  lower it faster.


        private void Awake()
        {
            if(controllerID == ControllerID.Unassigned)
            {
                Debug.LogWarning("ERROR: Cannot initialise the rewired player with a controller ID - " + controllerID.ToString());
            }
            else
            {
                rewiredPlayer = Rewired.ReInput.players.GetPlayer((int)controllerID);
            }


            aircraftPhysics = GetComponent<AircraftPhysics>();
            if(!aircraftPhysics)
            {
                Debug.Log("ERROR: AircraftPhysics script is not found");
                return;
            }

            aircraftBombHatch = GetComponent<AircraftBombHatch>();
            if(!aircraftBombHatch)
            {
                Debug.Log("ERROR: AircraftBombHatch script is not found");
                return;
            }

            aircraftFuelTank = GetComponent<AircraftFuelTank>();
            if(!aircraftFuelTank)
            {
                Debug.Log("ERROR: AircraftFuelTank script is not found");
                return;
            }

            aircraftHUD = GetComponent<AircraftHUD>();
            if(!aircraftHUD)
            {
                Debug.Log("ERROR: AircraftHUD script is not found");
                return;
            }

            aircraftCamera = FindObjectOfType<AircraftCamera>().GetComponent<Camera>();
            if(!aircraftCamera)
            {
                Debug.Log("ERROR: AircraftCamera script is not found");
                return;
            }
        }


        private void Start()
        {
            // TODO: Parse the tag into the Enum type and set the objectType as that.
        }



        private void OnEnable()
        {
            InputManager.inputDetected += HandleInput;
            PlayerManager.playerCreated += InsertPlayer;
            Runway.aircraftResupplied += Resupply;
        }


        private void OnDisable()
        {
            InputManager.inputDetected -= HandleInput;
            PlayerManager.playerCreated -= InsertPlayer;
            Runway.aircraftResupplied -= Resupply;
        }


        private void Update()
        {
            PollInput();

            //TODO add a cap on speed.
            aircraftFuelTank.UpdateMultiplier(aircraftPhysics.CurrentMagnitude);
            aircraftBombHatch.UpdateMultiplier(aircraftPhysics.CurrentMagnitude);


            if(aircraftBombHatch.Alert())
            {
                Debug.Log("Bomb stock low");
                aircraftHUD.BombStockOn();
            }
            else
            {
                aircraftHUD.BombStockOff();
            }

            if(aircraftFuelTank.Alert())
            {
                Debug.Log("Fuel stock low");
                aircraftHUD.FuelIndicatorOn();
            }
            else
            {
                aircraftHUD.FuelIndicatorOff();
            }


            if(aircraftFuelTank.FuelTankEmpty)
            {
                aircraftPhysics.EngineShutdown();
            }
            else
            {

                aircraftPhysics.EngineOnline();
            }
        }


        private void PollInput()
        {
            var accelerator = rewiredPlayer.GetAxis("Throttle");
            aircraftPhysics.ThrustData(accelerator);

           var slow = rewiredPlayer.GetAxis("Slow");
           if(slow < 0)
           {
               aircraftPhysics.ThrustData(slow);
           }



            var hor = rewiredPlayer.GetAxis("Roll");
            aircraftPhysics.RollData(hor);

            var vert = rewiredPlayer.GetAxis("Pitch");
            aircraftPhysics.PitchData(vert);



            var yawLeft = rewiredPlayer.GetButton("L-Yaw");
            if(yawLeft)
            {
                aircraftPhysics.YawData(-1);
            }

            var yawRight = rewiredPlayer.GetButton("R-Yaw");
            if(yawRight)
            {
                aircraftPhysics.YawData(1);
            }

            if(!yawLeft && !yawRight)
            {
                aircraftPhysics.YawData(0);
            }
            

            var toggleCam = rewiredPlayer.GetButtonDown("Switch Camera");
            var switchCam = 0f;

            if(toggleCam) switchCam = 1f;
                
            if(aircraftBombHatch.ToggleHatchCam(switchCam))
            {
                aircraftCamera.enabled = false;
            }
            else
            {
                aircraftCamera.enabled = true;
            }


            var dropBomb = rewiredPlayer.GetButton("Drop Bomb");
            if(dropBomb)
                aircraftBombHatch.DropBomb();



        }




        private void HandleInput(GameAction gameAction, float value, ControllerID ID)
        {
            if(controllerID != ID)
                return;

            switch(gameAction)
            {
                case GameAction.A_Down:
                    aircraftBombHatch.DropBomb();
                    break;
                case GameAction.B_Down:
                    if(aircraftBombHatch.ToggleHatchCam(value))
                    {
                        aircraftCamera.enabled = false;
                    }
                    else
                    {
                        aircraftCamera.enabled = true;
                    }
                    break;
                case GameAction.LS_X_Axis:
                    aircraftPhysics.RollData(value);
                    break;
                case GameAction.LS_Y_Axis:
                    aircraftPhysics.PitchData(-value);
                    break;
                case GameAction.RT_Axis:
                    aircraftPhysics.ThrustData(-value);
                    break;
                case GameAction.LB_Held:
                    aircraftPhysics.YawData(value);
                    break;
                case GameAction.RB_Held:
                    aircraftPhysics.YawData(value);
                    break;
            }
        }



        private void InsertPlayer(ControllerID ID, ObjectType objectType)
        {
            if(objectType != this.objectType)
                return;

            controllerID = ID;
        }


        private void Resupply()
        {
            aircraftBombHatch.Resupply();
            aircraftFuelTank.Resupply();
        }
    }
}
