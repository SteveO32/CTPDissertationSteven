using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetValueInput : MonoBehaviour {

    GameObject GM;
    InputField input;
    GameObject chipCount;

	// Use this for initialization
	void Start () {
		GM = GameObject.FindGameObjectWithTag("GameManager");
        if(this.gameObject.name == "InputField")
        {
            input = GameObject.Find("InputField").GetComponent<InputField>();
            chipCount = GameObject.Find("ChipStack");
        }
        if(this.gameObject.name == "EnemyInputField")
        {
            input = GameObject.Find("EnemyInputField").GetComponent<InputField>();
            chipCount = GameObject.Find("EnemyStack");
        }
    }

    public void CheckBetValue(string value)
    {
        int bet = int.Parse(value);
        input.text = "";
        if (this.gameObject.name == "InputField")
        {
            if (bet <= GM.GetComponent<GameManager>().playersStack && bet > 0)
            {
                GM.GetComponent<GameManager>().playersStack -= bet;
                chipCount.GetComponent<PlayerChipStack>().UpdateChipCountText();
                GM.GetComponent<GameManager>().pot += bet;
            }
        }
        if (this.gameObject.name == "EnemyInputField")
        {
            if (bet <= GM.GetComponent<GameManager>().enemysStack && bet > 0)
            {
                GM.GetComponent<GameManager>().enemysStack -= bet;
                chipCount.GetComponent<PlayerChipStack>().UpdateChipCountText();
                GM.GetComponent<GameManager>().pot += bet;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
