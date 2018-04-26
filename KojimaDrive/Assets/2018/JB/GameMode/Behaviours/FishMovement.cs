using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

    public class FishMovement : MonoBehaviour
    {

        [SerializeField] float movement_speed = 10.0f;
        [SerializeField] float rotation_speed = 1.5f;

        [SerializeField] float jump_height = 1.0f;

        [SerializeField] bool can_jump = false;

        private bool jumping = false;

        private Vector3 target;

        private Vector3 jump_start;
        private Vector3 jump_end;

        private GameObject boat;
        

        private float jump_timer = 0.0f;
        private float jump_threshold = 1.0f;

        private float jump_progress = 0.0f;

        // Use this for initialization
        void Start()
        {
            NewPos();

            boat = JBSceneRefs.boat.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if ((can_jump) && (!jumping))
            {
                jump_timer += Time.deltaTime;
            }

            if (jump_timer > jump_threshold)
            {
                if ((Vector3.Angle(transform.forward, boat.transform.forward) < 20.0f) ||
                    (Vector3.Angle(transform.forward, -boat.transform.forward) < 20.0f))
                {
                    target = transform.position + (transform.forward * 30);

                    GetComponent<Rigidbody>().AddForce(transform.forward * jump_height, ForceMode.VelocityChange);
                    GetComponent<Rigidbody>().AddForce(transform.up * jump_height * 2, ForceMode.VelocityChange);

                    jump_timer = 0.0f;

                    jumping = true;

                    jump_start = transform.position;
                }
            }

            if (jumping)
            {
                jump_progress += Time.deltaTime;

                if (jump_progress > 0.5f)
                {
                    if (Mathf.Abs(transform.position.y - jump_start.y) < 0.2f)
                    {
                        jumping = false;
                        jump_progress = 0.0f;
                        NewPos();
                    }
                }
            }

            else
            {
                if (Vector3.Distance(transform.position, target) > 2.0f)
                {
                    float move_step = movement_speed * Time.deltaTime;                    

                    transform.Translate((Vector3.forward * Time.deltaTime) * movement_speed);                   
                }



                else
                {
                    NewPos();
                }
            }

            float rot_step = rotation_speed * Time.deltaTime;
            Vector3 look_rot = target - transform.position;
            Vector3 new_rot = Vector3.RotateTowards(transform.forward, look_rot, rot_step, 0.0f);
            transform.rotation = Quaternion.LookRotation(new_rot);


        }

        void NewPos()
        {
            float rand_offsetX = Random.Range(-10.0f, 10.0f);
            float rand_offsetZ = Random.Range(-10.0f, 10.0f);

            target = transform.position;

            target.x = target.x + rand_offsetX;
            target.z = target.z + rand_offsetZ;
        }
    } 
}
