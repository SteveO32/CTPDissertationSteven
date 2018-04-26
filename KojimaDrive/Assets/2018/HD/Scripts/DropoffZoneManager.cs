using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Kojima Party - Hairy Devs 2018
 * Author: Owen Jackson
 * Purpose: Manages the dropoff zones, e.g. (if randomising) which one is active, where the next one will be, etc.
 * Namespace: HDev
 * Last Edited: Owen Jackson @ 21/02/2018
*/

namespace HDev
{
    public class DropoffZoneManager : MonoBehaviour
    {
        private static DropoffZoneManager instance;
        public static DropoffZoneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DropoffZoneManager();
                }
                return instance;
            }
        }
        public List<DropoffZone> openZones;     //zones that haven't been used yet
        public List<DropoffZone> closedZones;   //zones that have been used already

        [SerializeField]
        private int numberOfZonesAtATime = 1;   //how many zones can be active at a given time?

        public DropoffZone currentZone; //the currently active zone

        // Use this for initialization
        void Start()
        {
            //fill the open list with all zones
            instance = this;
            openZones = FindObjectsOfType<DropoffZone>().ToList();
            if (openZones.Count > 0)
            {
                //deactivate them all
                foreach (DropoffZone zone in openZones)
                {
                    zone.ResetZone();
                    zone.gameObject.SetActive(false);
                }
                //pick a zone to start with
                currentZone = openZones[Random.Range(0, openZones.Count - 1)];
            }
            closedZones = new List<DropoffZone>();
            //Begin();
        }

        //called when the game begins to activate the first zone
        public void Begin()
        {
            //use this for an immediate start
            currentZone.gameObject.SetActive(true);
        }

        //cycles to the next pickup zone
        public void CycleZones(DropoffZone zone)
        {
            //make sure we don't get this one again until after cycling through all of the other zones
            closedZones.Add(zone);
            openZones.Remove(zone);
            zone.gameObject.SetActive(false);
            //if we've cycled through all existing zones
            if (openZones.Count <= 0)
            {
                //reset the open list and pick a new zone
                openZones = closedZones;
                closedZones = new List<DropoffZone>();
            }
            //randomly select the next zone to go to
            currentZone = openZones[(Random.Range(0, openZones.Count - 1))];
            currentZone.gameObject.SetActive(true);
        }

        private void Update()
        {
        }
    }
}