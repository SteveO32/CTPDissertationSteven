using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Josh Fenlon
// Purpose:		Controllable missile class
// Namespace:	GG
//
//============================================================================//

namespace GG
{
	public class ControllableMissile : WeaponClass
	{
		public TurretRotation parent;
		private Rewired.Player rewiredPlayer;
		private Rigidbody rigid;
		public Explosion explosionType;

		[SerializeField]
		private string yawInputAxis = "Turret Yaw";
		[SerializeField]
		private Vector3 yawChange = Vector3.zero;
		[SerializeField]
		private string pitchInputAxis = "Turret Pitch";
		[SerializeField]
		private Vector3 pitchChange = Vector3.zero;
		[SerializeField]
		private Vector3 rot = Vector3.zero;
		[SerializeField]
		private Vector3 aimRot = Vector3.zero;
		[SerializeField]
		private Vector3 spawnRot = Vector3.zero;

		[SerializeField]
		private float speed = 0;
		[SerializeField]
		private float fov = 60;
		[SerializeField]
		private float timer = 0;

		public override void onStart()
		{
			rewiredPlayer = Rewired.ReInput.players.GetPlayer (parent.GetPlayerID ());
			toggleCamera (false);
			rigid = GetComponent<Rigidbody> ();
		}

		public override void onUpdate()
		{
			if (rewiredPlayer != null) {

				aimRot += yawChange * (rewiredPlayer.GetAxis (yawInputAxis) * Time.deltaTime);
				aimRot += pitchChange * (rewiredPlayer.GetAxis (pitchInputAxis) * Time.deltaTime);
				rigid.velocity = transform.forward * speed * Time.deltaTime;

				if (rewiredPlayer.GetButtonDown ("Use Powerup")) {
					timer = 0;
				}

			} else {
				if (parent != null) {
					rewiredPlayer = Rewired.ReInput.players.GetPlayer (parent.GetPlayerID ());
				}
				toggleCamera (false);
			}

			timer -= Time.deltaTime;

			if (timer <= 0) {
				callExplosion(parent.GetComponent<BasicHealthTest>(), explosionType, transform.position);
				toggleCamera (true);
				Destroy(gameObject);
			}
		}

		public override void onFixedUpdate ()
		{
			rot = Vector3.Lerp (rot, aimRot, Time.fixedDeltaTime);
			transform.eulerAngles = rot;
			GetComponentInChildren<Camera> ().transform.position = Vector3.Lerp (GetComponentInChildren<Camera> ().transform.position, transform.position, Time.fixedDeltaTime);
		}

		public override void testCollision(Collider other)
		{
			if (isInstantiate)
			{
				if (other.transform.CompareTag ("Tank")) {
					Transform highestParent = other.transform;
					TurretRotation tempObject = null;

					while (highestParent.parent != null && tempObject == null) {
						tempObject = highestParent.GetComponent<TurretRotation> ();
						highestParent = highestParent.parent;
					}

					if (parent != tempObject) {
						callExplosion (parent.GetComponent<BasicHealthTest> (), explosionType, transform.position);
						toggleCamera (true);
						Destroy (gameObject);
					}
				} else {
					if (!other.isTrigger) {
						callExplosion (parent.GetComponent<BasicHealthTest> (), explosionType, transform.position);
						toggleCamera (true);
						Destroy (gameObject);
					}
				}
			}
		}

		void toggleCamera(bool enabled) {
			parent.transform.parent.GetComponentInChildren<Camera> ().enabled = enabled;
			parent.GetComponent<TurretRotation> ().enabled = enabled;
			parent.GetComponent<TankController> ().canDrive = enabled;
			parent.GetComponent<Inventory> ().enabled = enabled;

			GetComponentInChildren<Camera> ().rect = parent.transform.parent.GetComponentInChildren<Camera> ().rect;
			GetComponentInChildren<Camera> ().fieldOfView = fov;
			GetComponentInChildren<Camera> ().enabled = !enabled;
			GetComponentInChildren<Camera> ().transform.position = parent.transform.position;
		}

		public override float weaponFire(GameObject tank)
		{
			parent = tank.GetComponent<TurretRotation> ();

			GameObject newMissile = GameObject.Instantiate (gameObject, parent.getAimPointer().transform.position, Quaternion.Euler(Vector3.zero));
			newMissile.GetComponent<ControllableMissile> ().parent = parent;
			newMissile.GetComponent<ControllableMissile> ().isInstantiate = true;
			newMissile.GetComponent<ControllableMissile> ().rot = spawnRot;

			decreaseWeaponCharges ();
			return delay;
		}

	}
}