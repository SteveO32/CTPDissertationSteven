using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Nicolas Smith
// Purpose:		The weapons that the player can use.
// Namespace:	GG
//
//============================================================================//

namespace GG
{
    public class Inventory : MonoBehaviour
    {
        public int inventoryLimit = 3;

        List<WeaponClass>weaponList = new List<WeaponClass>();

        Player controller;
        int index = 0;
        float weaponDelay = 0;

        private void Start()
        {
            controller = GetComponent<TurretRotation>().GetPlayer();
        }

        private void Update()
        {
            weaponDelay -= Time.deltaTime;
            
            if (controller.GetButtonDown("Use Powerup"))
            {
                if (!GetComponent<BasicHealthTest>().isShielded)
                {
                    if (weaponList.Count != 0)
                    {
                        if (weaponDelay <= 0)
                        {
                            weaponDelay = weaponList[index].weaponFire(transform.gameObject);

                            if (weaponList[index].charges <= 0)
                            {
                                weaponList.RemoveAt(index);
                                index = 0;
                                checkIndex();
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("No Powerups");
                    }
                }
            }

            if (controller.GetButtonDown("Cycle powerup Up"))
            {
                if (weaponList.Count != 0)
                {
                    displaySelectedPowerUp(weaponList[index]);
                    index++;
                    checkIndex();
                }
            }

            if (controller.GetButtonDown("Cycle powerup Down"))
            {
                if (weaponList.Count != 0)
                {
                    displaySelectedPowerUp(weaponList[index]); 
                    index--;
                    checkIndex();
                }
            }

        }

        private void checkIndex()
        {
            if (index > weaponList.Count - 1)
            {
                index = 0;
            }

            if (index < 0)
            {
                index = weaponList.Count - 1;
            }
            Debug.Log(index);
            Debug.Log(weaponList[index]);
        }

        public void assignPowerUp(WeaponClass powerUpType)
        {
            GameObject weaponType = Instantiate(powerUpType.gameObject, new Vector3(0.0f, -1000.0f, 0.0f), transform.rotation);
            weaponList.Add(weaponType.GetComponent<WeaponClass>());
            Debug.Log(powerUpType);
        }

        public void displaySelectedPowerUp(WeaponClass powerUpType)
        {

        }
    }
}
