using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace JB
{

    /// <summary>
    /// All variables in this class are nullable types.
    /// </summary>
    public class ProjectileRequest
    {
        public int? playerID = null;
        public float? damageMod = null;
        public Vector3? vTarget = null;
        public Transform tTarget = null;
    }

    public class Projectile : MonoBehaviour
    {
        [SerializeField] int damage = 1;
        [SerializeField] float speed = 5;
        [SerializeField] LayerMask hitLayers = 0;
        [SerializeField] float destroyDelay = 3;

        protected ProjectileRequest requestInfo;


        public void Init(ProjectileRequest _requestInfo)
        {
            requestInfo = _requestInfo;
        }


        void Start()
        {
            if (destroyDelay > 0)
            {
                Destroy(this.gameObject, destroyDelay);
            }
        }


        void FixedUpdate()
        {
            TravelToTarget();
        }


        void TravelToTarget()
        {
            if (requestInfo.vTarget != null)
            {
                Vector3 prevPos = transform.position;

                Vector3 dir = ((Vector3)requestInfo.vTarget - transform.position).normalized;
                transform.position += dir * speed * Time.fixedDeltaTime;

                HitCheck(prevPos, transform.position);
            }
            else if (requestInfo.tTarget != null)
            {
                Vector3 prevPos = transform.position;

                Vector3 dir = (requestInfo.tTarget.position - transform.position).normalized;
                transform.position += dir * speed * Time.fixedDeltaTime;

                HitCheck(prevPos, transform.position);
            }
            else
            {
                Vector3 prevPos = transform.position;

                Vector3 dir = transform.forward;
                transform.position += dir * speed * Time.fixedDeltaTime;

                HitCheck(prevPos, transform.position);
            }
        }


        void HitCheck(Vector3 _prevPos, Vector3 _currentPos)
        {
            Vector3 diff = (_currentPos - _prevPos);

            RaycastHit hit;
            Physics.Raycast(_prevPos, diff.normalized, out hit, diff.magnitude, hitLayers);

            if (hit.collider == null)
                return;

            var target = hit.collider.GetComponent<Target>();

            if (target != null)
            {
                int totalDamage = damage;
                if (requestInfo.damageMod != null)
                    totalDamage = (int)(damage * (float)requestInfo.damageMod);
                target.TargetHit(requestInfo.playerID, gameObject.transform, totalDamage);
            }

            //Instantiate(ricochet_prefab, hit.point, Quaternion.LookRotation(hit.normal));
            AudioManager.PlayOneShot("ricochet");

            Destroy(this.gameObject);
        }

    }

} // namespace JB