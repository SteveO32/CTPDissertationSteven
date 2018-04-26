using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GullMovement : MonoBehaviour {

    private int minX = -75;
    private int maxX = 75;
    private int minY = 0;
    private int maxY = 0;
    private int minZ = -75;
    private int maxZ = 75;

    public Vector3 acc;
    private Vector3 vel;
    private Vector3 prevAcc;
    float speed = 10.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, acc, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        BoundingBox();
        if (prevAcc != acc)
        {
            vel += acc * Time.deltaTime;
            prevAcc = acc;
            transform.position += vel;
        }
        vel = Vector3.zero;
        acc = Vector3.zero;
    }
    void BoundingBox()
    {
        if (this.transform.position.z >= maxZ)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, minZ);
            acc = new Vector3(-acc.x, -acc.y, -acc.z);
        }
        else if (this.transform.position.z <= minZ)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, maxZ);
            //acc = Vector3.zero;
            acc = new Vector3(-acc.x, -acc.y, -acc.z);
        }


        if (this.transform.position.x >= maxX)
        {
            this.transform.position = new Vector3(minX, this.transform.position.y, this.transform.position.z);
            // acc = Vector3.zero;
            acc = new Vector3(-acc.x, -acc.y, -acc.z);
        }
        else if (this.transform.position.x <= minX)
        {
            this.transform.position = new Vector3(maxX, this.transform.position.y, this.transform.position.z);
            //  acc = Vector3.zero;
            acc = new Vector3(-acc.x, -acc.y, -acc.z);
        }

        if (this.transform.position.y >= maxY)
        {
            this.transform.position = new Vector3(this.transform.position.x, minY, this.transform.position.z);
            // acc = Vector3.zero;
            acc = new Vector3(-acc.x, -acc.y, -acc.z);
        }
        else if (this.transform.position.y <= minY)
        {
            this.transform.position = new Vector3(this.transform.position.x, maxY, this.transform.position.z);
            //  acc = Vector3.zero;
            acc = new Vector3(-acc.x, -acc.y, -acc.z);
        }
    }
}
