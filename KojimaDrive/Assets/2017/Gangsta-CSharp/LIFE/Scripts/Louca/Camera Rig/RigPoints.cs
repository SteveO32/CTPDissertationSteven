using UnityEngine;
using System.Collections;

namespace GCSharp
{
    public class RigPoints : MonoBehaviour
    {

        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float rotSpeed;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        public float GetRotSpeed()
        {
            return rotSpeed;
        }
    }
}