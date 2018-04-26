using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Josh Fenlon
// Purpose:		Script for the firing of the cryo bomb also the script for 
//              the collision detection of the cryo bomb.
// Namespace:	GG
//
//============================================================================//


namespace GG
{

	public class CryoBomb : BouncingBombScript {

		public float slowDownTime = 1;
		public float slowDownAmmout = 2;
		public float slowDownTurretAmmout = 2;
		public float slowDownRange = 2;
		public SphereCollider rangeCollider;
		public float hitMargin = 0.5f;
		bool disableEffects = false;
		bool hasHit = false;
		[SerializeField]
		public List<objectData> withinRangeTanks;
		public objectData hitTank;

		[System.Serializable]
		public class objectData
		{
			public GameObject obj;
			public float originalSpeed = 0;
			public Vector2 originalTurretSpeed = Vector2.zero;
		}

		public override void onStart ()
		{
			rangeCollider.radius = slowDownRange;
		}

		public override void onUpdate()
		{
			if (!hasHit) {
				base.onUpdate ();

				if (!disableEffects) {
					affectTanks (false);
				}

			} else {
				disableEffects = true;
				timer = 0;
				if (slowDownTime >= 0) {
					slowDownTime -= Time.deltaTime;
					hitTank.obj.GetComponent<TankController> ().maxMotorTorque = 0;
					hitTank.obj.GetComponent<TurretRotation> ().setRotationSpeed (hitTank.originalTurretSpeed / slowDownAmmout);
				} else {
					hitTank.obj.GetComponent<TankController> ().maxMotorTorque = hitTank.originalSpeed;
					hitTank.obj.GetComponent<TurretRotation> ().setRotationSpeed (hitTank.originalTurretSpeed);
					hitTank = null;
					affectTanks (true);
					timerEnded ();
				}
			}
		}

		void affectTanks(bool reset) {
			for (int a = 0; a < withinRangeTanks.Count; a++) {
				if (!reset) {
					if (Vector3.Distance (withinRangeTanks [a].obj.transform.position, transform.position) <= slowDownRange && !withinRangeTanks [a].obj.GetComponent<BasicHealthTest>().isShielded) {
						withinRangeTanks [a].obj.GetComponent<TankController> ().maxMotorTorque = withinRangeTanks [a].originalSpeed / slowDownAmmout;
						withinRangeTanks [a].obj.GetComponent<TurretRotation> ().setRotationSpeed (withinRangeTanks [a].originalTurretSpeed / slowDownAmmout);
					} else {
						withinRangeTanks [a].obj.GetComponent<TankController> ().maxMotorTorque = withinRangeTanks [a].originalSpeed;
						withinRangeTanks [a].obj.GetComponent<TurretRotation> ().setRotationSpeed (withinRangeTanks [a].originalTurretSpeed);
						withinRangeTanks.RemoveAt (a);
					}
				} else {
					withinRangeTanks [a].obj.GetComponent<TankController> ().maxMotorTorque = withinRangeTanks [a].originalSpeed;
					withinRangeTanks [a].obj.GetComponent<TurretRotation> ().setRotationSpeed (withinRangeTanks [a].originalTurretSpeed);
					withinRangeTanks.RemoveAt (a);
				}
			}
		}

		public override void timerEnded ()
		{
			disableEffects = true;
			if (hitTank.obj == null) {
				base.timerEnded ();
			}
		}

		public override void collideWithTank (GameObject tank)
		{
			if (!disableEffects) {
				if (tank.GetComponent<BasicHealthTest> ()) {
					objectData temp = new objectData ();
					temp.obj = tank;
					temp.originalSpeed = tank.GetComponent<TankController> ().maxMotorTorque;
					temp.originalTurretSpeed = tank.GetComponent<TurretRotation> ().getRotationSpeed ();

					if (Vector3.Distance (transform.position, tank.transform.position) < hitMargin) {
						hasHit = true;
						hitTank = temp;
					} else {
						bool contains = false;
						for (int a = 0; a < withinRangeTanks.Count; a++) {
							if (withinRangeTanks [a].obj == tank) {
								contains = true;
								break;
							}
						}
						if (!contains) {
							withinRangeTanks.Add (temp);
						}
					}
				}
			}
		}
	}

}