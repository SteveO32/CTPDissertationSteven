using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    public class ColliderRepulsion : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            var hcc = collision.other.GetComponentInParent<HumanCharacterControl>();
            if (hcc != null)
            {
                hcc.transform.position += -collision.contacts[0].normal;
            }
        }
    }
}
