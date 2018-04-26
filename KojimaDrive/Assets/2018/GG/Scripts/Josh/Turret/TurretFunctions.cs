using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Josh Fenlon
// Purpose:		Basic functions that are needed for turret rotations
// Namespace:	GG
//
//============================================================================//

namespace GG {
	
	public class TurretFunctions {

		//return testRot below with default params for limit, enable limit
		public static Vector3 testRotationTypeAndApplyChange (turretRotationAxis rotType, Vector3 input, float change, bool add) {
			return testRotationTypeAndApplyChange (rotType, input, change, add, new Vector2 (0, 0), false);
		}

		//return the vector3 applied with change and limit
		public static Vector3 testRotationTypeAndApplyChange (turretRotationAxis rotType, Vector3 input, float change, bool add, Vector2 limit, bool enableLimit) {
			Vector3 output = input;

			//check what rotation axis should be affected
			switch (rotType) {
			case turretRotationAxis.x:
				//apply rotation change to specific axis
				output.x = RotationFunctions.applyRotationChange (output.x, change, add, limit, enableLimit);
				break;
			case turretRotationAxis.y:
				output.y = RotationFunctions.applyRotationChange (output.y, change, add, limit, enableLimit);
				break;
			case turretRotationAxis.z:
				output.z = RotationFunctions.applyRotationChange (output.z, change, add, limit, enableLimit);
				break;
			}

			return output;
		}

		//return a vector3 that contains input speed 
		public static Vector3 returnRotationSpeed(turretRotationAxis rotType, float speed, Vector3 input, Vector3 aim) {
			Vector3 output = Vector3.zero;

			//check what rotation axis should be affected
			switch (rotType) {
			case turretRotationAxis.x:
				//apply speed change to specific axis
				output.x = speed;
				break;
			case turretRotationAxis.y:
				output.y = speed;
				break;
			case turretRotationAxis.z:
				output.z = speed;
				break;
			}

			return output;
		}

		//return whether axis are the same
		public static bool returnRotationCheck(turretRotationAxis rotType, Vector3 input, Vector3 aim) {

			//check what rotation axis should be checked
			switch (rotType) {
			case turretRotationAxis.x:
				if (input.x == aim.x) {
					return true;
				}
				break;
			case turretRotationAxis.y:
				if (input.y == aim.y) {
					return true;
				}
				break;
			case turretRotationAxis.z:
				if (input.z == aim.z) {
					return true;
				}
				break;
			}

			return false;
		}
	}
}