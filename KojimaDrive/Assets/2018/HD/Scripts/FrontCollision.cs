using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HDev
{
    public class FrontCollision : MonoBehaviour
    {
        private HDev_Collision colScript;
        private float maxTime = 3.0f;
        private float currentTime = 0.0f;
        private bool timerOn = false;
        private bool timerDone = true;


        private void Start()
        {
            colScript = this.transform.parent.gameObject.GetComponent<HDev_Collision>();
        }

        private void Update()
        {
            if (!timerOn) return;
        
            currentTime += Time.deltaTime;

            if (currentTime < maxTime) return;

            currentTime = 0.0f;
            timerOn = false;
            timerDone = true;

            colScript.frontCollision = false;

            Debug.Log("Front Collision OFF");
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.tag == "Player" && other.gameObject != this.transform.parent.gameObject && !colScript.frontCollision && !timerOn) 
            {
                timerOn = true;
                timerDone = false;
                colScript.frontCollision = true;

                Debug.Log("Front Collision ON");
            }
        }
    }
}