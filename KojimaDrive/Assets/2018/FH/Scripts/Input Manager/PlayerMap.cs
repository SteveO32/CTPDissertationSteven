using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FH
{
    public class PlayerMap : ScriptableObject
    {
        public int RewiredPlayerId  { get; private set; }
        public int GamePlayerId     { get; private set; }

        public PlayerMap(int rewiredPlayerId, int gamePlayerId)
        {
            RewiredPlayerId = rewiredPlayerId;
            GamePlayerId = gamePlayerId;
        }
    }
}