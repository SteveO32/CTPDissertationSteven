using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Nicolas Smith, Connor Rhone, Josh Fenlon
// Purpose:		Behaviour of tank shell, penetration and others
// Namespace:	GG
//
//============================================================================//

namespace GG
{

    public class ProjectileBehaviour : MonoBehaviour
    {
        public float expiryTime = 3f;
        public Explosion explosion;
        private bool dealtDamage = false;
        private float damageValue = 1;

        Vector3 prevPos, prevPrevPos;
        int startCheck = -2;
        public FireProjectile parent = null;
        bool exploded = false;

        //disables explosions doing damage, and instead requires a direct hit
        bool impactDamage = false;

        private void Start()
        {
            Destroy(gameObject, expiryTime);
            prevPos = transform.position;
            prevPrevPos = transform.position;
        }

        private void Update()
        {
            //Debug.DrawRay(prevPos, transform.position - prevPrevPos, Color.red);

            //do the raycast
            if (startCheck > 0)
            {
                RaycastHit result;
                if (Physics.Raycast(prevPrevPos, transform.position - prevPrevPos, out result, Vector3.Distance(prevPrevPos, transform.position)))
                {

                    if (result.transform != transform)
                    {
                        transform.position = result.point;

                        //debug test for deform
                        if (GameObject.FindObjectOfType<DeformableMeshWithSpatialPartioning>())
                        {
							GameObject.FindObjectOfType<DeformableMeshWithSpatialPartioning>().addDestroyPoint(result.point, 2, 10);
                        }
                        Explode();
                    }
                }
            }
            else
            {
                startCheck++;
            }

            prevPrevPos = prevPos;
            prevPos = transform.position;
        }



        private void OnTriggerEnter(Collider other)
        {
            testCollision(other.gameObject, true);
        }

		private void testCollision(GameObject other, bool isTrigger)
        {
            if (GameObject.FindObjectOfType<DeformableMeshWithSpatialPartioning>())
            {
                GameObject.FindObjectOfType<DeformableMeshWithSpatialPartioning>().addDestroyPoint(transform.position, 2, 10);
            }

            if (impactDamage)
            {
                if (other.tag == "Tank" && dealtDamage == false)
                {
                    int playerID = parent.parentObj.GetComponent<TurretRotation>().GetPlayerID();
                    dealtDamage = true;
                    Instantiate(explosion, transform.position, transform.rotation);

                    int damageDone = other.GetComponent<BasicHealthTest>().takeDamage(damageValue);
                    TankGameManager tGM = GameObject.FindObjectOfType<TankGameManager>();
                    switch (damageDone)
                    {
                        case 1:
                            tGM.addDamageToPD(damageValue, playerID);
                            break;
                        case 2:
                            tGM.addKillToPD(playerID);
                            break;
                    }

					Explode();
                    Destroy(gameObject);
                }
            }

			if (!isTrigger) {
				Explode();
			}
            
        }

        void OnCollisionEnter(Collision col)
        {
            testCollision(col.gameObject, false);
        }

        void Explode()
        {
            if (exploded) return;
            exploded = true;

            int playerID = parent.parentObj.GetComponent<TurretRotation>().GetPlayerID();
            Explosion newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            if (!impactDamage)
            {
                newExplosion.Activate(playerID, damageValue, parent.parentObj.GetComponent<BasicHealthTest>());
            }
            Destroy(gameObject);
        }


    }
}