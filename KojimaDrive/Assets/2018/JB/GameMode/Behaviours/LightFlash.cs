using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

    public class LightFlash : MonoBehaviour
    {
        private bool flashing = true;
        private Light light;
        [SerializeField] float flash_speed = 30.0f;

        // Use this for initialization
        void Start()
        {
            light = GetComponent<Light>();
        }

        // Update is called once per frame
        void Update()
        {
            if (flashing)
            {
                light.intensity += (Time.deltaTime * flash_speed);

                if (light.intensity > 30)
                {
                    flashing = false;
                }
            }

            else
            {
                light.intensity -= (Time.deltaTime * flash_speed);

                if (light.intensity == 0)
                {
                    flashing = true;
                }
            }
        }
    }
}
