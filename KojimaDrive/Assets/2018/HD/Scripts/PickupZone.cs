using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Kojima Party - Hairy Devs 2018
 * Author: Owen Jackson
 * Purpose: Gives packages to the players when entered
 * Namespace: HDev
 * Last Edited: Owen Jackson @ 27/02/2018
*/

namespace HDev
{
    [System.Serializable]
    public class PickupZone : Zone //Note: might be able to abstract this class to become a general item-giver to the players (powerups, collectibles, etc.)
    {
        private HashSet<int> playerIDs;     //stores which players have collected from this spawn
        [SerializeField]
        private int packagesLeft;           //how many more packages can this zone give to players?
        [SerializeField]
        private int maxPackages;          //how many packages this zone has when it resets
        [SerializeField]
        private float timer;                //countdown to when this zone becomes invalid
        [SerializeField]
        private float maxTimer;             //start countdown from this

        public GameObject package;          //the object to spawn on the cars when collecting

        new private void Awake()
        {
            base.Awake();
            if(maxPackages == 0)
            {
                maxPackages = 1;
            }
        }

        private void Update()
        {            
            if(timer <= 0)
            {
                Deactivate();
            }
            if (packagesLeft <= 0)
            {
                Deactivate();
            }
            timer -= Time.deltaTime;
        }

        //give the collided player a package
        private void GivePackage(GameObject playerCar)
        {
            playerCar.GetComponent<PackageManager>().AddPackage(package,false);
            packagesLeft--;
        }

        //detects when players enter to give them packages
        protected void OnTriggerEnter(Collider other)
        {
            if (packagesLeft > 0)   //only perform logic if this zone can still hand out packages
            {
                if (other.tag == "Player")
                {
                    if (other.GetComponent<FH.LandVehicle>())
                    {
                        //Kojima.CarScript collidedCar = other.GetComponent<Kojima.CarScript>();
                        FH.LandVehicle collidedCar = other.GetComponent<FH.LandVehicle>();
                        if (!playerIDs.Contains(collidedCar.ControllerID))                  //this might break the game, probable fix is to add player index to land vehicle
                        {
                            //add this player's ID to the list so that they cannot collect from here anymore
                            playerIDs.Add(collidedCar.ControllerID);                        //this would also be player index
                            GivePackage(collidedCar.gameObject);
                        }
                    }
                }
            }
        }

        //disables the zone (used when the timer runs out)
        public void Deactivate()
        {
            //tell the zone manager to cycle to the next one
            PickupZoneManager.Instance.CycleZones(this);
            //gameObject.SetActive(false);
        }

        //Initialises values when the zone spawns and resets
        public override void ResetZone()
        {
            timer = maxTimer;
            playerIDs = new HashSet<int>();
            //Debug.Log("zone found " + Kojima.GameController.s_ncurrentPlayers + " player(s)");
            if (Kojima.GameController.s_ncurrentPlayers > 1)
            {
                //USE THIS IN FULL VERSION
                //packagesLeft = Kojima.GameController.s_ncurrentPlayers - 1;

                //PRE-ALPHA
                packagesLeft = 1;
            }
            else if (Kojima.GameController.s_ncurrentPlayers == 1)
            {
                packagesLeft = 1;
            }
            else
            {
                packagesLeft = maxPackages;
            }
        }
    }
}
