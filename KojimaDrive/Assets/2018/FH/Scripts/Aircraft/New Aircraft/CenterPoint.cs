using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    public class CenterPoint : MonoBehaviour
    {
        [SerializeField]
        List<Transform> objects;

        // Update is called once per frame
        void Update()
        {
            int c = 0;
            Vector3 pos = Vector3.zero;
            foreach (var p in objects)
            {
                pos += p.position;
                c++;
            }

            if (c > 0)
            {
                pos /= c;
            }

            transform.position = pos;
        }
    }
}
