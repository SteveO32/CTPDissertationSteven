using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Gualtiero Vercellotti 
// Purpose:		Input Manager for Human Physics Based characters. 
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public enum InputMode
    {
        CharacterRelative,
        CameraRelative,
        WorldRelative
    }

    [RequireComponent(typeof(HumanCharacterControl))]
    public class HumanCharacterInput : MonoBehaviour
    {
        public InputMode mode;

        HumanCharacterControl hcc;
        Rewired.Player player;

        [Header("Camera for 'Camera Relative' Input Mode")]

        [SerializeField]
        GameObject camera;

        [Header("Rewired Strings")]
        // --- Move forward --
        [SerializeField] string movementHorizontalString = "Move Horizontal";
        [SerializeField] string movementVerticalString = "Move Vertical";
        // --- Swing --
        [SerializeField] string swingString = "Swing";
        // --- Punch --
        [SerializeField] string punchLeftString = "PunchLeft";
        [SerializeField] string punchRightString = "PunchRight";
        [SerializeField] string Emote1String = "Emote1";
        [SerializeField] string Emote2String = "Emote2";
        [SerializeField] string SprintString = "Sprint";
        [SerializeField] string GetUpString = "GetUp";

        [System.NonSerialized]
        bool initialized = false;

        [Header("Input variables")]
        [SerializeField]
        Vector2 movementVector;

        [SerializeField]
        bool swing;

        [SerializeField]
        bool punchLeft;
        [SerializeField]
        bool punchRight;

        [SerializeField]
        bool emote1;
        [SerializeField]
        bool emote2;
        [SerializeField]
        bool sprint;
        [SerializeField]
        bool getUp;

        private void Initialize()
        {
            hcc = GetComponent<HumanCharacterControl>();
            player = Rewired.ReInput.players.GetPlayer(hcc.playerId);
            initialized = true;
        }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
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
            movementVector.y = player.GetAxis(movementVerticalString);

            swing = player.GetButtonDown(swingString);
            punchLeft = player.GetButtonDown(punchLeftString);
            punchRight = player.GetButtonDown(punchRightString);

            emote1 = player.GetButtonDown(Emote1String);
            emote2 = player.GetButtonDown(Emote2String);

            sprint = player.GetButton(SprintString);
            getUp = player.GetButtonTimeUnpressed(GetUpString) < 1.0f;
            getUp = getUp && player.GetButtonTimePressed(GetUpString) < 1.0f;
        }

        void ProcessInput()
        {
            switch (mode)
            {
                case InputMode.CharacterRelative:
                    hcc.Move(movementVector, mode);
                    break;
                case InputMode.WorldRelative:
                    hcc.Move(movementVector, mode);
                    break;
                case InputMode.CameraRelative:
                    hcc.Move(movementVector, mode, camera);
                    break;
            }
            
            if (punchRight) hcc.Punch(false);
            if (punchLeft) hcc.Punch(true);

            if (swing) hcc.Swing();

            if (emote1) hcc.Emote(1);
            if (emote2) hcc.Emote(2);

            if (sprint) hcc.speedMultiplier = 4.0f;

            else hcc.speedMultiplier = 2.0f;

            hcc.GetUp(getUp);
        }
    }
}
