using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FH
{
    public class Runway : MonoBehaviour
    {
        public delegate void AircraftResupplied();
        public static event AircraftResupplied aircraftResupplied;


        [SerializeField]
        private Renderer[] signalLightRenderers;
        [SerializeField]
        private bool recharging = false;
        [SerializeField]
        private bool resupplyConfirmed = false;
        [SerializeField]
        private float MAX_TIME = 5f;
        [SerializeField]
        private float timer = 0f;



        private void Start()
        {
            var signalLight = GameObject.FindGameObjectsWithTag("SignalLight");
            if(signalLight == null)
            {
                Debug.Log("ERROR: Signal light tag not found");
                return;
            }

            /* Debug signal towers.*/
            ///signalLightRenderers = new Renderer[2];
            ///for(int i = 0; i < signalLightRenderers.Length; i++)
            ///{
            ///    signalLightRenderers[i] = signalLight[i].GetComponent<Renderer>();
            ///    signalLightRenderers[i].material.color = Color.red;
            ///}
        }


        private void Update()
        {
            if(recharging && !resupplyConfirmed)
            {
                if(timer <= MAX_TIME)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    StartCoroutine(ColourSwitch());
                    if(aircraftResupplied != null)
                    {
                        aircraftResupplied();
                    }
                    else
                    {
                        Debug.Log("ERROR: Nothing has been attached to the aircraftResupplied delegate");
                    }
                    resupplyConfirmed = true;
                }
            }
            else
            {
                timer = 0f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("MESSAGE: Resupplying tag - " + other.tag);
            var otherObject = other.gameObject;
            if(otherObject.tag == "Aircraft")
            {

                recharging = true;
            }
        }



        private void OnTriggerExit(Collider other)
        {
            var otherObject = other.gameObject;
            if(otherObject.tag == "Aircraft")
            {
                recharging = false;
            }
        }


        /// <summary>
        /// Change the colours of the lights to green. When the lets turn off the plane can come back for a resupply.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ColourSwitch()
        {
            float MAX_TIME = 10f;
            float timer = 0f;
            while(timer < MAX_TIME)
            {
                for(int i = 0; i < signalLightRenderers.Length; i++)
                {
                    signalLightRenderers[i].material.color = Color.green;
                }
                timer += Time.deltaTime;
                yield return false;
            }
            for(int i = 0; i < signalLightRenderers.Length; i++)
            {
                signalLightRenderers[i].material.color = Color.red;
                resupplyConfirmed = false;
            }
            yield return true;
        }
    }
}