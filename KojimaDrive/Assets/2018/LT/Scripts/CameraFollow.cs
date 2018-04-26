using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Rewired;
using System.Text.RegularExpressions;

namespace LT
{
    public class CameraFollow
    {
        [RuntimeInitializeOnLoadMethod]
        static void InitializeModule()
        {
            CinemachineCore.GetInputAxis = GetInput;
        }

        static float GetInput(string name)
        {
            var resultString = Regex.Match(name, @"\d+").Value;
            int playerNumber = int.Parse(resultString);
            var player = ReInput.players.GetPlayer(playerNumber);
            return player.GetAxis("OrbitCamera");
        }
    }
}
