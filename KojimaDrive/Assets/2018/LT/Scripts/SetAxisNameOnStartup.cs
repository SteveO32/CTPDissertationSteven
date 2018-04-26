using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    public class SetAxisNameOnStartup : MonoBehaviour {

        // Use this for initialization
        void Start() {
            GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine.CinemachineOrbitalTransposer>().m_XAxis.m_InputAxisName =
                 "Player" + transform.root.GetComponentInChildren<HumanCharacterControl>().playerId;
    
        }

        // Update is called once per frame
        void Update() {

        }
    }
}