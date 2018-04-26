using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//===================== Kojima Drive - FluffyHedgehog 2018 ====================//
//
// Author:		Viv
// Purpose:		[NOT IMPLEMENTED] Data is passed from the AircraftManager but OnCollisionEnter is always running.
// Namespace:	FH
//
//===============================================================================//


namespace FH
{
    public class AircraftCollision : MonoBehaviour
    {
        private Vector3 respawnPosition = new Vector3(-150,24,2937);
        private Vector3 respawnRotation = new Vector3(0, 240, 0); 
        public GameObject player;
        Rigidbody rb;

        void Start()
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
        }

        void OnTriggerEnter(Collider collision)
        {
            // If plane collides with runway
            if (collision.gameObject.tag == "Runway")
            {
                // Fill up fuel and bomb stock ...

            }
            else
            {
                // .. otherwise pause for 2 seconds and then respawn at the runway
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.Sleep();
                StartCoroutine(Wait());
                player.transform.position = respawnPosition;
                player.transform.eulerAngles = respawnRotation;
            }

        }

        IEnumerator Wait()
        {
            Debug.Log(Time.time);
            yield return new WaitForSecondsRealtime(2);
            Debug.Log(Time.time);
        }

        // TODO: Handle collision with ground
    }
}