using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - Team Lunatics 2018 ====================//
//
// Author:		Charlie Saunders
// Purpose:		Script for Golf Hole behaviour 
// Namespace:	LT
//
//===============================================================================//

namespace LT
{
    public class GolfHole : MonoBehaviour
    {

        // Use this for initialization
        public float forceAmount = 500.0f;
        public float sqrMag = 0;
        public ParticleEmitter particle;
        public ParticleEmitter particle2;
        public ParticleEmitter particle3;
        public ParticleEmitter particle4;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        //void OnTriggerEnter(Collider other)
        //{
        //       if (other.tag == "GolfBall")
        //       {
        //           other.GetComponent<ConstantForce>().force = (transform.position - other.transform.position).normalized * forceAmount * Time.smoothDeltaTime;
        //           Debug.Log("Trigger Enter");
        //       }
        //}
        void OnTriggerStay(Collider other)
        {
            if (other.tag == "GolfBall")
                if ((transform.position - other.transform.position).sqrMagnitude < 5.0f)
                {
                    other.GetComponent<Collider>().enabled = false;
                    other.transform.position = new Vector3(transform.position.x, other.transform.position.y - 0.5f, transform.position.z);
					other.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ; 
                    other.GetComponent<Rigidbody>().velocity = Vector2.zero;
					
                    particle.emit = true;
                    particle2.emit = true;
                    particle3.emit = true;
                    particle4.emit = true;

                    FindObjectOfType<GolfGameManager>().BallInHole(other.gameObject);
                }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "GolfBall")
            {
                other.GetComponent<ConstantForce>().force = (((transform.position - other.transform.position) * (Time.deltaTime * forceAmount)));
                Debug.Log("Trigger Enter");

            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.tag == "GolfBall")
            {
                other.GetComponent<ConstantForce>().force = Vector3.zero;
                Debug.Log("Trigger Exit");
            }
        }
    }
}
//other.GetComponent<Rigidbody>().AddForce((transform.position - other.transform.position).normalized* forceAmount * Time.smoothDeltaTime);