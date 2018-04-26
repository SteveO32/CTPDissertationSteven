using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - Team Frivolous 2018 ====================//
//
// Author:      Chloe Goument
// Purpose:     Handles the grabbing of trophies.
// Namespace:   TF
//
//===============================================================================//

namespace TF
{
    public class TrophyCollection : MonoBehaviour
    {
        int numplayers = 2;
        private void OnTriggerEnter(Collider other)
        {
            // Attaching the trophy to the player when they touch it.
            if (other.gameObject.tag == "Player")
            {
                transform.Find("Trophy").GetComponent<MeshCollider>().enabled = false;
                transform.Find("WoodenSpoon").GetComponent<MeshCollider>().enabled = false;
                gameObject.transform.GetComponent<Rigidbody>().Destroy();
                transform.SetParent(other.gameObject.transform.Find("RightHand"));
            }
        }
    }
}