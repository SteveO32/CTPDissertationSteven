using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeSetting : MonoBehaviour {
    public GameObject cylinder;
	// Use this for initialization
	void Start () {
        this.transform.localScale = new Vector3(cylinder.transform.localScale.x, this.transform.localScale.y, cylinder.transform.localScale.z);
        this.transform.localPosition = cylinder.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
