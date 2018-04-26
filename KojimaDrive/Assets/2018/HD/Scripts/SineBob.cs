using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Kojima Party - Hairy Devs 2018
 * Author: Owen Jackson
 * Purpose: produces a sine wave bobbing effect, used to animate the pickup zone lines
 * Namespace: HDev
 * Last Edited: Owen Jackson @ 20/02/2018
*/

namespace HDev
{
    public class SineBob : MonoBehaviour
    {
        [SerializeField]
        private float frequency = 5.0f; //how fast it bobs
        [SerializeField]
        private float magnitude = 0.8f; //how high/low it bobs
        [SerializeField]
        private float rotSpeed = 5f;    //how fast it rotates
        [SerializeField]
        private bool doesRotate = false;    //if it rotates at all
        public bool DoesRotate
        {
            get { return doesRotate; }
            set { doesRotate = value; }
        }

        private Vector3 axis;
        private Vector3 pos;
    
        // Use this for initialization
        void Start()
        {
            pos = transform.position;
            axis = transform.up;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
            if (DoesRotate)
            {
                transform.Rotate(axis * rotSpeed * Time.deltaTime);
            }
        }
    }
}