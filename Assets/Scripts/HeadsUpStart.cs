using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class used to hold more complex ayout of stages of play
//in function order to be called by game manager
public class HeadsUpStart : MonoBehaviour {
    //to get access to other classes
    GameObject HUD;
    GameObject EvalHand;
    GameObject GM;
    GameObject P;
    GameObject E;

    // Use this for initialization
    void Start () {
        //initialise references to other classes
        HUD = GameObject.FindGameObjectWithTag("HeadsUp");
        P = GameObject.FindGameObjectWithTag("PlayerEval");
        E = GameObject.FindGameObjectWithTag("EnemyEval");
        GM = GameObject.FindGameObjectWithTag("GameManager");
    }
	//function to handle the first stage of play with ordered function calls
	public void ShuffleAndDealHoleCards ()
    {
        HUD.GetComponent<HeadsUpDeal>().ShuffleDeck();
        HUD.GetComponent<HeadsUpDeal>().DealHoleCards();
        P.GetComponent<PlayerEvaluation>().GrabCards();
        P.GetComponent<PlayerEvaluation>().CheckForFlush(5);
        P.GetComponent<PlayerEvaluation>().CardMultiples(5);
        P.GetComponent<PlayerEvaluation>().CheckStraight(5);
        P.GetComponent<PlayerEvaluation>().StrongestHand();
        GM.GetComponent<GameManager>().DetermineWinningHand();
        P.GetComponent<PlayerEvaluation>().ClearCurrentHandEval();
        E.GetComponent<EnemyEvaluation>().GrabCards();
        E.GetComponent<EnemyEvaluation>().CheckForFlush(5);
        E.GetComponent<EnemyEvaluation>().CardMultiples(5);
        E.GetComponent<EnemyEvaluation>().CheckStraight(5);
        E.GetComponent<EnemyEvaluation>().StrongestHand();
        GM.GetComponent<GameManager>().DetermineWinningHand();
        E.GetComponent<EnemyEvaluation>().ClearCurrentHandEval();
    }
    //function to handle the second stage of play with ordered function calls
    public void DealFlop()
    {
        HUD.GetComponent<HeadsUpDeal>().DealFlop();
        P.GetComponent<PlayerEvaluation>().GrabCards();
        P.GetComponent<PlayerEvaluation>().CheckForFlush(2);
        P.GetComponent<PlayerEvaluation>().CardMultiples(2);
        P.GetComponent<PlayerEvaluation>().CheckStraight(2);
        P.GetComponent<PlayerEvaluation>().StrongestHand();
        GM.GetComponent<GameManager>().DetermineWinningHand();
        P.GetComponent<PlayerEvaluation>().ClearCurrentHandEval();
        E.GetComponent<EnemyEvaluation>().GrabCards();
        E.GetComponent<EnemyEvaluation>().CheckForFlush(2);
        E.GetComponent<EnemyEvaluation>().CardMultiples(2);
        E.GetComponent<EnemyEvaluation>().CheckStraight(2);
        E.GetComponent<EnemyEvaluation>().StrongestHand();
        GM.GetComponent<GameManager>().DetermineWinningHand();
        E.GetComponent<EnemyEvaluation>().ClearCurrentHandEval();
    }
    //function to handle the third stage of play with ordered function calls
    public void DealTurn()
    {
        HUD.GetComponent<HeadsUpDeal>().DealTurn();
        P.GetComponent<PlayerEvaluation>().GrabCards();
        P.GetComponent<PlayerEvaluation>().CheckForFlush(1);
        P.GetComponent<PlayerEvaluation>().CardMultiples(1);
        P.GetComponent<PlayerEvaluation>().CheckStraight(1);
        P.GetComponent<PlayerEvaluation>().StrongestHand();
        GM.GetComponent<GameManager>().DetermineWinningHand();
        P.GetComponent<PlayerEvaluation>().ClearCurrentHandEval();
        E.GetComponent<EnemyEvaluation>().GrabCards();
        E.GetComponent<EnemyEvaluation>().CheckForFlush(1);
        E.GetComponent<EnemyEvaluation>().CardMultiples(1);
        E.GetComponent<EnemyEvaluation>().CheckStraight(1);
        E.GetComponent<EnemyEvaluation>().StrongestHand();
        GM.GetComponent<GameManager>().DetermineWinningHand();
        E.GetComponent<EnemyEvaluation>().ClearCurrentHandEval();
    }
    //function to handle the fourth stage of play with ordered function calls
    public void DealRiver()
    {
        HUD.GetComponent<HeadsUpDeal>().DealRiver();
        P.GetComponent<PlayerEvaluation>().GrabCards();
        P.GetComponent<PlayerEvaluation>().CheckForFlush(0);
        P.GetComponent<PlayerEvaluation>().CardMultiples(0);
        P.GetComponent<PlayerEvaluation>().CheckStraight(0);
        P.GetComponent<PlayerEvaluation>().StrongestHand();
        GM.GetComponent<GameManager>().DetermineWinningHand();
        P.GetComponent<PlayerEvaluation>().ClearCurrentHandEval();
        E.GetComponent<EnemyEvaluation>().GrabCards();
        E.GetComponent<EnemyEvaluation>().CheckForFlush(0);
        E.GetComponent<EnemyEvaluation>().CardMultiples(0);
        E.GetComponent<EnemyEvaluation>().CheckStraight(0);
        E.GetComponent<EnemyEvaluation>().StrongestHand();
        GM.GetComponent<GameManager>().DetermineWinningHand();
        E.GetComponent<EnemyEvaluation>().ClearCurrentHandEval();
        GM.GetComponent<GameManager>().roundChipShare();
    }
    //function to destroy all cards in scene and clear evaluations of hands 
    //to reset gameobjects for next hand
    public void DestroyCards()
    {
        HUD.GetComponent<HeadsUpDeal>().DestroyCards();
        GM.GetComponent<GameManager>().DestroyTheCards.interactable = false;
        P.GetComponent<PlayerEvaluation>().ClearCurrentHandEval();
        E.GetComponent<EnemyEvaluation>().ClearCurrentHandEval();
        GM.GetComponent<GameManager>().ClearHands();
    }
    //reset game states
    public void ResetTable()
    {
        GM.GetComponent<GameManager>().GS = GameStates.BeforeHand;
        GM.GetComponent<GameManager>().PS = PlayerStates.Nothing;
        GM.GetComponent<GameManager>().ES = EnemyStates.Nothing;
    }
}
