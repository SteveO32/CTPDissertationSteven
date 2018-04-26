using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Josh Fenlon
// Purpose:		Basic script to allow turret rotation to change based on variables
// Namespace:	GG
//
//============================================================================//

namespace GG {

	public class AllowTurretDifference : MonoBehaviour
	{

		[SerializeField]
		private List<float> within1 = new List<float>();
		[SerializeField]
		private List<float> within2 = new List<float>();
		[SerializeField]
		private List<Vector2> allow = new List<Vector2>();
		[SerializeField]
		private bool yawCheck = false;
		[SerializeField]
		private bool pitchCheck = false;
		[SerializeField]
		private TurretRotation tr;

		void Start()
		{
			tr = gameObject.GetComponent<TurretRotation> ();
		}

		// Update is called once per frame
		void Update ()
		{
			//loop through all within checks
			for (int a = 0; a < within1.Count; a++)
			{
				//check if user picked check yaw or pitch
				if (yawCheck) {
					//check if yaw is within
					if (!Functions.checkWithin (tr.getYawRotAngle (), new Vector2 (within1 [a], within2 [a])))
					{
						//set new pitch limit
						tr.setPitchLimit (allow [a]);
					}
				} else if (pitchCheck) {
					//check if pitch is within
					if (!Functions.checkWithin (tr.getPitchRotAngle (), new Vector2 (within1 [a], within2 [a])))
					{
						//set new yaw limit
						tr.setYawLimit (allow [a]);
					}
				}
			}
		}

	}

}