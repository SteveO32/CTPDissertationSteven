using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//===================== Kojima Party - Team Frivolous 2018 ====================//
//
// Author:      Chloe Goument
// Purpose:     Handling death by collision.
// Namespace:   TF
//
//===============================================================================//

namespace TF
{
    public class CarHitRespawn : MonoBehaviour
    {
        // Debugging code for restarting a scene.
        void Update()
        {
            if (Input.GetKeyDown("1"))
            {
                SceneManager.LoadSceneAsync("EmptyOhiyaku");
                SceneManager.LoadScene("FroggerAdditive", LoadSceneMode.Additive);
            }
        }

        // Respawn if hit by a car
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "FroggerCar")
            {
                StartCoroutine(Respawn());
            }
        }

        // Execute the respawn
        // Currently only works once.
        IEnumerator Respawn()
        {
            yield return new WaitForSecondsRealtime(1);
            transform.position = GameObject.Find("Spawn 1").transform.position;
            GameObject.Find("CameraController").transform.position = GameObject.Find("StartPoint").transform.position;
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}