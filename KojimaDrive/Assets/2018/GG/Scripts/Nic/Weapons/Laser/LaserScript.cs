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
    public class LaserScript : WeaponClass
    {
        public int range = 500;
		public int destructionRange = 1;
		public int destructionMaxRange = 2;
        public float weaponDamage = 3;
        float timer = 1;
		public GameObject parent;
        LineRenderer line;
        TurretRotation turretRotation;
        RaycastHit hit;

		public override void onStart()
        {
			line = GetComponent<LineRenderer> ();
			line.enabled = true;
			turretRotation = parent.GetComponent<TurretRotation> ();
			line.positionCount = 2;
			line.SetPosition (0, turretRotation.getAimPointer ().transform.position);

			if (Physics.Raycast (turretRotation.getAimPointer ().transform.position + (turretRotation.getActualAimForward ()), turretRotation.getActualAimForward (), out hit, range)) {
				line.SetPosition (1, hit.point);
				if (hit.transform.CompareTag ("Tank")) {
					Transform highestParent = hit.transform;
					BasicHealthTest tempObject = null;


					while (highestParent.parent != null && tempObject == null) {
						tempObject = highestParent.GetComponent<BasicHealthTest> ();
						highestParent = highestParent.parent;
					}

					if (parent.GetComponent<BasicHealthTest> () != tempObject) {
						tempObject.takeDamage (weaponDamage);
						if (doesBuildingDamage) {
							callBuildingDamage (hit.point, destructionRange, destructionMaxRange);
						}
					}
				}
			} else {
				line.SetPosition (1, turretRotation.getAimPointer ().transform.position + (turretRotation.getActualAimForward () * range));
			}
        }

		public override void onUpdate()
		{
			timer -= Time.deltaTime;

			if (timer <= 0) {
				Destroy (gameObject);
			}
		}

        public override float weaponFire(GameObject tank)
		{
			GameObject newLaser = (GameObject)Instantiate(gameObject, tank.transform.position, tank.transform.rotation);
			newLaser.GetComponent<LaserScript>().isInstantiate = true;
			newLaser.GetComponent<LaserScript> ().parent = tank;

			decreaseWeaponCharges ();
			return delay;
		}
        
    }
}