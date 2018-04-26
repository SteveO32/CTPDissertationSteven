using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {
    //class to handle the player states
    public PlayerStates PS;
    public EnemyStates ES;

	// Use this for initialization
	void Start () {
        PS = PlayerStates.Nothing;
        ES = EnemyStates.Nothing;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void FoldHand()
    {
        PS = PlayerStates.Fold;
        ES = EnemyStates.Fold;
    }
}
