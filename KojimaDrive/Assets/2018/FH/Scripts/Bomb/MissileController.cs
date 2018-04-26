using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH
{
    public class MissileController : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> m_bombWagons = new List<Transform>();
        [SerializeField]
        private GameObject m_missilePrefab;
        [SerializeField]
        private float m_coolDownTime = 5f;
        private bool m_active = false;


        public void FireBombs()
        {
            if(m_active) return;
            if(m_bombWagons.Count <= 0)
            {
                Debug.LogWarning("Warning: No bomb wagons");
                return;
            }

            var rID = Random.Range(0, m_bombWagons.Count);
            var missile = Instantiate(
                m_missilePrefab,
                m_bombWagons[rID].position,
                Quaternion.Euler(0, 0, 90) * transform.localRotation).GetComponent<Missile>();

            //Debug.DrawLine(m_bombWagons[rID].position, transform.localPosition + transform.forward * 1000, Color.yellow, 100f);

            if(!missile)
            {
                Debug.LogWarning("Warning: Instansiatied Missle is missing Missle Script");
                return;
            }

            var heading = (transform.localPosition + transform.forward) - transform.localPosition;
            var distance = heading.magnitude;
            var direction = heading / distance;



            missile.Forward = direction;
            missile.Thrust = GetComponent<Rigidbody>().velocity;





            StartCoroutine(CoolDown(active => { m_active = active; }));

        }


        private IEnumerator CoolDown(System.Action<bool> active)
        {
            active(true);
            yield return new WaitForSeconds(m_coolDownTime);
            active(false);
        }





    }
}
