using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathCar : MonoBehaviour {

    public Color pathwayColour;

    private List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmos()
    {
        Gizmos.color = pathwayColour;

        Transform[] pathwayTransform = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for(int i = 0; i < pathwayTransform.Length; i++)
        {
            if(pathwayTransform[i] != transform)
            {
                nodes.Add(pathwayTransform[i]);
            }
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 prevNode = Vector3.zero;

            if (i > 0)
            {
                prevNode = nodes[i - 1].position;
            } else if (i == 0 && nodes.Count > 1)
            {
                prevNode = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawLine(prevNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.7f);
        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
