using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FH
{
    public class Water : MonoBehaviour
    {
        private const float MAX_TIME = 15f;
        private float timer = 0f;




        private void Update()
        {
            if(timer < MAX_TIME)
            {
                timer += Time.deltaTime;
            }
            else
            {
                Destroy(this.gameObject);
            }


            GetComponent<Rigidbody>().velocity -= Vector3.up * 20f * Time.deltaTime;
        }



        private void OnCollisionEnter(Collision other)
        {
            var otherObject = other.gameObject;
            if(otherObject.tag == "Building")
            {
                if(otherObject.GetComponent<Renderer>().material.color == Color.red)
                {
                    otherObject.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }
    }
}