using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Nicolas Smith
// Purpose:		Script for the firing of the bouncing bomb also the script for 
//              the collision detection of the bouncing bomb.
// Namespace:	GG
//
//============================================================================//


namespace GG
{
    public class BouncingBombScript : WeaponClass
    {
        public float yOffset = 1;
        public float timer = 5;
        public int projectileForce;
        public BasicHealthTest parent;
        public Explosion explosionType;
        Transform tankBody;

        // Use this for initialization
        void Start()
        {
           
        }

        public override void onUpdate()
        {
           timer -= Time.deltaTime;
           if (timer <= 0)
           {
				timerEnded ();
           }    
            
        }

		public override void testCollision(Collider other)
        {
            if (isInstantiate)
            { 
                if (other.transform.CompareTag("Tank"))
                {
                    Transform highestParent = other.transform;
                    BasicHealthTest tempObject = null;


                    while (highestParent.parent != null && tempObject == null)
                    {
                        tempObject = highestParent.GetComponent<BasicHealthTest>();
                        highestParent = highestParent.parent;
                    }

                    if (parent != tempObject)
                    {
						collideWithTank (tempObject.gameObject);
                    }
                }
            }
        }

		public virtual void collideWithTank(GameObject tank) {
			callExplosion(parent, explosionType, transform.position);
			Destroy(gameObject);
		}

		public virtual void timerEnded() {
			callExplosion(parent, explosionType, transform.position);
			Destroy(gameObject);
		}

        public override float weaponFire(GameObject tank)
        {
			GameObject newBomb = (GameObject)Instantiate(gameObject, (tank.transform.position - tank.transform.forward * 2.0f) + (yOffset * tank.transform.up), tank.transform.rotation);
            newBomb.GetComponent<Rigidbody>().velocity = -newBomb.transform.forward * projectileForce;
            newBomb.GetComponent<BouncingBombScript>().parent = tank.GetComponent<BasicHealthTest>();
            newBomb.GetComponent<BouncingBombScript>().timer = timer;
            newBomb.GetComponent<BouncingBombScript>().isInstantiate = true;
            decreaseWeaponCharges();


            return delay;
        }

        public override void weaponImpact()
        {

        } 
    }
}