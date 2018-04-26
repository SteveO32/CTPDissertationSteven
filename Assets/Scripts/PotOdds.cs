using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//class for calculating pot odds to display to player
public class PotOdds : MonoBehaviour {

    GameObject GM;
    Text potOddText;
    Text potTotalText;
    
    [SerializeField]
    private float oddCalculated = 0.0f;

	// Use this for initialization
	void Start () {
        //initialise reference to game manager
        GM = GameObject.FindGameObjectWithTag("GameManager");
        //assign text to text field for pot odds
        potOddText = GetComponent<Text>();
        if(gameObject.name == "Pot Total")
        {
            potTotalText = GetComponent<Text>();
        }
	}
    //function to calculate pot odds
    //works by knowing how much is in the pot and dividing that by
    //how much the player needs to bet to call
    //gives it as a ratio on screen
    public void CalculatePotOdds()
    {
        if (GM.GetComponent<GameManager>().pot != 0 && GM.GetComponent<GameManager>().enemysLastBet != 0)
        {
            oddCalculated = GM.GetComponent<GameManager>().pot / GM.GetComponent<GameManager>().enemysLastBet;
            System.Math.Round(oddCalculated, 3);
            potOddText.text = "Pot Odds: " + oddCalculated.ToString() + ": 1";
        }
        
    }


	// Update is called once per frame
    //update the calculation
	void Update ()
    {
        CalculatePotOdds();

        if(gameObject.name == "Pot Total")
        {
            if (GM.GetComponent<GameManager>().pot > 0)
            {
                potTotalText.text = "Pot Total: " + GM.GetComponent<GameManager>().pot.ToString();

            }
            else if (GM.GetComponent<GameManager>().pot == 0)
            {
                potTotalText.text = "Pot Total: 0";
            }
        }
        
    }
}
