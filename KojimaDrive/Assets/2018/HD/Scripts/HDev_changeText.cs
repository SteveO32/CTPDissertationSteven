using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Kojima Party - Team Hairy Devs 2018
// Author: Elliott Joseph Phillips
// Purpose: temp change text of place untill one is added
// Namespace: Hairy Devs
// Script Created: 17/04/2018 16:03
// Last Edited by 

public class HDev_changeText : MonoBehaviour {

    Text text;

    [SerializeField]
    int place = 1;
    string stringPlace;
    // Use this for initialization
    void Start ()
    {
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (place == 1)
        {
            stringPlace = "1st";
        }
        else if (place == 2)
        {
            stringPlace = "2nd";
        }
        else if (place == 3)
        {
            stringPlace = "3rd";
        }
        else if (place == 4)
        {
            stringPlace = "4th";
        }
        text.text = stringPlace;
    }

    public void SetPlace(int newPlace)
    {
        place = newPlace;
    }
}
