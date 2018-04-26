using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FroggerCam : MonoBehaviour {
	public float minSpeed = 0.0f;
	public float maxSpeed = 5.0f;
	private float speed;
	public float acceleration = 0.25f;
	public float decceleration = 0.5f;
	public Transform startPoint, endPoint;

	public bool playerInCollider;
	public int numOfPlayersInCollider = 0;

	//private Camera cam;
	// Use this for initialization
	void Start () {
		//cam = GetComponentInChildren<Camera>();
		speed = minSpeed;
		playerInCollider = false;

		Vector3 startPos = startPoint.transform.position;
		this.transform.position = new Vector3 (startPos.x, startPos.y, startPos.z);
		Quaternion startRot = startPoint.transform.rotation;
		this.transform.rotation = new Quaternion(startRot.x, startRot.y, startRot.z, startRot.w);
	}

	// Update is called once per frame
	void Update () {
		if (numOfPlayersInCollider > 0) {
			if (speed < maxSpeed) {
				speed += acceleration;
			}
		} else {
			if (speed > minSpeed)
				speed -= decceleration;
		
			if (speed < minSpeed)
				speed = minSpeed;
		}
		float step = speed * Time.deltaTime;
		this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint.position, step);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			numOfPlayersInCollider += 1;
			playerInCollider = true;
			//Debug.Log ("Player Entered Collider");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player") 
		{
			numOfPlayersInCollider -= 1;
			playerInCollider = false;
			//Debug.Log ("Player Exited Collider");
		}
	}



}
