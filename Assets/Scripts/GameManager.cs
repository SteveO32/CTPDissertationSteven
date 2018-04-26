using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Game manager to control the stages of play and manage

public class GameManager : MonoBehaviour {
    //to access other classes functions 
    GameObject PCS;
    GameObject P;
    GameObject E;
    GameObject PA;
    GameObject HUS;
    //to access states
    public GameStates GS;
    public PlayerStates PS;
    public EnemyStates ES;
    //variables for logic
    public int playersStack;
    public int enemysStack;
    public string EnemyBestHandIs;
    public string PlayersBestHandIs;
    [SerializeField]
    public int pot;
    public string RoundsBestHand;
    public bool didPlayerWin = false;
    public bool didEnemyWin = false;
    public bool didYouDraw = false;
    public bool initialDeal = false;
    public int playersTurnTotalBet = 0;
    public int enemysTurnTotalBet = 0;
    public int playersLastBet = 0;
    public int enemysLastBet = 0;
    public Button DestroyTheCards;

    // Use this for initialization
    void Start ()
    {
        //Initialise references
        PCS = GameObject.FindGameObjectWithTag("ChipStack");
        P = GameObject.FindGameObjectWithTag("PlayerEval");
        E = GameObject.FindGameObjectWithTag("EnemyEval");
        PA = GameObject.FindGameObjectWithTag("PlayerActions");
        HUS = GameObject.FindGameObjectWithTag("HUSPF");
        GS = GameStates.BeforeHand;
        PS = PlayerStates.Nothing;
        ES = EnemyStates.Nothing;
    }

    public void ClearHands()
    {
        //clear variables used for between rounds
        EnemyBestHandIs = null;
        PlayersBestHandIs = null;
        RoundsBestHand = null;
        didPlayerWin = false;
        didEnemyWin = false;
        didYouDraw = false;
        pot = 0;
    }
    //Function to assign the winnings to each player at the end of a hand
    public void roundChipShare()
    {
        if( didYouDraw == false)
        {
            if (didEnemyWin == true)
            {
                Debug.Log(pot);
                Debug.Log(enemysStack);
                enemysStack += pot;
                PCS.GetComponent<PlayerChipStack>().UpdateChipCountText();
                pot = 0;
            }
            else if (didPlayerWin == true)
            {
                playersStack += pot;
                PCS.GetComponent<PlayerChipStack>().UpdateChipCountText();
                pot = 0;
            }
        }

        if (didYouDraw == true)
        {
            pot = pot / 2;
            enemysStack += pot;
            playersStack += pot;
            PCS.GetComponent<PlayerChipStack>().UpdateChipCountText();
            pot = 0;
        }
        
    }

    //function that checks the strength of hands gathered from evaluation classes
    public void DetermineWinningHand()
    {
        if (P.GetComponent<PlayerEvaluation>().StrengthOutput() > E.GetComponent<EnemyEvaluation>().StrengthOutput())
        {
            RoundsBestHand = PlayersBestHandIs;
            //Player wins
            didYouDraw = false;
            didPlayerWin = true;
        }
        if (P.GetComponent<PlayerEvaluation>().StrengthOutput() < E.GetComponent<EnemyEvaluation>().StrengthOutput())
        {
            RoundsBestHand = EnemyBestHandIs;
            //Enemy wins
            didYouDraw = false;
            didEnemyWin = true;
        }


        if (P.GetComponent<PlayerEvaluation>().StrengthOutput() == E.GetComponent<EnemyEvaluation>().StrengthOutput())
        {
            //Straight Flush
            if (P.GetComponent<PlayerEvaluation>().StrengthOutput() == 8)
            {
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestStraightFlush > E.GetComponent<EnemyEvaluation>().EnemyhighestStraightFlush)
                {
                    RoundsBestHand = PlayersBestHandIs;
                    //Player wins
                    didYouDraw = false;
                    didPlayerWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestStraightFlush < E.GetComponent<EnemyEvaluation>().EnemyhighestStraightFlush)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Enemy wins
                    didYouDraw = false;
                    didEnemyWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestStraightFlush == E.GetComponent<EnemyEvaluation>().EnemyhighestStraightFlush)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Draw
                    didYouDraw = true;
                }
            }
            //Quads
            if(P.GetComponent<PlayerEvaluation>().StrengthOutput() == 7)
            {
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestQuads > E.GetComponent<EnemyEvaluation>().EnemyhighestQuads)
                {
                    RoundsBestHand = PlayersBestHandIs;
                    //Player wins
                    didYouDraw = false;
                    didPlayerWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestQuads < E.GetComponent<EnemyEvaluation>().EnemyhighestQuads)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Enemy wins
                    didYouDraw = false;
                    didEnemyWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestQuads == E.GetComponent<EnemyEvaluation>().EnemyhighestQuads)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Draw
                    didYouDraw = true;
                }
            }
            //Full House
            if (P.GetComponent<PlayerEvaluation>().StrengthOutput() == 6)
            {
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestFullHouse > E.GetComponent<EnemyEvaluation>().EnemyhighestFullHouse)
                {
                    RoundsBestHand = PlayersBestHandIs;
                    //Player wins
                    didYouDraw = false;
                    didPlayerWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestFullHouse < E.GetComponent<EnemyEvaluation>().EnemyhighestFullHouse)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Enemy wins
                    didYouDraw = false;
                    didEnemyWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestFullHouse == E.GetComponent<EnemyEvaluation>().EnemyhighestFullHouse)
                {
                    if(P.GetComponent<PlayerEvaluation>().PlayerhighestPair > E.GetComponent<EnemyEvaluation>().EnemyhighestPair)
                    {
                        RoundsBestHand = PlayersBestHandIs;
                        //Player wins
                        didYouDraw = false;
                        didPlayerWin = true;
                    }
                    if (P.GetComponent<PlayerEvaluation>().PlayerhighestPair < E.GetComponent<EnemyEvaluation>().EnemyhighestPair)
                    {
                        RoundsBestHand = EnemyBestHandIs;
                        //Enemy wins
                        didYouDraw = false;
                        didEnemyWin = true;
                    }
                    if (P.GetComponent<PlayerEvaluation>().PlayerhighestPair == E.GetComponent<EnemyEvaluation>().EnemyhighestPair)
                    {
                        RoundsBestHand = EnemyBestHandIs;
                        //Draw
                        didYouDraw = true;
                    }
                }
            }
            //Flush
            if (P.GetComponent<PlayerEvaluation>().StrengthOutput() == 5)
            {
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestFlush > E.GetComponent<EnemyEvaluation>().EnemyhighestFlush)
                {
                    RoundsBestHand = PlayersBestHandIs;
                    //Player wins
                    didYouDraw = false;
                    didPlayerWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestFlush < E.GetComponent<EnemyEvaluation>().EnemyhighestFlush)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Enemy wins
                    didYouDraw = false;
                    didEnemyWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestFlush == E.GetComponent<EnemyEvaluation>().EnemyhighestFlush)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Draw
                    didYouDraw = true;
                }
            }
            //Straight
            if (P.GetComponent<PlayerEvaluation>().StrengthOutput() == 4)
            {
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestStraight > E.GetComponent<EnemyEvaluation>().EnemyhighestStraight)
                {
                    RoundsBestHand = PlayersBestHandIs;
                    //Player wins
                    didYouDraw = false;
                    didPlayerWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestStraight < E.GetComponent<EnemyEvaluation>().EnemyhighestStraight)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Enemy wins
                    didYouDraw = false;
                    didEnemyWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestStraight == E.GetComponent<EnemyEvaluation>().EnemyhighestStraight)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Draw
                    didYouDraw = true;
                }
            }
            //Set
            if (P.GetComponent<PlayerEvaluation>().StrengthOutput() == 3)
            {
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestSet > E.GetComponent<EnemyEvaluation>().EnemyhighestSet)
                {
                    RoundsBestHand = PlayersBestHandIs;
                    //Player wins
                    didYouDraw = false;
                    didPlayerWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestSet < E.GetComponent<EnemyEvaluation>().EnemyhighestSet)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Enemy wins
                    didYouDraw = false;
                    didEnemyWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestSet == E.GetComponent<EnemyEvaluation>().EnemyhighestSet)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Draw
                    didYouDraw = true;
                }
            }
            //Two Pair
            if (P.GetComponent<PlayerEvaluation>().StrengthOutput() == 2)
            {
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestTwoPair > E.GetComponent<EnemyEvaluation>().EnemyhighestTwoPair)
                {
                    RoundsBestHand = PlayersBestHandIs;
                    //Player wins
                    didYouDraw = false;
                    didPlayerWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestTwoPair < E.GetComponent<EnemyEvaluation>().EnemyhighestTwoPair)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Enemy wins
                    didYouDraw = false;
                    didEnemyWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestTwoPair == E.GetComponent<EnemyEvaluation>().EnemyhighestTwoPair)
                {
                    if(P.GetComponent<PlayerEvaluation>().PlayerHighestSecondPair > E.GetComponent<EnemyEvaluation>().EnemyHighestSecondPair)
                    {
                        RoundsBestHand = PlayersBestHandIs;
                        //Player wins
                        didYouDraw = false;
                        didPlayerWin = true;
                    }
                    if (P.GetComponent<PlayerEvaluation>().PlayerHighestSecondPair < E.GetComponent<EnemyEvaluation>().EnemyHighestSecondPair)
                    {
                        RoundsBestHand = EnemyBestHandIs;
                        //Enemy wins
                        didYouDraw = false;
                        didEnemyWin = true;
                    }
                    if (P.GetComponent<PlayerEvaluation>().PlayerHighestSecondPair == E.GetComponent<EnemyEvaluation>().EnemyHighestSecondPair)
                    {
                        RoundsBestHand = EnemyBestHandIs;
                        //Draw
                        didYouDraw = true;
                    }
                }
            }
            //Pair
            if (P.GetComponent<PlayerEvaluation>().StrengthOutput() == 1)
            {
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestPair > E.GetComponent<EnemyEvaluation>().EnemyhighestPair)
                {
                    RoundsBestHand = PlayersBestHandIs;
                    //Player wins
                    didYouDraw = false;
                    didPlayerWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestPair < E.GetComponent<EnemyEvaluation>().EnemyhighestPair)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Enemy wins
                    didYouDraw = false;
                    didEnemyWin = true;
                }
                if (P.GetComponent<PlayerEvaluation>().PlayerhighestPair == E.GetComponent<EnemyEvaluation>().EnemyhighestPair)
                {
                    RoundsBestHand = EnemyBestHandIs;
                    //Draw
                    didYouDraw = true;
                }
            }
        }


    }


    // Update is called once per frame
    void Update ()
    {//game states management and switching logic
        switch(GS)
        {
            case GameStates.BeforeHand:
                {
                    if(initialDeal == true)
                    {
                        HUS.GetComponent<HeadsUpStart>().ShuffleAndDealHoleCards();
                        NextGameState();
                        initialDeal = false;
                    }
                    break;
                   
                }

            case GameStates.Preflop:
                {
                    if(initialDeal == true)
                    {
                        initialDeal = false;
                    }
                    if( PS == PlayerStates.Check && ES == EnemyStates.Check)
                    {
                        HUS.GetComponent<HeadsUpStart>().DealFlop();
                        NextGameState();
                        PS = PlayerStates.Nothing;
                        ES = EnemyStates.Nothing;
                    }
                    if(PS == PlayerStates.Bet && ES == EnemyStates.Bet)
                    {
                        HUS.GetComponent<HeadsUpStart>().DealFlop();
                        PS = PlayerStates.Nothing;
                        ES = EnemyStates.Nothing;
                        NextGameState();
                    }
                    if(PS == PlayerStates.Fold || ES == EnemyStates.Fold)
                    {
                        didEnemyWin = true;
                        didYouDraw = false;
                        roundChipShare();
                        GS = GameStates.EndHand;
                    }
                    break;
                }
            case GameStates.Flop:
                {
                    if (initialDeal == true)
                    {
                        initialDeal = false;
                    }
                    if (PS == PlayerStates.Check && ES == EnemyStates.Check)
                    {
                        HUS.GetComponent<HeadsUpStart>().DealTurn();
                        NextGameState();
                        PS = PlayerStates.Nothing;
                        ES = EnemyStates.Nothing;
                    }
                    if (PS == PlayerStates.Bet && ES == EnemyStates.Bet)
                    {
                        HUS.GetComponent<HeadsUpStart>().DealTurn();
                        PS = PlayerStates.Nothing;
                        ES = EnemyStates.Nothing;
                        NextGameState();
                    }
                    if (PS == PlayerStates.Fold || ES == EnemyStates.Fold)
                    {
                        didEnemyWin = true;
                        didYouDraw = false;
                        roundChipShare();
                        GS = GameStates.EndHand;
                    }
                    break;
                }
            case GameStates.Turn:
                {
                    if (initialDeal == true)
                    {
                        initialDeal = false;
                    }
                    if (PS == PlayerStates.Check && ES == EnemyStates.Check)
                    {
                        HUS.GetComponent<HeadsUpStart>().DealRiver();
                        NextGameState();
                        PS = PlayerStates.Nothing;
                        ES = EnemyStates.Nothing;
                    }
                    if (PS == PlayerStates.Bet && ES == EnemyStates.Bet)
                    {
                        HUS.GetComponent<HeadsUpStart>().DealRiver();
                        PS = PlayerStates.Nothing;
                        ES = EnemyStates.Nothing;
                        NextGameState();
                    }
                    if (PS == PlayerStates.Fold || ES == EnemyStates.Fold)
                    {
                        didEnemyWin = true;
                        didYouDraw = false;
                        roundChipShare();
                        GS = GameStates.EndHand;
                    }
                    break;
                }
            case GameStates.River:
                {
                    if (initialDeal == true)
                    {
                        initialDeal = false;
                    }
                    if (PS == PlayerStates.Check && ES == EnemyStates.Check)
                    {
                        NextGameState();
                        PS = PlayerStates.Nothing;
                        ES = EnemyStates.Nothing;
                        roundChipShare();
                        DestroyTheCards.interactable = true;
                    }
                    if (PS == PlayerStates.Bet && ES == EnemyStates.Bet)
                    {
                        PS = PlayerStates.Nothing;
                        ES = EnemyStates.Nothing;
                        NextGameState();
                        roundChipShare();
                        DestroyTheCards.interactable = true;
                    }
                    if (PS == PlayerStates.Fold || ES == EnemyStates.Fold)
                    {
                        GS = GameStates.EndHand;
                    }
                    break;
                }
            case GameStates.EndHand:
                {
                    if(PS == PlayerStates.Fold)
                    {
                        didEnemyWin = true;
                        didYouDraw = false;
                        roundChipShare();
                        DestroyTheCards.interactable = true;
                        didEnemyWin = false;
                        PS = PlayerStates.Nothing;
                    }
                    if (ES == EnemyStates.Fold)
                    {
                        didPlayerWin = true;
                        didYouDraw = false;
                        roundChipShare();
                        DestroyTheCards.interactable = true;
                        didPlayerWin = false;
                        ES = EnemyStates.Nothing;
                    }

                    if (initialDeal == true)
                    {
                        initialDeal = false;
                    }
                    break;
                }
        }
        
    }
    //Function that can be called to move onto the next stage of play
    public void NextGameState()
    {
        switch (GS)
        {
            case GameStates.BeforeHand:
                {
                    GS = GameStates.Preflop;
                    break;
                }
            case GameStates.Preflop:
                {
                    GS = GameStates.Flop;
                    break;
                }
            case GameStates.Flop:
                {
                    GS = GameStates.Turn;
                    break;
                }
            case GameStates.Turn:
                {
                    GS = GameStates.River;
                    break;
                }
            case GameStates.River:
                {
                    GS = GameStates.EndHand;
                    break;
                }
            case GameStates.EndHand:
                {
                    GS = GameStates.Preflop;
                    break;
                }
        }
    }
    //Function that moves the gamestate to a specific state under a specific condition
    public void HandFolded()
    {
        if (PA.GetComponent<PlayerActions>().PS == PlayerStates.Fold)
        {
            GS = GameStates.EndHand;
        }
        if (PA.GetComponent<PlayerActions>().ES == EnemyStates.Fold)
        {
            GS = GameStates.EndHand;
        }
    }

}
