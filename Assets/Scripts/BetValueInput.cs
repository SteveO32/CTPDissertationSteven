using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//class to handle bets inputed in fields
public class BetValueInput : MonoBehaviour {
    //used to access other classes
    GameObject GM;
    InputField input;
    GameObject chipCount;

	// Use this for initialization
	void Start () {
        //initialise reference
		GM = GameObject.FindGameObjectWithTag("GameManager");
        //assign correct input field to correct ui element
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
    //function to ensure bets are valid
    public void CheckBetValue(string value)
    {
        //parse the inputted string into an int
        int bet = int.Parse(value);
        input.text = "";
        if (this.gameObject.name == "InputField" && GM.GetComponent<GameManager>().GS != GameStates.EndHand)
        {
            //check to make sure bet is less than stack and equal or higher than last bet
            if (bet <= GM.GetComponent<GameManager>().playersStack && bet > 0 && bet >= GM.GetComponent<GameManager>().enemysLastBet)
            {
                GM.GetComponent<GameManager>().playersStack -= bet;
                chipCount.GetComponent<PlayerChipStack>().UpdateChipCountText();
                GM.GetComponent<GameManager>().pot += bet;
                GM.GetComponent<GameManager>().playersTurnTotalBet += bet;

                if(GM.GetComponent<GameManager>().playersTurnTotalBet == GM.GetComponent<GameManager>().enemysTurnTotalBet)
                {
                    GM.GetComponent<GameManager>().playersLastBet = 0;
                    GM.GetComponent<GameManager>().enemysLastBet = 0;
                    GM.GetComponent<GameManager>().playersTurnTotalBet = 0;
                    GM.GetComponent<GameManager>().enemysTurnTotalBet = 0;
                    Debug.Log("this");
                    //called, both put in even money
                    GM.GetComponent<GameManager>().PS = PlayerStates.Bet;
                    GM.GetComponent<GameManager>().ES = EnemyStates.Bet;
                }

                GM.GetComponent<GameManager>().playersLastBet = bet;
                GM.GetComponent<GameManager>().enemysLastBet = 0;
            }
        }
        if (this.gameObject.name == "EnemyInputField" && GM.GetComponent<GameManager>().GS != GameStates.EndHand)
        {
            if (bet <= GM.GetComponent<GameManager>().enemysStack && bet > 0 && bet >= GM.GetComponent<GameManager>().playersLastBet)
            {
                GM.GetComponent<GameManager>().enemysStack -= bet;
                chipCount.GetComponent<PlayerChipStack>().UpdateChipCountText();
                GM.GetComponent<GameManager>().pot += bet;
                GM.GetComponent<GameManager>().enemysTurnTotalBet += bet;

                if (GM.GetComponent<GameManager>().enemysTurnTotalBet == GM.GetComponent<GameManager>().playersTurnTotalBet)
                {
                    GM.GetComponent<GameManager>().playersLastBet = 0;
                    GM.GetComponent<GameManager>().enemysLastBet = 0;
                    GM.GetComponent<GameManager>().playersTurnTotalBet = 0;
                    GM.GetComponent<GameManager>().enemysTurnTotalBet = 0;
                    //called, both put in even money
                    GM.GetComponent<GameManager>().PS = PlayerStates.Bet;
                    GM.GetComponent<GameManager>().ES = EnemyStates.Bet;
                }

                GM.GetComponent<GameManager>().enemysLastBet = bet;
                GM.GetComponent<GameManager>().playersLastBet = 0;
            }
        }
    }
    
}
