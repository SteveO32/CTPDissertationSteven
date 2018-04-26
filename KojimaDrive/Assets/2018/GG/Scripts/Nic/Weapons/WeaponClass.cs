using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Nicolas Smith
// Purpose:		Base weapon class.
// Namespace:	GG
//
//============================================================================//


namespace GG
{

    public class WeaponClass : MonoBehaviour
    {
        public int charges;
        public float delay = 0;
        public int damageValue;
        public bool isInstantiate = false;
		public bool testTriggerAsCollision = false;
		public bool doesBuildingDamage = false;

        private void Start()
        {
			if (isInstantiate)
			{
				onStart();
			}
        }

        private void Update()
        {
            if (isInstantiate)
            {
                onUpdate();
            }
        }

		private void FixedUpdate()
		{
			if (isInstantiate)
			{
				onFixedUpdate();
			}
		}

        public virtual void OnCollisionEnter(Collision other)
        {
			testCollision(other.collider);
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (testTriggerAsCollision)
            {
                testCollision(other);
            }
        }

		public virtual void testCollision(Collider other)
        {
            
        }

        public virtual float weaponFire(GameObject tank)
        {
            return delay;
        }

        public virtual void weaponImpact()
        {

        }

        public virtual void decreaseWeaponCharges()
        {
            charges--;
            if(charges <= 0)
            {
                Destroy(gameObject);
            }
        }

		public virtual void onStart()
		{

		}

        public virtual void onUpdate()
        {
            
        }

		public virtual void onFixedUpdate()
		{

		}

		public virtual void callBuildingDamage(Vector3 pos, float range, float maxRange) {
			if (GameObject.FindObjectOfType<DeformableMeshWithSpatialPartioning> ())
			{
				GameObject.FindObjectOfType<DeformableMeshWithSpatialPartioning> ().addDestroyPoint (pos, range, maxRange);
			}
		}

        public virtual void callExplosion(BasicHealthTest parent, Explosion explosion, Vector3 bombPos)
        {
            int playerID = parent.GetComponent<TurretRotation>().GetPlayerID();
            Explosion newExplosion = Instantiate(explosion, bombPos, transform.rotation);
            newExplosion.Activate(playerID, damageValue, parent);
        }

    }

}