using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    public static class PlayerMaterialsManager
    {
        [SerializeField]
        static List<PlayerCollection> playerCollections;
        
        static PlayerMaterialsManager()
        {
            //Apply some default materials for ease of testing
            playerCollections = new List<PlayerCollection>();
            playerCollections.Add(Resources.Load("PlayerCollections/ArcherGirl") as PlayerCollection);
            playerCollections.Add(Resources.Load("PlayerCollections/Chef") as PlayerCollection);
            playerCollections.Add(Resources.Load("PlayerCollections/Pirate") as PlayerCollection);
            playerCollections.Add(Resources.Load("PlayerCollections/PoliceMan") as PlayerCollection);
        }

        public static void setPlayerCollections(List<PlayerCollection> coll)
        {
            playerCollections = coll;
        }

        public static PlayerCollection getPlayerCollection(int playerIndex)
        {
            return playerCollections[playerIndex];
        }
    }
}