using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Nicolas Smith
// Purpose:		Script for picking up powerups.
// Namespace:	GG
//
//============================================================================//

namespace GG
{
    public class PowerUpScript : MonoBehaviour
    {
        [SerializeField]
        private List<WeaponClass> weaponList = new List<WeaponClass>();

        bool givenPowerUp = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Tank"))
            {
                if (other.GetComponentInParent<Inventory>() != null)
                {
                    if (!givenPowerUp)
                    {
                        pickUp(other);
                    }
                }
            }
        }

        void pickUp(Collider other)
        {
            int randomNumber = Random.Range(0, weaponList.Count);
            other.GetComponentInParent<Inventory>().assignPowerUp(weaponList[randomNumber]);
            givenPowerUp = true;
            Destroy(gameObject);
        }
    }
}