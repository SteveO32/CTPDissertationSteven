using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FH
{
    public class AircraftBombHatch : MonoBehaviour
    {
        [SerializeField]
        private GameObject bombPrefab;
        [SerializeField]
        private float HATCH_LOCK_TIME = 0.5f;
        [SerializeField]
        private float timer = 0f;
        [SerializeField]
        private bool hatchLocked = false;
        [SerializeField]
        private int defaultStock = 8;
        [SerializeField]
        private int bombStock = 0;

        [SerializeField]
        private Camera hatchCam;
        [SerializeField]
        private bool active = false;
        //[SerializeField]
        //private float cameraX = 0f;
        //[SerializeField]
        //private float cameraZ = 7.5f;
        [SerializeField]
        private float hatchCamShakeAmount = 2.5f;
        [SerializeField]
        private float speedMultiplier = 0f;
        [SerializeField]
        private float hatchCamRotationTightness = 5f;



        private void Start()
        {
            bombStock = defaultStock;
            hatchCam = GetComponentInChildren<Camera>();
            Instantiate(bombPrefab, transform.position + new Vector3(4.18f, -6.62f, 0.6f), Quaternion.identity, this.gameObject.transform);
            if (!hatchCam)
            {
                Debug.Log("ERROR: HatchCam component not found in child object.");
            }
            active = false;
        }


        private void Update()
        {
            if(!hatchCam) return;

            if(hatchLocked)
            {
                if(timer <= HATCH_LOCK_TIME)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    if (bombStock > 0)
                    {
                        Instantiate(bombPrefab, transform.position + new Vector3(4.18f, -6.62f, 0.6f), Quaternion.identity, this.gameObject.transform);
                    }
                    hatchLocked = false;
                    timer = 0f;
                }
            }

                hatchCam.enabled = active;
            var hatchCamShake = hatchCamShakeAmount * (speedMultiplier * 0.1f * Time.deltaTime);

            var direction = hatchCam.transform.forward;
            var newRotation = Quaternion.LookRotation(direction + new Vector3(
                Random.Range(-hatchCamShake, hatchCamShake),
                Random.Range(-hatchCamShake, hatchCamShake),
                Random.Range(-hatchCamShake, hatchCamShake)),
                hatchCam.transform.up);

            hatchCam.transform.rotation = Quaternion.Slerp(hatchCam.transform.rotation, newRotation, Time.deltaTime * hatchCamRotationTightness);
        }

        public void UpdateMultiplier(float speedMultiplier)
        {
            this.speedMultiplier = speedMultiplier;
        }

        public void DropBomb()
        {
            if(!hatchLocked && bombStock > 0)
            {
                /*var bomb = *///Instantiate(bombPrefab, transform.position + (transform.forward * 25f), Quaternion.identity, this.gameObject.transform);
                var child = transform.Find("Bomb(Clone)");
                child.transform.parent = null;

                hatchLocked = true;
                bombStock--;
            }
        }

        public bool Alert()
        {
            if(bombStock <= 3)
                return true;
            return false;
        }

        /// <summary>
        /// Toggle the hatch camera on/ off.
        /// </summary>
        /// <returns></returns>
        public bool ToggleHatchCam(float value)
        {
            if(value > 0)
                active = !active;
            return active;
        }


        public void Resupply()
        {
            bombStock = defaultStock;
        }
    }
}
