using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriving : MonoBehaviour {

    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;
    public float maxSteerAngle = 45.0f;
    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelBL;
    public WheelCollider WheelBR;

    // Use this for initialization
    void Start () {
        Transform[] pathwayTransform = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathwayTransform.Length; i++)
        {
            if (pathwayTransform[i] != path.transform)
            {
                nodes.Add(pathwayTransform[i]);
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        ApplySteer();
        Drive();
        CheckWaypointDistance();
	}

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        WheelFL.steerAngle = newSteer * (Time.deltaTime * 22);
        WheelFR.steerAngle = newSteer * (Time.deltaTime * 22);
    }

    private void Drive()
    {
        WheelBL.motorTorque = 225f;
        WheelBR.motorTorque = 225f;
        WheelFL.motorTorque = 50f;
        WheelFR.motorTorque = 50f;
    }

    private void CheckWaypointDistance()
    {
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 5.0f)
        {
            if(currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
            }
        }
    }
}
