using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO : Put massive sphere trigger aroudn the city which wants to be bombed then..
///             - if the aircraft is not in the spehre trigger bombs have no effect. 
/// </summary>


namespace FH
{
    public class BombCast : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_firePrefab;
        [SerializeField]
        private Vector3[] directions;
        [SerializeField]
        private float m_rayLength = 10f;
        [SerializeField]
        private bool m_defused = false;
        [SerializeField]
        private int m_maxHits = 3;







        private void Start()
        {
            directions = new Vector3[14];
            directions[0] = Vector3.up;
            directions[1] = Vector3.down;
            directions[2] = Vector3.forward;
            directions[3] = Vector3.back;
            directions[4] = Vector3.left;
            directions[5] = Vector3.right;
            directions[6] = (Vector3.up - Vector3.right);
            directions[7] = (Vector3.up - Vector3.left);
            directions[8] = (Vector3.up - Vector3.forward);
            directions[9] = (Vector3.up - Vector3.back);
            directions[10] = (Vector3.down - Vector3.right);
            directions[11] = (Vector3.down - Vector3.left);
            directions[12] = (Vector3.down - Vector3.forward);
            directions[13] = (Vector3.down - Vector3.back);
        }


        private void OnCollisionEnter(Collision other)
        {
            if(m_defused)
                return;

            var hitList = new List<RaycastHit>();

            for(int i = 0; i < directions.Length; i++)
            {
                //var rayHits = Physics.RaycastAll(transform.position, directions[i], m_rayLength);
                //foreach(var hit in rayHits)
                //{
                //    if(hit.transform.tag == "Building")
                //        hitList.Add(hit);
                //}


                var _rayHits = Physics.SphereCastAll(transform.position, m_rayLength, directions[i]);
                foreach(var hit in _rayHits)
                {
                    if(hit.transform.tag == "Building")
                        hitList.Add(hit);
                }
            }
            //Debug.Log("Distance - " + Vector3.Distance(Vector3.down, Vector3.up));
            if(hitList.Count <= 0)
                return;

            // Add fire object to each point. 
            List<int> points = new List<int>(m_maxHits);
            for(int i = 0; i < m_maxHits; i++)
            {
                var id = Random.Range(0, hitList.Count);
                var hit = hitList[id];

                if(points.Count > 0 && points.Contains(id) && !hit.collider)
                {
                    i--;
                    continue;
                }

                if(!hitList.Exists(n =>
                {
                    var dist = Vector3.Distance(n.point, hit.point);
                    return dist < 5f;
                }))
                {
                    i--;
                    continue;
                }


                // TODO: To Be... bomb cast is managed by BombManager which communicates with FireManager.
                // This is HACKY
                var fire = Instantiate(m_firePrefab, hit.point, Quaternion.identity);
                FireManager.FirePoints.Add(fire.GetComponent<SphereCollider>());
                points.Add(id);

                // TODO: Make this less coupled. 
                FH_GameManager.Score++;
            }

            transform.GetComponent<Collider>().enabled = false;




            // TODO: Destruct bomb mesh and destroy after x seconds. 
            //Debug.DrawRay(transform.position, Vector3.up * 10f, Color.red, 100f);
            //Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red, 100f);
            //Debug.DrawRay(transform.position, Vector3.forward * 10f, Color.red, 100f);
            //Debug.DrawRay(transform.position, Vector3.back * 10f, Color.red, 100f);
            //Debug.DrawRay(transform.position, Vector3.left * 10f, Color.red, 100f);
            //Debug.DrawRay(transform.position, Vector3.right * 10f, Color.red, 100f);
            //
            //
            //Debug.DrawRay(transform.position, (Vector3.up - Vector3.right) * 10f, Color.yellow, 100f);
            //Debug.DrawRay(transform.position, (Vector3.up - Vector3.left) * 10f, Color.yellow, 100f);
            //Debug.DrawRay(transform.position, (Vector3.up - Vector3.forward) * 10f, Color.yellow, 100f);
            //Debug.DrawRay(transform.position, (Vector3.up - Vector3.back) * 10f, Color.yellow, 100f);
            //
            //Debug.DrawRay(transform.position, (Vector3.down - Vector3.right) * 10f, Color.yellow, 100f);
            //Debug.DrawRay(transform.position, (Vector3.down - Vector3.left) * 10f, Color.yellow, 100f);
            //Debug.DrawRay(transform.position, (Vector3.down - Vector3.forward) * 10f, Color.yellow, 100f);
            //Debug.DrawRay(transform.position, (Vector3.down - Vector3.back) * 10f, Color.yellow, 100f);




            m_defused = true;
        }
    }
}