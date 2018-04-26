using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Kojima Party - Hairy Devs 2018
 * Author: Owen Jackson
 * Purpose: Remove packages from players and add to their score
 * Namespace: HDev
 * Last Edited: Owen Jackson @ 28/02/2018
*/

namespace HDev
{
    public class DropoffZone : Zone
    {
        [SerializeField]
        private float timer;                //countdown to when this zone becomes invalid
        [SerializeField]
        private float maxTimer;             //start countdown from this

        new private void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (timer <= 0)
            {
                Deactivate();
            }
            timer -= Time.deltaTime;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if(other.GetComponent<FH.LandVehicle>())
                {
                    FH.LandVehicle collidedCar = other.GetComponent<FH.LandVehicle>();
                    /*
                    //if the player is supposed to be delivering to this zone
                    if(playersDelivering.Contains(collidedCar))
                    {
                        //access the score manager and update their score
                        //remove any packages that are on the player
                        collidedCar.gameObject.GetComponent<PackageManager>();
                    }
                    */
                    if(collidedCar.GetComponent<PackageManager>() != null)
                    {
                        collidedCar.GetComponent<PackageManager>().BankPackages();
                    }
                }
            }
        }

        private void Deactivate()
        {
            DropoffZoneManager.Instance.CycleZones(this);
        }

        //Initialises values when the zone spawns and resets
        public override void ResetZone()
        {
            timer = maxTimer;
        }
    }
}