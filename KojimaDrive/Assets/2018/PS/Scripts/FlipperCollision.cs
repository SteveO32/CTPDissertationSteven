using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperCollision : MonoBehaviour {

    PS.BumperControls controls;
    public int hitForce = 15;
	// Use this for initialization
	void Start () {
		controls = transform.parent.transform.parent.GetComponent<PS.BumperControls> ();
	}

	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerStay(Collider coll)
	{
        if (coll.CompareTag("Player"))
        {
            if (controls.checkBumperState(gameObject.name))
            {
                Vector3 direction = (coll.transform.position - transform.position) * hitForce;
                coll.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(direction, transform.position, ForceMode.VelocityChange);
                controls.setBumperStateOff(gameObject.name);
                //Debug.Log ("hello callan" + direction);

            }
        }


	}



}
