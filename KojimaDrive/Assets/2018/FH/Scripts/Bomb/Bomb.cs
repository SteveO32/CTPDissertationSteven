using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FH
{
    public class Bomb : MonoBehaviour
    {
        public float acceleration = 5f;
        private new Rigidbody rigidbody;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            GetComponent<Collider>().isTrigger = true;
        }


        //private void Update()
        //{
        //    if(transform.position.y < 10f)
        //        Destroy(this.gameObject);

        //    if (this.transform.parent == null)
        //    {
        //        GetComponent<Collider>().isTrigger = false;
        //        GetComponent<Rigidbody>().isKinematic = false;
        //        GetComponent<Rigidbody>().useGravity = true;
        //    }
        //}

        private void FixedUpdate()
        {
            rigidbody.AddForce(Vector3.up * acceleration);
            //rigidbody.velocity += Vector3.up * acceleration * Time.fixedDeltaTime;
        }

    //    private void OnTriggerExit(Collider other)
    //    {
    //        //GetComponent<Collider>().isTrigger = false;
    //    }

        

    //    private void OnCollisionEnter(Collision other)
    //    {
    //        // Debug.Log("Bomb Hit: " + other.gameObject.tag);
    //        var otherObject = other.gameObject;

    //        if (otherObject.tag == "Building")
    //        {
    //            Debug.Log("Bomb Hit: " + otherObject.tag);
    //            if(otherObject.GetComponent<Renderer>() != null)
    //            {
    //                otherObject.GetComponent<Renderer>().material.color = Color.red;
    //            }
    //            Destroy(this.gameObject);
    //        }
    //    }

    }
}