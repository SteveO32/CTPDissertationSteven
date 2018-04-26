using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FH
{
    public class BombCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera bombCamera;
        [SerializeField]
        private bool active = false;
        //[SerializeField]
        //private GameObject lastBomb;
        [SerializeField]
        private float cameraX = 0f;
        [SerializeField]
        private float cameraZ = 7.5f;

        // This would get picked up in the Aircraft object and passed to this script depending on the active
        //     camera.
        //private void OnEnable()
        //{
        //    InputManager.InputDetected += HandleInput;
        //}
        //
        //private void OnDisable()
        //{
        //    InputManager.InputDetected -= HandleInput;
        //}


        private void Awake()
        {
            bombCamera = GetComponent<Camera>();
        }

        private void Update()
        {
            bombCamera.enabled = active;

            // Points at the last bomb dropped
            //if(lastBomb != null)
            //    transform.LookAt(lastBomb.transform);
            //else
            //    transform.LookAt(new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z));

            transform.LookAt(new Vector3(transform.position.x - cameraX, transform.position.y - 10f, transform.position.z - cameraZ));
        }


        public void UpdateTarget(GameObject target)
        {
            //lastBomb = target;
        }


        public bool Toggle()
        {
            active = !active;
            return active;
        }


        private void HandleInput(GameAction gameAction, float value, int ID)
        {
            //if(myID == ID)
            //{
            //    if(gameAction == GameAction.LeftStick)
            //    {
            //        // TODO: On leftstick move camera moves around slightly.
            //    }
            //}
        }

    }
}