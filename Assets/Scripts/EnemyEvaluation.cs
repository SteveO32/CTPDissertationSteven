using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Class to manage the opponent's card evaluation system
//Works by taking up to the maximum seven cards in play
//and filtering out the highest ranked possibility of hand
//down to the lowest, i.e. detects made hands

public class EnemyEvaluation : MonoBehaviour {
    //array to store the card objects
    public GameObject[] EnemySevenCards;
    //to access other class functions
    GameObject HUD;
    GameObject GM;
    //variables to faciltate evaluation checks
    private string StraightFlush;
    private string Quads;
    private string FullHouse;
    private string Flush;
    private string Straight;
    private string Set;
    private string TwoPair;
    private string Pair;

    private bool flush = false;
    private bool isStraightFlush = false;
    private bool isQuads = false;
    private bool isFullHouse = false;
    private bool isFlush = false;
    private bool isStraight = false;
    private bool isSet = false;
    private bool isTwoPair = false;
    private bool isPair = false;

    private int highestStraightFlush = 0;
    private int highestQuads = 0;
    private int highestFullHouse = 0;
    private int highestFlush = 0;
    private int highestStraight = 0;
    private int highestSet = 0;
    private int highestTwoPair = 0;
    private int highestPair = 0;
    private int SecondHighestTwoPair = 0;
    public int EnemyhighestStraightFlush = 0;
    public int EnemyhighestQuads = 0;
    public int EnemyhighestFullHouse = 0;
    public int EnemyhighestFlush = 0;
    public int EnemyhighestStraight = 0;
    public int EnemyhighestSet = 0;
    public int EnemyhighestTwoPair = 0;
    public int EnemyhighestPair = 0;
    public int EnemyHighestSecondPair = 0;
    public int EnemyHandStrengthType = 0;

    // Use this for initialization
    void Start()
    {   //Initialise reference to other classes
        HUD = GameObject.FindGameObjectWithTag("HeadsUp");
        GM = GameObject.FindGameObjectWithTag("GameManager");
    }
    
    public void GrabCards()
    {
        //Used to store the current max 7 cards in play
        EnemySevenCards[0] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[1];
        EnemySevenCards[1] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[3];
        EnemySevenCards[2] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[5];
        EnemySevenCards[3] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[6];
        EnemySevenCards[4] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[7];
        EnemySevenCards[5] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[9];
        EnemySevenCards[6] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[11];
    }
    //function to check for flushes, takes stage parameter to know
    //how many cards are in play at the stage of play
    public void CheckForFlush(int stage)
    {
        //list to store card names for outputting
        List<int> cardsName = new List<int>();
        int clubflush = 0;
        int spadeflush = 0;
        int heartflush = 0;
        int diamondflush = 0;
        int clubhighFlushCard = 0;
        int spadehighFlushCard = 0;
        int hearthighFlushCard = 0;
        int diamondhighFlushCard = 0;
        int temp = 0;
        string help;
        //for loop iterating checks through each card in the current stage
        for (int i = 0; i < EnemySevenCards.Length - stage; i++)
        {
            //grabbing the names from the objects to identify the cards
            help = EnemySevenCards[i].gameObject.name;
            help = help.Substring(0, help.LastIndexOf("(Clone)"));
            //parse string into int
            temp = int.Parse(help);
            cardsName.Add(temp);
            //sorting the cards from highest to lowest for efficiency
            //to catch highest valued flush first instead of potentially
            //having to check through several lower level hands first
            cardsName.Sort();
            cardsName.Reverse();
            //counting the suits for the cards
            if (EnemySevenCards[i].gameObject.tag == "Clubs")
            {
                clubflush++;
                if (EnemySevenCards[i].gameObject.tag == "Clubs" && temp > clubhighFlushCard)
                {
                    clubhighFlushCard = temp;
                }
            }
            if (EnemySevenCards[i].gameObject.tag == "Spade")
            {
                spadeflush++;
                if (EnemySevenCards[i].gameObject.tag == "Spade" && temp > spadehighFlushCard)
                {
                    spadehighFlushCard = temp;
                }
            }
            if (EnemySevenCards[i].gameObject.tag == "Heart")
            {
                heartflush++;
                if (EnemySevenCards[i].gameObject.tag == "Heart" && temp > hearthighFlushCard)
                {
                    hearthighFlushCard = temp;
                }
            }
            if (EnemySevenCards[i].gameObject.tag == "Diamond")
            {
                diamondflush++;
                if (EnemySevenCards[i].gameObject.tag == "Diamond" && temp > diamondhighFlushCard)
                {
                    diamondhighFlushCard = temp;
                }
            }
        }
        //if there are 5 or more same suited cards then there is a flush
        if (clubhighFlushCard > spadehighFlushCard && clubhighFlushCard > hearthighFlushCard && clubhighFlushCard > diamondhighFlushCard)
        {
            if (clubflush > 4)
            {
                //trigger bool to indicate flush and identify the
                //high card and the flush suit to a string for output
                flush = true;
                isFlush = true;
                highestFlush = clubhighFlushCard;
                Flush = clubhighFlushCard.ToString();
            }
        }

        if (spadehighFlushCard > clubhighFlushCard && spadehighFlushCard > hearthighFlushCard && spadehighFlushCard > diamondhighFlushCard)
        {
            if (spadeflush > 4)
            {
                flush = true;
                isFlush = true;
                highestFlush = spadehighFlushCard;
                Flush = spadehighFlushCard.ToString();
            }
        }

        if (hearthighFlushCard > clubhighFlushCard && hearthighFlushCard > spadehighFlushCard && hearthighFlushCard > diamondhighFlushCard)
        {
            if (heartflush > 4)
            {
                flush = true;
                isFlush = true;
                highestFlush = hearthighFlushCard;
                Flush = hearthighFlushCard.ToString();
            }
        }

        if (diamondhighFlushCard > clubhighFlushCard && diamondhighFlushCard > spadehighFlushCard && diamondhighFlushCard > hearthighFlushCard)
        {
            if (diamondflush > 4)
            {
                flush = true;
                isFlush = true;
                highestFlush = diamondflush;
                Flush = diamondhighFlushCard.ToString();
            }
        }
    }
    //Function to check for different card multiple hands
    //Eg pairs, set
    public void CardMultiples(int stage)
    {
        //dictionary used to store suit and value of card to count
        Dictionary<int, int> cardCounting = new Dictionary<int, int>();
        List<int> cardsName = new List<int>();
        int temp = 0;
        string help;
        int firstPair = 0;
        int secondPair = 0;
        int HighestPair = 0;
        int twoPairCount = 0;
        int firstSet = 0;
        int secondSet = 0;
        int HighestSet = 0;
        int fullHouseCount = 0;

        for (int i = 0; i < EnemySevenCards.Length - stage; i++)
        {
            //sorting the list of cards from highest to lowest for effiency
            //for checks to find the highest value hands first
            help = EnemySevenCards[i].gameObject.name;
            help = help.Substring(0, help.LastIndexOf("(Clone)"));
            temp = int.Parse(help);
            cardsName.Add(temp);
            cardsName.Sort();
            cardsName.Reverse();
        }
        //foreach used to make a count of how many of the same value card
        foreach (int item in cardsName)
        {
            if (!cardCounting.ContainsKey(item))
            {
                cardCounting.Add(item, 1);
            }
            else
            {
                int counting = 0;
                cardCounting.TryGetValue(item, out counting);
                cardCounting.Remove(item);
                cardCounting.Add(item, counting + 1);
            }
        }

        var ranks = from pair in cardCounting orderby pair.Value descending select pair;
        //Checks for multiples
        foreach (KeyValuePair<int, int> pair in ranks)
        {//logic to filter through multiples
            if (pair.Value == 2)
            {
                twoPairCount++;

                if (twoPairCount == 1)
                {
                    firstPair = pair.Key;
                    HighestPair = firstPair;
                    highestPair = HighestPair;
                }
                else if (twoPairCount == 2)
                {
                    secondPair = pair.Key;
                    TwoPair = firstPair.ToString() + " and " + secondPair.ToString();
                    isTwoPair = true;

                    if (firstPair < secondPair)
                    {
                        HighestPair = secondPair;
                        highestTwoPair = HighestPair;
                        SecondHighestTwoPair = firstPair;
                    }
                    else
                    {
                        HighestPair = firstPair;
                        highestTwoPair = HighestPair;
                        SecondHighestTwoPair = secondPair;
                    }
                }
                else if (twoPairCount == 3)
                {
                    if (pair.Key > firstPair && pair.Key < secondPair)
                    {
                        firstPair = pair.Key;
                        TwoPair = secondPair.ToString() + " and " + firstPair.ToString();
                        isTwoPair = true;
                        highestTwoPair = secondPair;
                        SecondHighestTwoPair = pair.Key;
                    }
                    if (pair.Key < firstPair && pair.Key > secondPair)
                    {
                        secondPair = pair.Key;
                        TwoPair = secondPair.ToString() + " and " + firstPair.ToString();
                        isTwoPair = true;
                        highestTwoPair = firstPair;
                        SecondHighestTwoPair = pair.Key;
                    }
                    if (pair.Key > firstPair && pair.Key > secondPair)
                    {
                        HighestPair = pair.Key;
                        highestTwoPair = pair.Key;
                        if (firstPair < secondPair)
                        {
                            TwoPair = secondPair.ToString() + " and " + HighestPair.ToString();
                            isTwoPair = true;
                            SecondHighestTwoPair = secondPair;
                        }
                        else
                        {
                            TwoPair = HighestPair.ToString() + " and " + firstPair.ToString();
                            isTwoPair = true;
                            SecondHighestTwoPair = firstPair;
                        }
                    }
                }

                Pair = HighestPair.ToString();
                isPair = true;

            }

            if (pair.Value == 3)
            {
                fullHouseCount++;

                if (fullHouseCount < 2)
                {
                    firstSet = pair.Key;
                    HighestSet = firstSet;
                    highestSet = firstSet;
                }
                if (fullHouseCount == 2)
                {
                    secondSet = pair.Key;
                    if (firstSet > secondSet)
                    {
                        HighestSet = firstSet;
                        highestSet = firstSet;
                        isFullHouse = true;
                        FullHouse = HighestSet.ToString() + " and " + HighestPair.ToString();
                    }
                    else
                    {
                        HighestSet = secondSet;
                        highestSet = secondSet;
                        isFullHouse = true;
                        FullHouse = HighestSet.ToString() + " and " + HighestPair.ToString();
                    }
                }

                Set = HighestSet.ToString();
                isSet = true;
            }

            if (firstSet > 0 && firstPair > 0)
            {
                isFullHouse = true;
                highestFullHouse = highestSet;
                FullHouse = HighestSet.ToString() + " and " + HighestPair.ToString();
            }

            if (pair.Value == 4)
            {
                isQuads = true;
                highestQuads = pair.Key;
                Quads = pair.Key.ToString();
            }

        }
    }
    //Function to check for straight
    public void CheckStraight(int stage)
    {
        //dictionary to store card value and suit
        Dictionary<int, int> cardCounting = new Dictionary<int, int>();
        List<int> cardsName = new List<int>();

        int temp = 0;
        string help;
        int FirstStraightHighFlush = 0;
        int SecondStraightHighFlush = 0;
        int ThirdStraightHighFlush = 0;
        int FirstStraight = 0;
        int SecondStraight = 0;
        int ThirdStraight = 0;

        for (int i = 0; i < EnemySevenCards.Length - stage; i++)
        {   //rearrange the list to have highest value first for effiency
            help = EnemySevenCards[i].gameObject.name;
            help = help.Substring(0, help.LastIndexOf("(Clone)"));
            temp = int.Parse(help);
            cardsName.Add(temp);
            cardsName.Sort();
            cardsName.Reverse();
        }
        //checking for straight of 5 cards
        if (cardsName.Count >= 5)
        {
            if (cardsName[0] - cardsName[1] == 1)
            {
                if (cardsName[1] - cardsName[2] == 1)
                {
                    if (cardsName[2] - cardsName[3] == 1)
                    {
                        if (cardsName[3] - cardsName[4] == 1)
                        {
                            //additional check to find flush, and if present
                            //too then straight flush is present
                            if (flush == true)
                            {
                                FirstStraightHighFlush = cardsName[0];
                            }
                            else
                            {
                                FirstStraight = cardsName[0];
                            }
                        }
                    }
                }
            }
            //checks for more than 5 cards
            if (cardsName.Count >= 6)
            {
                if (cardsName[1] - cardsName[2] == 1)
                {
                    if (cardsName[2] - cardsName[3] == 1)
                    {
                        if (cardsName[3] - cardsName[4] == 1)
                        {
                            if (cardsName[4] - cardsName[5] == 1)
                            {
                                if (flush == true)
                                {
                                    SecondStraightHighFlush = cardsName[1];
                                }
                                else
                                {
                                    SecondStraight = cardsName[1];
                                }
                            }
                        }
                    }
                }

                if (cardsName.Count == 7)
                {
                    if (cardsName[2] - cardsName[3] == 1)
                    {
                        if (cardsName[3] - cardsName[4] == 1)
                        {
                            if (cardsName[4] - cardsName[5] == 1)
                            {
                                if (cardsName[5] - cardsName[6] == 1)
                                {
                                    if (flush == true)
                                    {
                                        ThirdStraightHighFlush = cardsName[2];
                                    }
                                    else
                                    {
                                        ThirdStraight = cardsName[2];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //logic to filter the strongest hand if multiple of the same type of hand is present
        if (FirstStraightHighFlush > SecondStraightHighFlush && FirstStraightHighFlush > ThirdStraightHighFlush)
        {
            StraightFlush = FirstStraightHighFlush.ToString();
            highestStraightFlush = FirstStraightHighFlush;
            isStraightFlush = true;
        }
        if (SecondStraightHighFlush > FirstStraightHighFlush && SecondStraightHighFlush > ThirdStraightHighFlush)
        {
            StraightFlush = SecondStraightHighFlush.ToString();
            highestStraightFlush = SecondStraightHighFlush;
            isStraightFlush = true;
        }
        if (ThirdStraightHighFlush > FirstStraightHighFlush && ThirdStraightHighFlush > SecondStraightHighFlush)
        {
            StraightFlush = ThirdStraightHighFlush.ToString();
            highestStraightFlush = ThirdStraightHighFlush;
            isStraightFlush = true;
        }

        if (FirstStraight > SecondStraight && FirstStraight > ThirdStraight)
        {
            Straight = FirstStraight.ToString();
            highestStraight = FirstStraight;
            isStraight = true;
        }
        if (SecondStraight > FirstStraight && SecondStraight > ThirdStraight)
        {
            Straight = SecondStraight.ToString();
            highestStraight = SecondStraight;
            isStraight = true;
        }
        if (ThirdStraight > FirstStraight && ThirdStraight > SecondStraight)
        {
            Straight = ThirdStraight.ToString();
            highestStraight = ThirdStraight;
            isStraight = true;
        }
    }

    public void StrongestHand()
    {
        // Take results from checks to determine player's strongest hand
        bool handDoneYet = false;
        //Logic to filter out the strongest hand from all the possible
        //hands that may have been detected
        //works from the highest value type of hand down to the lowest
        //in order to be the quickest to discover the strongest hand
        //without finding other lower strength hands and always having to work
        //from bottom to top, just stops as soon as one is found 
        //as it is guaranteed the highest strength due to the order
        if (isStraightFlush == true)
        {
                GM.GetComponent<GameManager>().EnemyBestHandIs = StraightFlush + " High straight flush";
                EnemyhighestStraightFlush = highestStraightFlush;
            EnemyHandStrengthType = 8;
            handDoneYet = true;
        }
        else if (isQuads == true && handDoneYet == false)
        {
                GM.GetComponent<GameManager>().EnemyBestHandIs = "Four of a kind of: " + Quads;
                EnemyhighestQuads = highestQuads;
            EnemyHandStrengthType = 7;
            handDoneYet = true;
        }
        else if (isFullHouse == true && handDoneYet == false)
        {
                GM.GetComponent<GameManager>().EnemyBestHandIs = "Full House of: " + FullHouse;
                EnemyhighestFullHouse = highestFullHouse;
                EnemyhighestPair = highestPair;
            EnemyHandStrengthType = 6;
            handDoneYet = true;
        }
        else if (isFlush == true && handDoneYet == false)
        {
                GM.GetComponent<GameManager>().EnemyBestHandIs = Flush + " high flush";
                EnemyhighestFlush = highestFlush;
            EnemyHandStrengthType = 5;
            handDoneYet = true;
        }
        else if (isStraight == true && handDoneYet == false)
        {
                GM.GetComponent<GameManager>().EnemyBestHandIs = Straight + " High straight";
                EnemyhighestStraight = highestStraight;
            EnemyHandStrengthType = 4;
            handDoneYet = true;
        }
        else if (isSet == true && handDoneYet == false)
        {
                GM.GetComponent<GameManager>().EnemyBestHandIs = "Set of: " + Set;
                EnemyhighestSet = highestSet;
            EnemyHandStrengthType = 3;
            handDoneYet = true;
        }
        else if (isTwoPair == true && handDoneYet == false)
        {
                EnemyhighestTwoPair = highestTwoPair;
                EnemyHighestSecondPair = SecondHighestTwoPair;
                GM.GetComponent<GameManager>().EnemyBestHandIs = "Two pair of: " + EnemyhighestTwoPair.ToString() + " and " + SecondHighestTwoPair;
            EnemyHandStrengthType = 2;
            handDoneYet = true;
        }
        else if (isPair == true && handDoneYet == false)
        {
                GM.GetComponent<GameManager>().EnemyBestHandIs = "Pair of: " + Pair;
                EnemyhighestPair = highestPair;
            EnemyHandStrengthType = 1;
            handDoneYet = true;
        }
    }
    //Function to clear shared used variables to reset the checks made
    //between stages of play to make the functions reusable
    public void ClearCurrentHandEval()
    {
        for (int i = 0; i < EnemySevenCards.Length; i++)
        {
            EnemySevenCards[i] = null;
        }
        flush = false;
        isStraightFlush = false;
        isQuads = false;
        isFullHouse = false;
        isFlush = false;
        isStraight = false;
        isSet = false;
        isTwoPair = false;
        isPair = false;
    }
    //used for game manager logic to evaluate between opponent
    //and players hand strength to determine winner
    public int StrengthOutput()
    {
        return EnemyHandStrengthType;
    }






}
