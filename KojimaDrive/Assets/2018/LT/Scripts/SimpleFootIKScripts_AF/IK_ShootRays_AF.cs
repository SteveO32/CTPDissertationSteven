using UnityEngine;
using System.Collections;

namespace AnimFollow
{
	public partial class SimpleFootIK_AF
	{
		void ShootIKRays()
		{		
			leftFootPosition = new Vector3(leftFoot.position.x, leftFootPosition.y, leftFoot.position.z);
			rightFootPosition = new Vector3(rightFoot.position.x, rightFootPosition.y, rightFoot.position.z);

			// Shoot ray to determine where the feet should be placed.
			Debug.DrawRay(rightFootPosition + Vector3.up * maxStepHeight, Vector3.down * raycastLength, Color.green);
			if (!Physics.Raycast(rightFootPosition + Vector3.up * maxStepHeight, Vector3.down, out raycastHitRightFoot, raycastLength, layerMask, QueryTriggerInteraction.Ignore))
			{
				raycastHitRightFoot.normal = Vector3.up;
				raycastHitRightFoot.point = rightFoot.position - raycastLength * Vector3.up;
			}
			footForward = rightToe.position - rightFoot.position;
			footForward = new Vector3(footForward.x, 0f, footForward.z);
			footForward = Quaternion.FromToRotation(Vector3.up, raycastHitRightFoot.normal) * footForward;
			if (!Physics.Raycast(rightFootPosition + footForward + Vector3.up * maxStepHeight, Vector3.down, out raycastHitToe, maxStepHeight * 2f, layerMask, QueryTriggerInteraction.Ignore))
			{
				raycastHitToe.normal = raycastHitRightFoot.normal;
				raycastHitToe.point = raycastHitRightFoot.point + footForward;
			}
			else
			{		
				if(raycastHitRightFoot.point.y < raycastHitToe.point.y - footForward.y)
					raycastHitRightFoot.point = new Vector3(raycastHitRightFoot.point.x, raycastHitToe.point.y - footForward.y, raycastHitRightFoot.point.z);
				
				// Put avgNormal in foot normal
				raycastHitRightFoot.normal = (raycastHitRightFoot.normal + raycastHitToe.normal).normalized;
			}

			Debug.DrawRay(leftFootPosition + Vector3.up * maxStepHeight, Vector3.down * raycastLength , Color.red);
			if (!Physics.Raycast(leftFootPosition + Vector3.up * maxStepHeight, Vector3.down, out raycastHitLeftFoot, raycastLength, layerMask, QueryTriggerInteraction.Ignore))
			{
				raycastHitLeftFoot.normal = Vector3.up;	
				raycastHitLeftFoot.point = leftFoot.position - raycastLength * Vector3.up;
			}
			footForward = leftToe.position - leftFoot.position;
			footForward = new Vector3(footForward.x, 0f, footForward.z);
			footForward = Quaternion.FromToRotation(Vector3.up, raycastHitLeftFoot.normal) * footForward;
			if (!Physics.Raycast(leftFootPosition + footForward + Vector3.up * maxStepHeight, Vector3.down, out raycastHitToe, maxStepHeight * 2f, layerMask, QueryTriggerInteraction.Ignore))
			{
				raycastHitToe.normal = raycastHitLeftFoot.normal;
				raycastHitToe.point = raycastHitLeftFoot.point + footForward;
			}
			else
			{
				if(raycastHitLeftFoot.point.y < raycastHitToe.point.y - footForward.y)
					raycastHitLeftFoot.point = new Vector3(raycastHitLeftFoot.point.x, raycastHitToe.point.y - footForward.y, raycastHitLeftFoot.point.z);
				
				// Put avgNormal in foot normal
				raycastHitLeftFoot.normal = (raycastHitLeftFoot.normal + raycastHitToe.normal).normalized;
			}

            // Do not tilt feet if on to steep an angle
            if (raycastHitLeftFoot.normal.y < maxIncline)
            {
                if (!animFollow.GetComponent<RagdollControl_AF>().gettingUp && !animFollow.GetComponent<RagdollControl_AF>().falling)
                {
                    animFollow.GetComponent<RagdollControl_AF>().falling = true;
                    Debug.DrawRay(raycastHitLeftFoot.point, raycastHitLeftFoot.normal, Color.blue, 10.0f);
                    foreach (var rb in animFollow.GetComponent<RagdollControl_AF>().ragdollRootBone.GetComponentsInChildren<Rigidbody>())
                        rb.AddForce(raycastHitLeftFoot.normal * 20, ForceMode.VelocityChange);
                }
                raycastHitLeftFoot.normal = Vector3.RotateTowards(Vector3.up, raycastHitLeftFoot.normal, Mathf.Acos(maxIncline), 0f);
            }
            if (raycastHitRightFoot.normal.y < maxIncline)
			{
                if (!animFollow.GetComponent<RagdollControl_AF>().gettingUp && !animFollow.GetComponent<RagdollControl_AF>().falling)
                {
                    animFollow.GetComponent<RagdollControl_AF>().falling = true;
                    Debug.DrawRay(raycastHitRightFoot.point, raycastHitRightFoot.normal, Color.blue, 10.0f);
                    foreach (var rb in animFollow.GetComponent<RagdollControl_AF>().ragdollRootBone.GetComponentsInChildren<Rigidbody>())
                        rb.AddForce(raycastHitRightFoot.normal * 20, ForceMode.VelocityChange);
                }
                raycastHitRightFoot.normal = Vector3.RotateTowards(Vector3.up, raycastHitRightFoot.normal, Mathf.Acos(maxIncline), 0f);

            }

            if (followTerrain)
			{
				transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, Mathf.Min(raycastHitLeftFoot.point.y, raycastHitRightFoot.point.y), transformYLerp * extraYLerp * deltaTime), transform.position.z);
//				Debug.DrawLine(raycastHitLeftFoot.point, raycastHitRightFoot.point);
			}
		}
	}
}
