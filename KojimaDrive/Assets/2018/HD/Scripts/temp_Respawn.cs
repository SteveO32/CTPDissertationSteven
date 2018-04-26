using UnityEngine;

// Kojima Party - Team Hairy Devs 2018
// Author: Piotr Lubinski
// Purpose: Teleporting player to the play-area if he falls into the water
// Namespace: HDev
// Script Created: 27/02/2018 09:00
// Last Edited by Curtis Wiseman 13/03/18 13:07

namespace HDev
{
    public class Temp_Respawn : MonoBehaviour
    {
        float y = 5.0f;

        void Start()
        {

        }

        void Update()
        {
            if (transform.position.y < -2.0f)
            {
                GetComponent<Rigidbody>().Sleep();
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                transform.rotation = new Quaternion(0, 0, 0, 1);
                Vector3 restart = new Vector3(Random.Range(-30.0f, 30.0f), y, Random.Range(-30.0f, 30.0f));
                //restart.y += 3f;
                transform.position = restart;
            }
        }
    }
}