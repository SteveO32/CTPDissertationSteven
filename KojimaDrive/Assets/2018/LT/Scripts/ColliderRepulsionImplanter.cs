using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    public class ColliderRepulsionImplanter : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            foreach (var coll in GetComponentsInChildren<Collider>())
            {
                coll.gameObject.AddComponent<ColliderRepulsion>();
            }
        }
    }
}