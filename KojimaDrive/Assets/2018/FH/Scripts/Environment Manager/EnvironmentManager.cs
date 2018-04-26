using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FH
{
    public class EnvironmentManager : MonoBehaviour
    {
        [SerializeField]
        private float ambientIntensity = 100f;



        private void Update()
        {
            RenderSettings.ambientIntensity = ambientIntensity;
        }
    }
}