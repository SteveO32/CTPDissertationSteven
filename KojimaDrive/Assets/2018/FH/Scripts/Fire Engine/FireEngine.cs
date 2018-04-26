using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: This class would be a fire engine manager ie log the posiitons which players are in, and each role.

// For this the fire engine does everything, later each person will have a different role based on which posiiton they're in.


namespace FH
{
    public class FireEngine : MonoBehaviour
    {
        Rewired.Player m_rewiredPlayer; 

        [SerializeField]
        private float driveSpeed = 30f;
        [SerializeField]
        private float turnSpeed = 10f;


        [SerializeField]
        private ControllerID controllerID = ControllerID.Unassigned;
        [SerializeField]
        private ObjectType objectType = ObjectType.Unassigned;
        [SerializeField]
        private new Rigidbody rigidbody;

        private bool slowing        = false;
        private bool accelerating   = false;
        private bool turning        = false;

        private float turnValue = 0f;
        private float gasValue = 0f;
        private float slowValue = 0f;


        private void Start()
        {
            rigidbody       = GetComponent<Rigidbody>();
            m_rewiredPlayer = Rewired.ReInput.players.GetPlayer(0);
        }



        private void OnEnable()
        {
           // InputManager.inputDetected += HandleInput;
            PlayerManager.playerCreated += InsertPlayer;
        }


        private void OnDisable()
        {
           // InputManager.inputDetected -= HandleInput;
            PlayerManager.playerCreated -= InsertPlayer;
        }


        private void Update()
        {
            // TODO: Tidy this code.
            if(m_rewiredPlayer.GetAxis("Gas") > 0)
            {
                accelerating = true;
                gasValue = m_rewiredPlayer.GetAxis("Gas");
            }

            if(m_rewiredPlayer.GetAxis("Slow") > 0)
            {
                slowing = true;
                slowValue = m_rewiredPlayer.GetAxis("Slow");
            }

            if(m_rewiredPlayer.GetAxis("Turn") != 0)
            {
                turning = true;
                turnValue = m_rewiredPlayer.GetAxis("Turn");
            }
        }


        private void FixedUpdate()
        {
            if(turning)      Turn(turnValue);
            if(slowing)      Reverse(slowValue);
            if(accelerating) Drive(gasValue);


            slowing      = false;
            turning      = false;
            accelerating = false;
        }

        ///private void HandleInput(GameAction gameAction, float value, ControllerID ID)
        ///{
        ///    if(controllerID != ID)
        ///        return;
        ///
        ///    switch(gameAction)
        ///    {
        ///        case GameAction.RT_Axis:
        ///            Drive(value);
        ///            break;
        ///        case GameAction.LT_Axis:
        ///            Reverse(value);
        ///            break;
        ///        case GameAction.LS_X_Axis:
        ///            Turn(value);
        ///            break;
        ///        case GameAction.A_Held:
        ///            break;
        ///        case GameAction.RS_X_Axis:
        ///            break;
        ///        case GameAction.RS_Y_Axis:
        ///            break;
        ///    }
        ///}


        private void InsertPlayer(ControllerID ID, ObjectType objectType)
        {
            if(objectType != this.objectType)
                return;
            controllerID = ID;
        }


        // TODO: Make this all booleans which activates the fixed Updates.
        private void Drive(float value)
        {
            var speed = driveSpeed * value;
            rigidbody.velocity += transform.forward * speed * Time.deltaTime;
        }

        private void Reverse(float value)
        {
            var speed = driveSpeed * value;
            rigidbody.velocity -= transform.forward * speed * Time.deltaTime;
        }

        private void Turn(float value)
        {

            var speed = value * turnSpeed * Time.deltaTime;
            transform.Rotate(0f, speed, 0f);
        }
    }
}