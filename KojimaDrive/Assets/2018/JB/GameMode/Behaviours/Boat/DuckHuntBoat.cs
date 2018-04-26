using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{
    public class DuckHuntBoat : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] float max_speed = 5;
        [SerializeField] float stopping_distance = 1;

        [Header("References")]
        [SerializeField] List<Transform> spawn_points;

        private float steer_speed
        {
            get { return max_speed * 0.3f; }
        }

        private BoatCircuitNode target_node;

        private bool isRotating = false;
        private float rotate_timestamp;

        private Vector3 forward;
        private Vector3 endDireVector3;
        private float _angleToNextNode;
        private float _turnSpeed = 3;
        private float _turnDuration;
        private float _tempSpeed;


        public List<Transform> GetSpawnPoints()
        {
            return spawn_points;
        }


        public void SetStart(BoatCircuitNode _node)
        {
            transform.position = _node.transform.position;
            transform.rotation = _node.transform.rotation;

            target_node = _node;
        }


        void Update()
        {
            TravelToNode();
            SlowRotate();
        }


        void TravelToNode()
        {
            if (target_node == null)
                return;

            float dist = Vector3.Distance(transform.position, target_node.transform.position);
            if (dist > stopping_distance)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    target_node.transform.position, max_speed * Time.deltaTime);

                dist = Vector3.Distance(transform.position, target_node.transform.position);
                if (dist <= stopping_distance)
                {
                    NextNode();
                }
            }
            else
            {
                NextNode();
            }
        }

        void SlowRotate()
        {
            if (isRotating)
            {
                float timeSinceStarted = Time.time - rotate_timestamp;
                float percentageComplete = timeSinceStarted / _turnDuration;
                transform.forward = Vector3.Slerp(forward, endDireVector3, percentageComplete);
                if (percentageComplete >= 1.0f)
                {
                    isRotating = false;
                    max_speed = _tempSpeed;
                }
            }
        }


        Vector3 DirToTargetNode()
        {
            return (target_node.transform.position - transform.position).normalized;
        }


        void NextNode()
        {
            target_node.BoatArrived(this);
            target_node = target_node.nextNode;

            if (target_node == null)
                return;

            rotate_timestamp = Time.time;
            isRotating = true;
            forward = transform.forward;
            endDireVector3 = DirToTargetNode();
            _angleToNextNode = Vector3.Angle(transform.forward, endDireVector3);
            _turnDuration = _angleToNextNode / _turnSpeed;
            _tempSpeed = max_speed;
            max_speed = JHelper.Remap(_angleToNextNode, 0, 90, _tempSpeed, 1);
        }

    }
} // namespace JB