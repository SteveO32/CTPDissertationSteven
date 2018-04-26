using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH
{
    public enum ObjectType
    {
        Aircraft,
        Driver,
        Cannon_One,
        Cannon_Two,
        Unassigned
    }

    public class PlayerData
    {
        public ObjectType PlayerType { get; set; }
        public ControllerID ControllerID { get; set; }


        public PlayerData(ControllerID controllerID)
        {
            ControllerID = controllerID;
            PlayerType = ObjectType.Unassigned;
        }
    }


    public class PlayerManager : MonoBehaviour
    {
        public delegate void PlayerCreated(ControllerID ID, ObjectType playerType);
        public static event PlayerCreated playerCreated;

        public delegate void PlayerReconnected(ControllerID ID);
        public static event PlayerReconnected playerReconnected;

        public delegate void PlayerDisconnected(ControllerID ID);
        public static event PlayerDisconnected playerDisconnected;

        [SerializeField]
        private InputManager inputManager;
        [SerializeField]
        private List<PlayerData> players = new List<PlayerData>();
        //[SerializeField]
        //private List<ObjectType> objectTypes = new List<ObjectType>(new ObjectType[] { ObjectType.Aircraft, ObjectType.Driver, ObjectType.Cannon_One, ObjectType.Cannon_Two });
        [SerializeField]
        private GameObject aircraft;
        [SerializeField]
        private GameObject fireEngine;
        [SerializeField]
        private GameObject canonOne;
        [SerializeField]
        private GameObject canonTwo;
        [SerializeField]
        private const int MAX_PLAYERS = 4;


        private void Awake()
        {
            inputManager = FindObjectOfType<InputManager>();
        }


        private void Start()
        {
            for(int i = 1; i <= MAX_PLAYERS; i++)
            {
                var controllerID = inputManager.GetControllerID(i);
                AddPlayer(controllerID);
            }
        }



        private void OnEnable()
        {
            InputManager.controllerStatusChange += HandlePlayers;
        }


        private void OnDisable()
        {
            InputManager.controllerStatusChange -= HandlePlayers;
        }



        private void HandlePlayers(ControllerStatus controllerStatus, ControllerID controllerID)
        {
            switch(controllerStatus)
            {
                case ControllerStatus.Reconnected:
                    Reconnected(controllerID);
                    break;
                case ControllerStatus.Disconnected:
                    Disconnected(controllerID);
                    break;
            }
        }


        /// <summary>
        /// Before sending the Callback. Check this player exists. If not add the player. 
        /// </summary>
        /// <param name="controllerID"></param>
        private void Reconnected(ControllerID controllerID)
        {
            var playerWithController = players.Exists(playerData => { return (playerData.ControllerID == controllerID); });
            if(!playerWithController)
            {
                AddPlayer(controllerID);
            }

            if(playerReconnected != null)
                playerReconnected(controllerID);
        }



        private void Disconnected(ControllerID controllerID)
        {
            var playerWithController = players.Exists(playerData => { return (playerData.ControllerID == controllerID); });
            if(!playerWithController)
            {
                Debug.Log("ERROR: Player who has not been assigned a controller has been disconnected.");
                return;
            }

            if(playerDisconnected != null)
                playerDisconnected(controllerID);
        }

        /// <summary>
        /// Currently only adding the controllerID
        /// TODO: Setup the PlayerType management so each player is given a different role.
        /// </summary>
        /// <param name="controllerID"></param>
        private void AddPlayer(ControllerID controllerID)
        {
            players.Add(new PlayerData(controllerID));

            Debug.Log("MESSAGE: Adding player " + controllerID);

            // TODO: Turn this into a loop/ clean it up. 
            if((int)controllerID == 1)
            {
                players[players.Count - 1].PlayerType = ObjectType.Aircraft;
            }
            else if((int)controllerID == 2)
            {
                players[players.Count - 1].PlayerType = ObjectType.Driver;
            }
            else if((int)controllerID == 3)
            {
                players[players.Count - 1].PlayerType = ObjectType.Cannon_One;
            }
            else if((int)controllerID == 4)
            {
                players[players.Count - 1].PlayerType = ObjectType.Cannon_Two;
            }
            else
            {
                players[players.Count - 1].PlayerType = ObjectType.Unassigned;
            }



            if(players[players.Count - 1].PlayerType == ObjectType.Unassigned)
            {
                Debug.Log("ERROR: Player has not correctly been assigned a objectType/ role");
                return;
            }
            playerCreated(players[players.Count - 1].ControllerID, players[players.Count - 1].PlayerType);
        }

    }
}