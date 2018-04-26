using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTest : MonoBehaviour {

    public GameObject game_object;
    Quaternion initialRotation;
    float distance = 0.0f;

    // Use this for initialization
    void Start () {
        initialRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.eulerAngles.x, this.transform.localRotation.eulerAngles.y, this.transform.localRotation.eulerAngles.z));
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 locations = this.transform.position - game_object.transform.position;
        distance = locations.magnitude;
        //Debug.Log(distance + "km");
        this.transform.LookAt(game_object.transform);
        this.transform.localRotation = Quaternion.Euler(new Vector3(initialRotation.x, this.transform.localRotation.eulerAngles.y, initialRotation.z));
    }
}
