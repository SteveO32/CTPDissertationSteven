using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Kojima Party - Team Hairy Devs 2018
// Author: Piotr Lubinski
// Purpose: Interaction between players (hitting eachother to knock-off their packages)
// Namespace: Hairy Devs
// Script Created: 17/02/2018 19:00
// Last Edited by Owen Jackson 21/02/18 01:30

namespace HDev
{
    public class HDev_Collision : MonoBehaviour
    {
        float magnitude;                                            // Stores vehicles magnitude of impact upon colliding with another player
        Vector3 velocity;                                           // Stores vehicles velocity (in x,y,z) upon colliding with another player

        [Range(1,30)]                                               // slider within inspector to choose a number between 1 and 30
        public float impact = 20;                                   // stores a public variable which determines the neccesary strenght of impact before triggering a reaction
        public bool frontCollision = false;                         // Whether or not the player is hitting another car from the front (set from a different script & used in OnCollisionEnter)

        //GameObject lastHit; //stops the player from hitting the same person repeatedly        
        public List<KeyValuePair<GameObject, float>> lastHits; //stores recently hit players so you can't steal from them multiple times in a row
        float lastHitTimer; //timer to when you can hit the same player again
        float lastHitTimerMax = 2f;  

        //public PackageManager packageManager;
        public PackageManager packageManager;

        private void Start()
        {
            lastHits = new List<KeyValuePair<GameObject, float>>();
            //lastHit = null;
            packageManager = GetComponentInParent<PackageManager>();
        }

        void Update()
        {
            for(int i = 0; i < lastHits.Count; i++)
            {
                //update the timers for each last hit player
                lastHits[i] = new KeyValuePair<GameObject, float>(lastHits[i].Key, lastHits[i].Value - Time.deltaTime);
                //allow this player to be hit again when the timer is 0
                if(lastHits[i].Value <= 0)
                {
                    lastHits.RemoveAt(i);
                }
            }
        }

        void OnCollisionEnter(Collision col)                        // if trigger collider collides with another collider
        {
            if (!frontCollision) return;

            //if (GetComponent<Kojima.CarScript>())   //this set is for where we still have the collisions attached to the main car body, we should phase this check out in favour of the one in the large else version
            //{
                if (col.gameObject.tag == "Player")
                {
                    if (lastHits.FirstOrDefault(x => x.Key == col.gameObject).Key == null)
                    {
                        if (col.gameObject.GetComponent<PackageManager>().packages.Count > 0)
                        {
                            //if (GetComponent<Rigidbody>().velocity.sqrMagnitude > col.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude)
                            //{
                            magnitude = col.relativeVelocity.magnitude;         // save the magnitude
                            velocity = col.relativeVelocity;                    // save the velocity
                            Debug.Log("magnitude of hit:" + magnitude);
                            //float force = Vector3.Dot(col.contacts[0].normal, velocity);
                            //Vector3 impulse = col.impulse;
                            //Debug.Log("impulse from this: " + impulse.magnitude);
                            if (col.relativeVelocity.magnitude > impact)        // if imapact magnitude is bigger than "impact"
                            {                                                   // remove a package from the player you collide with
                                col.gameObject.GetComponent<PackageManager>().DestroyPackage();
                            //packageManager.AddPackage(packageManager.pack);
                            packageManager.AddPackage(packageManager.pack,true);

                            //Debug.Log("magnitude of hit:" + magnitude);
                            Debug.Log("velocity you hit them at: " + velocity);
                            }
                            //}
                        }
                        lastHits.Add(new KeyValuePair<GameObject, float>(col.gameObject, lastHitTimerMax));
                    }
                    //else
                    //{
                    //    Debug.Log("you already hit this player!");
                    //}
                }
            //}
            //else
            //{
            //    if (col.transform.parent.gameObject != transform.parent.gameObject) //prevents triggering by my own colliders
            //    {
            //        if (col.gameObject.tag == "CarRear")                     // and that collider belogs to Player controller car
            //        {
            //            if (!lastHits.FirstOrDefault(x => x.Key == col.gameObject).Key)
            //            {
            //                if (col.gameObject.GetComponentInParent<PackageManager>().packages.Count > 0)
            //                {
            //                    //if (GetComponent<Rigidbody>().velocity.sqrMagnitude > col.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude)
            //                    //{
            //                    magnitude = col.relativeVelocity.magnitude;         // save the magnitude
            //                    velocity = col.relativeVelocity;                    // save the velocity
            //                    Debug.Log("magnitude of hit:" + magnitude);
            //                    //float force = Vector3.Dot(col.contacts[0].normal, velocity);
            //                    //Vector3 impulse = col.impulse;
            //                    //Debug.Log("impulse from this: " + impulse.magnitude);
            //                    if (col.relativeVelocity.magnitude > impact)        // if imapact magnitude is bigger than "impact"
            //                    {                                                   // remove a package from the player you collide with
            //                        col.gameObject.GetComponentInParent<PackageManager>().DestroyPackage();
            //                        packageManager.AddPackage(packageManager.pack);
            //                        //Debug.Log("magnitude of hit:" + magnitude);
            //                        Debug.Log("velocity you hit them at: " + velocity);
            //                    }
            //                    //}
            //                }
            //                lastHits.Add(new KeyValuePair<GameObject, float>(col.gameObject, lastHitTimerMax));
            //            }
            //            else
            //            {
            //                Debug.Log("you already hit this player!");
            //            }
            //        }
            //    }
            //}
        }
    }
}

/**/
