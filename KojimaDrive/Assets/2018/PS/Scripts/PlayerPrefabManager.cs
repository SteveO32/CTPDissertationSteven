using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public class PlayerInfo
    {
        public int playerNumber;
        public bool isAlive = true;
        public Theme playerTheme;
        public int ControllerID;

        public PlayerInfo(int playerNumber)
        {
            this.playerNumber = playerNumber;
            
            isAlive = true;

        }
    }

    public class PlayerPrefabManager : MonoBehaviour
    {

        //public struct player base prefabs
        public List<PlayerInfo> players = new List<PlayerInfo>();

        //player themes
        ThemeManager themeManager;

        // Use this for initialization
        public void ConnectPlayer(int playerNumber)
        {
            bool alreadyConnected = false;
            foreach (PlayerInfo player in players)
            {
                if (player.playerNumber == playerNumber)
                {
                    alreadyConnected = true;
                }
            }
            if (!alreadyConnected)
            {
                PlayerInfo newPlayer = new PlayerInfo(playerNumber);
                newPlayer.ControllerID = playerNumber;
                players.Add(newPlayer);

            }
        }
        public void DisconnectPlayer(int playerNumber)
        {
            bool alreadyDisconnected = true;
            foreach (PlayerInfo player in players)
            {
                if (player.playerNumber == playerNumber)
                {
                    alreadyDisconnected = false;
                }
            }
            if (!alreadyDisconnected)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].playerNumber == playerNumber)
                    {
                        players.RemoveAt(i);
                    }
                }
            }
        }

        public void SelectThemeForPlayer(int playerNumber, Theme theme)
        {
            PlayerInfo player = GetPlayerInfo(playerNumber);
            if (player != null)
            {
                player.playerTheme = theme;
            }
        }

        public PlayerInfo GetPlayerInfo(int playerNumber)
        {
            PlayerInfo playerInfo = null;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].playerNumber == playerNumber)
                {
                    playerInfo = players[i];
                }
            }
            return playerInfo;
        }

        GameObject GetPlayerPrefab(PlayerInfo playerInfo, Avatar playerAvatar)
        {
            GameObject playerPrefab = null;
            switch (playerAvatar)
            {
                case Avatar.CAR:
                    playerPrefab = playerInfo.playerTheme.car;
                    break;
                case Avatar.FIREENGINE:
                    playerPrefab = playerInfo.playerTheme.fireEngine;
                    break;
                case Avatar.PERSON:
                    playerPrefab = playerInfo.playerTheme.person;
                    break;
                case Avatar.PLANE:
                    playerPrefab = playerInfo.playerTheme.plane;
                    break;
                case Avatar.TANK:
                    playerPrefab = playerInfo.playerTheme.tank;
                    break;
            }
            return playerPrefab;
        }

        public GameObject GetPlayer(int playerNumber, Avatar playerAvatar)
        {
            return GetPlayerPrefab(GetPlayerInfo(playerNumber), playerAvatar);
        }

        public int PlayersConnected()
        {
            return players.Count;
        }
    }
}
