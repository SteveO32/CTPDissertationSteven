using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TF
{
    public class Controller_FV : MonoBehaviour
    {

        public Rewired.Player player;
        public int playerId;


        public string movementHorizontalString = "Move Horizontal";

        public string movementVerticalString = "Move Vertical";


        public bool initialized = false;
        public float rotSpeed = 90;
        [Header("Input variables")]

        public Vector3 movementVector;
        public float speed = 1F;
        public Vector3 moveDirection = Vector3.zero;
        public CharacterController controller;
        private void Initialize()
        {

            player = Rewired.ReInput.players.GetPlayer(playerId);
            initialized = true;
            controller = GetComponent<CharacterController>();
        }
        void Update()
        {
            if (!initialized)
            {
                Initialize();
            }

            GetInput();
            ProcessInput();
        }

        void GetInput()
        {
            movementVector.x = player.GetAxis(movementHorizontalString);
            movementVector.z = player.GetAxis(movementVerticalString);
        }

        void ProcessInput()
        {

            Debug.Log(movementVector);
            moveDirection = new Vector3(movementVector.x, 0, movementVector.z);

            //moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            transform.Translate(moveDirection, Space.World);
            //controller.Move(moveDirection * Time.deltaTime);
           
        }
    }
}