using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsUpStart : MonoBehaviour {

    GameObject HUD;
    GameObject EvalHand;
    GameObject GM;
    string Opponent = "Enemy";
    string User = "Player";

    // Use this for initialization
    void Start () {
        HUD = GameObject.FindGameObjectWithTag("HeadsUp");
        EvalHand = GameObject.FindGameObjectWithTag("HandEval");
        GM = GameObject.FindGameObjectWithTag("GameManager");
    }
	
	public void ShuffleAndDealHoleCards ()
    {
        HUD.GetComponent<HeadsUpDeal>().ShuffleDeck();
        HUD.GetComponent<HeadsUpDeal>().DealHoleCards();
        EvalHand.GetComponent<EvaluatingHand>().GrabCards(Opponent);
        EvalHand.GetComponent<EvaluatingHand>().CheckForFlush(5);
        EvalHand.GetComponent<EvaluatingHand>().CardMultiples(5);
        EvalHand.GetComponent<EvaluatingHand>().CheckStraight(5);
        EvalHand.GetComponent<EvaluatingHand>().StrongestHand(Opponent);
        EvalHand.GetComponent<EvaluatingHand>().ClearCurrentHandEval();
        EvalHand.GetComponent<EvaluatingHand>().GrabCards(User);
        EvalHand.GetComponent<EvaluatingHand>().CheckForFlush(5);
        EvalHand.GetComponent<EvaluatingHand>().CardMultiples(5);
        EvalHand.GetComponent<EvaluatingHand>().CheckStraight(5);
        EvalHand.GetComponent<EvaluatingHand>().StrongestHand(User);
        EvalHand.GetComponent<EvaluatingHand>().ClearCurrentHandEval();
    }

    public void DealFlop()
    {
        HUD.GetComponent<HeadsUpDeal>().DealFlop();
        EvalHand.GetComponent<EvaluatingHand>().GrabCards(Opponent);
        EvalHand.GetComponent<EvaluatingHand>().CheckForFlush(2);
        EvalHand.GetComponent<EvaluatingHand>().CardMultiples(2);
        EvalHand.GetComponent<EvaluatingHand>().CheckStraight(2);
        EvalHand.GetComponent<EvaluatingHand>().StrongestHand(Opponent);
        EvalHand.GetComponent<EvaluatingHand>().ClearCurrentHandEval();
        EvalHand.GetComponent<EvaluatingHand>().GrabCards(User);
        EvalHand.GetComponent<EvaluatingHand>().CheckForFlush(2);
        EvalHand.GetComponent<EvaluatingHand>().CardMultiples(2);
        EvalHand.GetComponent<EvaluatingHand>().CheckStraight(2);
        EvalHand.GetComponent<EvaluatingHand>().StrongestHand(User);
        EvalHand.GetComponent<EvaluatingHand>().ClearCurrentHandEval();
    }

    public void DealTurn()
    {
        HUD.GetComponent<HeadsUpDeal>().DealTurn();
        EvalHand.GetComponent<EvaluatingHand>().GrabCards(Opponent);
        EvalHand.GetComponent<EvaluatingHand>().CheckForFlush(1);
        EvalHand.GetComponent<EvaluatingHand>().CardMultiples(1);
        EvalHand.GetComponent<EvaluatingHand>().CheckStraight(1);
        EvalHand.GetComponent<EvaluatingHand>().StrongestHand(Opponent);
        EvalHand.GetComponent<EvaluatingHand>().ClearCurrentHandEval();
        EvalHand.GetComponent<EvaluatingHand>().GrabCards(User);
        EvalHand.GetComponent<EvaluatingHand>().CheckForFlush(1);
        EvalHand.GetComponent<EvaluatingHand>().CardMultiples(1);
        EvalHand.GetComponent<EvaluatingHand>().CheckStraight(1);
        EvalHand.GetComponent<EvaluatingHand>().StrongestHand(User);
        EvalHand.GetComponent<EvaluatingHand>().ClearCurrentHandEval();
    }

    public void DealRiver()
    {
        HUD.GetComponent<HeadsUpDeal>().DealRiver();
        EvalHand.GetComponent<EvaluatingHand>().GrabCards(Opponent);
        EvalHand.GetComponent<EvaluatingHand>().CheckForFlush(0);
        EvalHand.GetComponent<EvaluatingHand>().CardMultiples(0);
        EvalHand.GetComponent<EvaluatingHand>().CheckStraight(0);
        EvalHand.GetComponent<EvaluatingHand>().StrongestHand(Opponent);
        EvalHand.GetComponent<EvaluatingHand>().ClearCurrentHandEval();
        EvalHand.GetComponent<EvaluatingHand>().GrabCards(User);
        EvalHand.GetComponent<EvaluatingHand>().CheckForFlush(0);
        EvalHand.GetComponent<EvaluatingHand>().CardMultiples(0);
        EvalHand.GetComponent<EvaluatingHand>().CheckStraight(0);
        EvalHand.GetComponent<EvaluatingHand>().StrongestHand(User);
        EvalHand.GetComponent<EvaluatingHand>().ClearCurrentHandEval();
        GM.GetComponent<GameManager>().roundChipShare();
    }

    public void DestroyCards()
    {
        HUD.GetComponent<HeadsUpDeal>().DestroyCards();
        EvalHand.GetComponent<EvaluatingHand>().ClearCurrentHandEval();
        GM.GetComponent<GameManager>().ClearHands();
    }
}
