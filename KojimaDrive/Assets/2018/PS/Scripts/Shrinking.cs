using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public class Shrinking : MonoBehaviour
    {

        public float scaleRate = 0.02f;
        PlayerPrefabManager playerManager;

        // Use this for initialization
        void Start()
        {
            playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerPrefabManager>();
        }

        // Update is called once per frame
        void Update()
        {

            if (transform.localScale.x > 0 || transform.localScale.z > 0)
            {
                transform.localScale -= new Vector3(scaleRate, 0, scaleRate);
            }


            if (transform.localScale.x <= 0 || transform.localScale.z <= 0)
            {
                //CYLINDER FINISHED SHRINKING
                //DESTROYGAMEOBJECT
                //Destroy(this.gameObject);
                //transform.localScale = new Vector3(20.0f, 0, 20.0f);
            }


        }

        void OnTriggerExit(Collider col)
        {
            Debug.Log(col);
            if(col.CompareTag("Player"))
            {
                col.gameObject.SetActive(false);
               
                //send message to the gameModeManager saying that the player has been eliminated
                //col.GetComponent<PlayerInfo>().isAlive = false;
                //LOSE SCREEN
            }
        }
    }
}
