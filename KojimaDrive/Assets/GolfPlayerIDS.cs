using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfPlayerIDS : MonoBehaviour {

    private int player_id;

	// Use this for initialization
	void Start () {
        player_id = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetID(int id)
    {
        player_id = id;
    }

    public int GetID()
    {
        return player_id;
    }
}
