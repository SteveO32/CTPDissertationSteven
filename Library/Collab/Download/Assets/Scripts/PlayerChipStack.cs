using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChipStack : MonoBehaviour {

    GameObject GM;
    Text myStack;
    public int lastBet = 0;
    public int potTotal = 0;


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
        if (this.gameObject.name == "Pot Odds")
        {
            myStack.text = "Pot Odds are: " + lastBet + " / " + potTotal;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
