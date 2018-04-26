using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH
{


    public class PressStartToJoin : MonoBehaviour
    {
        //[SerializeField]
        //private MapCatergories mapCatergories = MapCatergories.Aircraft | MapCatergories.UI | MapCatergories.Assignment;
        [SerializeField]
        private int maxPlayers = 4;
        [SerializeField]
        private List<PlayerMap> playerMap;
        [SerializeField]
        private int gamePlayerIdCounter = 0;

        private void Awake()
        {
            playerMap = new List<PlayerMap>();

            for(int i = 0; i < Rewired.ReInput.players.playerCount; i++)
            {
                var player = Rewired.ReInput.players.GetPlayer(i);
                player.controllers.maps.AddMap(player.controllers.GetController(Rewired.ControllerType.Joystick, 0), player.controllers.maps.GetMap(Rewired.ControllerType.Joystick, 0, "Assignment", "Default"));
            }
        }


        private void Update()
        {
            for(int i = 0; i < Rewired.ReInput.players.playerCount; i++)
            {
                if(Rewired.ReInput.players.GetPlayer(i).GetButtonDown("Join Game"))
                {
                    AssignNextPlayer(i);
                }
            }   
        }

        private void AssignNextPlayer(int rewiredPlayerId)
        {
            if(playerMap.Count >= maxPlayers)
            {
                Debug.LogError("ERROR: Max Player limit is already reached");
                return;
            }
            int gamePlayerId = GetNextPlayerID();

            // Add the Rewired Player as the next open game player slot.
            playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

            Rewired.Player rewiredPlayer = Rewired.ReInput.players.GetPlayer(rewiredPlayerId);

            // Disable the Assignemnt map category in Player so no more JoinGame Action return.
            rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Assignment");

            // Enable UI control for this player now that they have joined. 
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "UI");

            Debug.Log("MESSAGE: Added Rewired Player ID - " + rewiredPlayerId + " to game player " + gamePlayerId);
        }

        private int GetNextPlayerID()
        {
            return gamePlayerIdCounter++;
        }
    }
}