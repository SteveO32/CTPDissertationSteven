using UnityEngine;
using System.Collections.Generic;

// Kojima Party - Team Hairy Devs 2018
// Author: Curtis Wiseman
// Purpose: Spline creation for navigation system
// Namespace: Hairy Devs
// Script Created: 20/02/2018 16:00

namespace HDev
{
    public class RoadMap : MonoBehaviour
    {
        public List<Transform> CreatedObjects;
        private Bird.BezierSpline spline;
        [SerializeField] private int frequency;
        [SerializeField] private bool lookForward;
        public Transform[] items;

        public void SetupPoints()
        {
            spline = GetComponent<Bird.BezierSpline>();
            CreatedObjects = new List<Transform>();

            if (frequency <= 0 || items == null || items.Length == 0)
            {
                return;
            }

            float stepSize = frequency * items.Length;

            if (spline.Loop || stepSize == 1)
            {
                stepSize = 1f / stepSize;
            }
            else
            {
                stepSize = 1f / (stepSize - 1);
            }

            for (int p = 0, f = 0; f < frequency; f++)
            {
                for (int i = 0; i < items.Length; i++, p++)
                {
                    Transform item = Instantiate(items[i]) as Transform;
                    Vector3 position = spline.GetPoint(p * stepSize);
                    item.transform.localPosition = position;

                    if (lookForward)
                    {
                        item.transform.LookAt(position + spline.GetDirection(p * stepSize));
                    }

                    item.transform.parent = transform;
                    CreatedObjects.Add(item);
                }
            }
        }
    }
}