using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    public class ContainmentSystemVisual : MonoBehaviour
    {

        public LineRenderer original;

        public List<LineRenderer> lrs;

        public bool render;

        float startTime;

        [SerializeField]
        float speed;

        [SerializeField]
        float frequency;

        // Use this for initialization
        void Start()
        {
            startTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time - startTime > frequency)
            {
                startTime = Time.time;
                var newLine = new GameObject();
                newLine.transform.parent = this.transform;
                var lr = newLine.AddComponent<LineRenderer>();
                Vector3[] positions = new Vector3[original.positionCount];
                original.GetPositions(positions);
                lr.positionCount = original.positionCount;
                lr.SetPositions(positions);
                lr.material = original.material;
                lr.colorGradient = original.colorGradient;
                lrs.Add(lr);
                Destroy(newLine, 20.0f);
            }

            lrs.RemoveAll(l => l == null);

            foreach (var line in lrs)
            {
                Vector3[] positions = new Vector3[original.positionCount];
                line.GetPositions(positions);
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] += Vector3.up * speed;
                }

                line.SetPositions(positions);
            }
        }
    }
}