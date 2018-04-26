using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//class to handle UI button behaviours
public class ButtonHandles : MonoBehaviour {
    //variables to assign text
    Text checkText;
    Text betText;
    Text foldText;
    Text clearHandText;
    GameObject GM;

    // Use this for initialization
    void Start ()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager");
        //assign placeholder text to correct object
        switch (gameObject.name)
        {
            case "Check":
                {
                    checkText = GetComponentInChildren<Text>();
                    break;
                }
            case "Bet":
                {
                    betText = GetComponentInChildren<Text>();
                    break;
                }
            case "Fold":
                {
                    foldText = GetComponentInChildren<Text>();
                    break;
                }
            case "Destroy Cards":
                {
                    clearHandText = GetComponentInChildren<Text>();
                    break;
                }
        }
    }
    //functions to change state when button pressed
    public void PlayerCheck()
    {
        GM.GetComponent<GameManager>().PS = PlayerStates.Check;
        GM.GetComponent<GameManager>().ES = EnemyStates.Check;//////
        Debug.Log("Player Checked");
    }

    public void EnemyCheck()
    {
        GM.GetComponent<GameManager>().ES = EnemyStates.Check;
        Debug.Log("Player Checked");
    }

    public void PlayerFold()
    {
        GM.GetComponent<GameManager>().PS = PlayerStates.Fold;
    }

    public void EnemyFold()
    {
        GM.GetComponent<GameManager>().ES = EnemyStates.Fold;
    }

    public void StartDealClicked()
    {
        GM.GetComponent<GameManager>().initialDeal = true;
    }

    // Update is called once per frame
    void Update ()
    {
        //switch statement to have intended text on buttons
        //for each stage
        switch (GM.GetComponent<GameManager>().GS)
        {
            case GameStates.BeforeHand:
                {

                    break;
                }

            case GameStates.Preflop:
                {
                    if (gameObject.name == "Check")
                    {
                        checkText.text = "Check";
                    }
                    if (gameObject.name == "Bet")
                    {
                        betText.text = "Bet";
                    }
                    if (gameObject.name == "Fold")
                    {
                        foldText.text = "Fold";
                    }
                    break;
                }
            case GameStates.Flop:
                {
                    if (gameObject.name == "Check")
                    {
                        checkText.text = "Check";
                    }
                    if (gameObject.name == "Bet")
                    {
                        betText.text = "Bet";
                    }
                    if (gameObject.name == "Fold")
                    {
                        foldText.text = "Fold";
                    }
                    break;
                }
            case GameStates.Turn:
                {
                    if (gameObject.name == "Check")
                    {
                        checkText.text = "Check";
                    }
                    if (gameObject.name == "Bet")
                    {
                        betText.text = "Bet";
                    }
                    if (gameObject.name == "Fold")
                    {
                        foldText.text = "Fold";
                    }
                    break;
                }
            case GameStates.River:
                {
                    if (gameObject.name == "Check")
                    {
                        checkText.text = "Check";
                    }
                    if (gameObject.name == "Bet")
                    {
                        betText.text = "Bet";
                    }
                    if (gameObject.name == "Fold")
                    {
                        foldText.text = "Fold";
                    }
                    break;
                }
            case GameStates.EndHand:
                {
                    if(gameObject.name == "Destroy Cards")
                    {
                        clearHandText.text = "Clear Hand";
                    }
                    
                    break;
                }
            default:
                {
                    if (gameObject.name == "Check")
                    {
                        checkText.text = "Check";
                    }
                    if (gameObject.name == "Bet")
                    {
                        betText.text = "Bet";
                    }
                    if (gameObject.name == "Fold")
                    {
                        foldText.text = "Fold";
                    }
                    break;
                }
        }  
	}
}
