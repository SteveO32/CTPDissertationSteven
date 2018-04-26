using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//class with functions to handle the players in the games stacks
public class PlayerChipStack : MonoBehaviour {
    //to access other class
    GameObject GM;
    Text myStack;

	// Use this for initialization
	void Start () {
        //initialise references to other class
        GM = GameObject.FindGameObjectWithTag("GameManager");
        myStack = GetComponent<Text>();
        //assign the text component to the right gameobject by name
        if(this.gameObject.name == "ChipStack")
        {
            myStack.text = "Chips: " + GM.GetComponent<GameManager>().playersStack.ToString();
        }
        if(this.gameObject.name == "EnemyStack")
        {
            myStack.text = "Chips: " + GM.GetComponent<GameManager>().enemysStack.ToString();
        }
    }
    //update the text on the chip stack fields in the UI
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

}
