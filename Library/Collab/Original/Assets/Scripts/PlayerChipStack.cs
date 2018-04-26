using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChipStack : MonoBehaviour {

    GameObject GM;
    Text myStack;

	// Use this for initialization
	void Start () {
        GM = GameObject.FindGameObjectWithTag("GameManager");
        myStack = GetComponent<Text>();

        if(this.gameObject.name == "ChipStack")
        {
            myStack.text = "Chips: " + GM.GetComponent<GameManager>().playersStack.ToString();
        }
        if(this.gameObject.name == "EnemyStack")
        {
            myStack.text = "Chips: " + GM.GetComponent<GameManager>().enemysStack.ToString();
        }
    }

    public void UpdateChipCountText()
    {
        if (this.gameObject.name == "ChipStack")
        {
            myStack.text = "Chips: " + GM.GetComponent<GameManager>().playersStack.ToString();
        }
        if (this.gameObject.name == "EnemyStack")
        {
            myStack.text = "Chips: " + GM.GetComponent<GameManager>().enemysStack.ToString();
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
