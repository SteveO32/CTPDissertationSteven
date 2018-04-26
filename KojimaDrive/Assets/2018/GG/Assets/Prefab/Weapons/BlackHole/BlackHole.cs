﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlackHole : MonoBehaviour {

        public float PullRadius; // Radius to pull
        public float GravitationalPull; // Pull force
        public float MinRadius; // Minimum distance to pull from
        public float DistanceMultiplier; // Factor by which the distance affects force
        public GameObject blackHole;

        public LayerMask LayersToPull;
       

    
        void FixedUpdate()
        {
                   
            Collider[] colliders = Physics.OverlapSphere(transform.position, PullRadius, LayersToPull);

            foreach (var collider in colliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();

                if (rb == null) continue; // Can only pull objects with Rigidbody

                Vector3 direction = transform.position - collider.transform.position;

                if (direction.magnitude < MinRadius) continue;

                float distance = direction.sqrMagnitude * DistanceMultiplier + 1; // The distance formula

                // Object mass also affects the gravitational pull
                
                rb.AddForce(direction.normalized * (GravitationalPull / distance) * rb.mass * Time.fixedDeltaTime);
            }
        }
    private void OnCollisionEnter(Collision collision)
    {
        blackHole.GetComponent<Rigidbody>().useGravity = false;
        blackHole.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }


}

