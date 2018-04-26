using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateChildrenViewports : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Camera[] childViewports = GetComponentsInChildren<Camera>();
        for (int i = 0; i < childViewports.Length; i++)
        {
            childViewports[i].rect = GetComponent<Camera>().rect;
        }
	}
}
