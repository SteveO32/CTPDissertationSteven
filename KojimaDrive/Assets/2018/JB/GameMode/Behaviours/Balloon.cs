using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

    public class Balloon : MonoBehaviour
    {
		private float min;
		private float max;

		private float timer = 0.0f;
		private float timer_threshold = 1.0f;

		private Vector3 original_pos;
		private Vector3 end_pos;

        // Use this for initialization
        void Start()
        {
			original_pos = transform.position;

			max = 2.0f;

			end_pos = original_pos;
			end_pos.y = end_pos.y + max;
        }

        // Update is called once per frame
        void Update()
        {
			timer += Time.deltaTime;

			if (timer > timer_threshold) 
			{
				float temp = end_pos.y;
				end_pos.y = original_pos.y;
				original_pos.y = temp;
				timer = 0.0f;
			}
		
			float t = timer / timer_threshold;
			t = t*t * (3f - 2f*t);

			float perc = timer / timer_threshold;
			transform.position = Vector3.Lerp (original_pos, end_pos, t);
        }
    }
}
