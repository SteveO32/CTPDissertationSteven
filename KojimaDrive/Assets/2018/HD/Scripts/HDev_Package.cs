using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kojima Party - Team Hairy Devs 2018
// Author: Elliott Joseph Phillips
// Purpose: To set packages stolen
// Namespace: Hairy Devs
// Script Created: 27/03/2018 15:19
// Last Edited by 

namespace HDev
{
    public class HDev_Package : MonoBehaviour
    {
        [SerializeField]
        private bool stolen;
        // Use this for initialization
        void Start()
        {

        }

        public void SetStolen(bool setIsStolen)
        {
            stolen = setIsStolen;
            Debug.Log("SetStolen");
        }

        public bool GetStolen()
        {
            return stolen;
        }

        

    }
}