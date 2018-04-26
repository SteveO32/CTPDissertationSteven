using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Drive - FluffyHedgehog 2018 ====================//
//
// Author:		Viv 
// Purpose:		Visual display for the aircraft altimeter
// Namespace:	FH
//
//===============================================================================//

public class AircraftAltimeter : MonoBehaviour {

    public GameObject player;
    public Text altimeter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if(!player      ) return;
        if(!altimeter   ) return;

        float position_y = Mathf.Round(player.transform.position.y);
        altimeter.text = position_y.ToString() + " ft";
    }
}
