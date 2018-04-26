using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EvaluatingHand : MonoBehaviour {

    public GameObject[] AllSevenCards;
    GameObject HUD;
    GameObject GM;

    bool flush = false;

    string StraightFlush;
    string Quads;
    string FullHouse;
    string Flush;
    string Straight;
    string Set;
    string TwoPair;
    string Pair;

    bool isStraightFlush = false;
    bool isQuads = false;
    bool isFullHouse = false;
    bool isFlush = false;
    bool isStraight = false;
    bool isSet = false;
    bool isTwoPair = false;
    bool isPair = false;

    int highestStraightFlush = 0;
    int highestQuads = 0;
    int highestFullHouse = 0;
    int highestFlush = 0;
    int highestStraight = 0;
    int highestSet = 0;
    int highestTwoPair = 0;
    int highestPair = 0;
    int EnemyhighestStraightFlush = 0;
    int EnemyhighestQuads = 0;
    int EnemyhighestFullHouse = 0;
    int EnemyhighestFlush = 0;
    int EnemyhighestStraight = 0;
    int EnemyhighestSet = 0;
    int EnemyhighestTwoPair = 0;
    int EnemyhighestPair = 0;
    int PlayerhighestStraightFlush = 0;
    int PlayerhighestQuads = 0;
    int PlayerhighestFullHouse = 0;
    int PlayerhighestFlush = 0;
    int PlayerhighestStraight = 0;
    int PlayerhighestSet = 0;
    int PlayerhighestTwoPair = 0;
    int PlayerhighestPair = 0;
    int SecondHighestTwoPair = 0;
    int EnemyHighestSecondPair = 0;
    int PlayerHighestSecondPair = 0;

    // Use this for initialization
    void Start ()
    {
        HUD = GameObject.FindGameObjectWithTag("HeadsUp");
        GM = GameObject.FindGameObjectWithTag("GameManager");
    }
	
	// Update is called once per frame
	void Update () {
		
    }
    public void GrabCards(string name)
    {
        if (name == "Player")
        {
            AllSevenCards[0] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[0];
            AllSevenCards[1] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[2];
            AllSevenCards[2] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[5];
            AllSevenCards[3] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[6];
            AllSevenCards[4] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[7];
            AllSevenCards[5] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[9];
            AllSevenCards[6] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[11];
        }
        if(name == "Enemy")
        {
            AllSevenCards[0] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[1];
            AllSevenCards[1] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[3];
            AllSevenCards[2] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[5];
            AllSevenCards[3] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[6];
            AllSevenCards[4] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[7];
            AllSevenCards[5] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[9];
            AllSevenCards[6] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[11];
        }
    }

    public void CheckForFlush(int stage)
    {
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
        
        for (int i = 0; i < AllSevenCards.Length - stage; i++)
        {
            help = AllSevenCards[i].gameObject.name;
            help = help.Substring(0, help.LastIndexOf("(Clone)"));
            temp = int.Parse(help);
            cardsName.Add(temp);
            cardsName.Sort();
            cardsName.Reverse();

            if (AllSevenCards[i].gameObject.tag == "Clubs")
                {
                    clubflush++;
                if(AllSevenCards[i].gameObject.tag == "Clubs" && temp > clubhighFlushCard)
                {
                    clubhighFlushCard = temp;
                }
            }
            if (AllSevenCards[i].gameObject.tag == "Spade")
                {
                    spadeflush++;
                if (AllSevenCards[i].gameObject.tag == "Spade" && temp > spadehighFlushCard)
                {
                    spadehighFlushCard = temp;
                }
            }
            if (AllSevenCards[i].gameObject.tag == "Heart")
                {
                    heartflush++;
                if (AllSevenCards[i].gameObject.tag == "Heart" && temp > hearthighFlushCard)
                {
                    hearthighFlushCard = temp;
                }
            }
            if (AllSevenCards[i].gameObject.tag == "Diamond")
                {
                    diamondflush++;
                if (AllSevenCards[i].gameObject.tag == "Diamond" && temp > diamondhighFlushCard)
                {
                    diamondhighFlushCard = temp;
                }
            }
        }

        if(clubhighFlushCard > spadehighFlushCard && clubhighFlushCard > hearthighFlushCard && clubhighFlushCard > diamondhighFlushCard)
        {
            if(clubflush > 4)
            {
                flush = true;
                isFlush = true;
                highestFlush = clubhighFlushCard;
                Flush = clubhighFlushCard.ToString();
            }
        }

        if (spadehighFlushCard > clubhighFlushCard && spadehighFlushCard > hearthighFlushCard && spadehighFlushCard > diamondhighFlushCard)
        {
            if(spadeflush > 4)
            {
                flush = true;
                isFlush = true;
                highestFlush = spadehighFlushCard;
                Flush = spadehighFlushCard.ToString();
            }
        }

        if (hearthighFlushCard > clubhighFlushCard && hearthighFlushCard > spadehighFlushCard && hearthighFlushCard > diamondhighFlushCard)
        {
            if(heartflush > 4)
            {
                flush = true;
                isFlush = true;
                highestFlush = hearthighFlushCard;
                Flush = hearthighFlushCard.ToString();
            }
        }

        if (diamondhighFlushCard > clubhighFlushCard && diamondhighFlushCard > spadehighFlushCard && diamondhighFlushCard > hearthighFlushCard)
        {
            if(diamondflush > 4)
            {
                flush = true;
                isFlush = true;
                highestFlush = diamondflush;
                Flush = diamondhighFlushCard.ToString();
            }
        }
    }

    public void CardMultiples(int stage)
    {
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

        for (int i = 0; i < AllSevenCards.Length - stage; i++)
        {
            help = AllSevenCards[i].gameObject.name;
            help = help.Substring(0, help.LastIndexOf("(Clone)"));
            temp = int.Parse(help);
            cardsName.Add(temp);
            cardsName.Sort();
            cardsName.Reverse();
        }
        
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

        foreach (KeyValuePair<int, int> pair in ranks)
        {
            if (pair.Value == 2)
            {
                twoPairCount++;

                if( twoPairCount == 1)
                {
                    firstPair = pair.Key;
                    HighestPair = firstPair;
                    highestPair = HighestPair;
                }else if (twoPairCount == 2)
                {
                    secondPair = pair.Key;
                    TwoPair = firstPair.ToString() + " and " + secondPair.ToString();
                    isTwoPair = true;

                    if(firstPair < secondPair)
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
                }else if(twoPairCount == 3)
                {
                    if(pair.Key > firstPair && pair.Key < secondPair)
                    {
                        firstPair = pair.Key;
                        TwoPair = secondPair.ToString() + " and " + firstPair.ToString();
                        isTwoPair = true;
                        highestTwoPair = secondPair;
                        SecondHighestTwoPair = pair.Key;
                    }
                    if(pair.Key < firstPair && pair.Key > secondPair)
                    {
                        secondPair = pair.Key;
                        TwoPair = secondPair.ToString() + " and " + firstPair.ToString();
                        isTwoPair = true;
                        highestTwoPair = firstPair;
                        SecondHighestTwoPair = pair.Key;
                    }
                    if(pair.Key > firstPair && pair.Key > secondPair)
                    {
                        HighestPair = pair.Key;
                        highestTwoPair = pair.Key;
                        if(firstPair < secondPair)
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
                    if(firstSet > secondSet)
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

            if(firstSet > 0 && firstPair > 0)
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

    public void CheckStraight(int stage)
    {
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

        for (int i = 0; i < AllSevenCards.Length - stage; i++)
        {
            help = AllSevenCards[i].gameObject.name;
            help = help.Substring(0, help.LastIndexOf("(Clone)"));
            temp = int.Parse(help);
            cardsName.Add(temp);
            cardsName.Sort();
            cardsName.Reverse();
        }

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

                if(cardsName.Count == 7)
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

        if(FirstStraightHighFlush > SecondStraightHighFlush && FirstStraightHighFlush > ThirdStraightHighFlush)
        {
            StraightFlush = FirstStraightHighFlush.ToString();
            highestStraightFlush = FirstStraightHighFlush;
            isStraightFlush = true;
        }
        if(SecondStraightHighFlush > FirstStraightHighFlush && SecondStraightHighFlush > ThirdStraightHighFlush)
        {
            StraightFlush = SecondStraightHighFlush.ToString();
            highestStraightFlush = SecondStraightHighFlush;
            isStraightFlush = true;
        }
        if(ThirdStraightHighFlush > FirstStraightHighFlush && ThirdStraightHighFlush > SecondStraightHighFlush)
        {
            StraightFlush = ThirdStraightHighFlush.ToString();
            highestStraightFlush = ThirdStraightHighFlush;
            isStraightFlush = true;
        }

        if(FirstStraight > SecondStraight && FirstStraight > ThirdStraight)
        {
            Straight = FirstStraight.ToString();
            highestStraight = FirstStraight;
            isStraight = true;
        }
        if(SecondStraight > FirstStraight && SecondStraight > ThirdStraight)
        {
            Straight = SecondStraight.ToString();
            highestStraight = SecondStraight;
            isStraight = true;
        }
        if(ThirdStraight > FirstStraight && ThirdStraight > SecondStraight)
        {
            Straight = ThirdStraight.ToString();
            highestStraight = ThirdStraight;
            isStraight = true;
        }
    }

    public void StrongestHand(string whichPlayer)
    {
        // Take results from checks to determine player's strongest hand
        //if(whichPlayer == "Enemy" || whichPlayer == "Player")
        //{
        


        if (isStraightFlush == true)
            {
                if(whichPlayer == "Enemy")
                {
                    GM.GetComponent<GameManager>().EnemyBestHandIs = StraightFlush + " High straight flush";
                EnemyhighestStraightFlush = highestStraightFlush;
                }
                if (whichPlayer == "Player")
                {
                    GM.GetComponent<GameManager>().PlayersBestHandIs = StraightFlush + " High straight flush";
                PlayerhighestStraightFlush = highestStraightFlush;
                }
                if(EnemyhighestStraightFlush > PlayerhighestStraightFlush)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = EnemyhighestStraightFlush.ToString() + " High straight flush";
                GM.GetComponent<GameManager>().didYouWin = false;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if (EnemyhighestStraightFlush < PlayerhighestStraightFlush)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = PlayerhighestStraightFlush.ToString() + " High straight flush";
                GM.GetComponent<GameManager>().didYouWin = true;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if(EnemyhighestStraightFlush == PlayerhighestStraightFlush)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = PlayerhighestStraightFlush.ToString() + " High straight flush";
                GM.GetComponent<GameManager>().didYouDraw = true;
            }
        }else if (isQuads == true)
            {
                if (whichPlayer == "Enemy")
                    {
                        GM.GetComponent<GameManager>().EnemyBestHandIs = "Four of a kind of: " + Quads;
                EnemyhighestQuads = highestQuads;
                    }
                if (whichPlayer == "Player")
                    {
                        GM.GetComponent<GameManager>().PlayersBestHandIs = "Four of a kind of: " + Quads;
                PlayerhighestQuads = highestQuads;
                    }
            if (EnemyhighestQuads > PlayerhighestQuads)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Four of a kind of: " + EnemyhighestQuads.ToString();
                GM.GetComponent<GameManager>().didYouWin = false;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if (EnemyhighestQuads < PlayerhighestQuads)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Four of a kind of: " + PlayerhighestQuads.ToString();
                GM.GetComponent<GameManager>().didYouWin = true;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if(EnemyhighestQuads == PlayerhighestQuads)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Four of a kind of: " + PlayerhighestQuads.ToString();
                GM.GetComponent<GameManager>().didYouDraw = true;
            }
        }else if (isFullHouse == true)
                {
                if(whichPlayer == "Enemy")
                    {
                        GM.GetComponent<GameManager>().EnemyBestHandIs = "Full House of: " + FullHouse;
                EnemyhighestFullHouse = highestFullHouse;
                EnemyhighestPair = highestPair;
                    }
                if (whichPlayer == "Player")
                    {
                        GM.GetComponent<GameManager>().PlayersBestHandIs = "Full House of: " + FullHouse;
                PlayerhighestFullHouse = highestFullHouse;
                PlayerhighestPair = highestPair;
                    }
            if (EnemyhighestSet > PlayerhighestSet)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Full House of: " + FullHouse + " and " + EnemyhighestPair;
                GM.GetComponent<GameManager>().didYouWin = false;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if (EnemyhighestSet < PlayerhighestSet)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Full House of: " + FullHouse + " and " + PlayerhighestPair;
                GM.GetComponent<GameManager>().didYouWin = true;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if(EnemyhighestSet == PlayerhighestSet)
            {
                if(EnemyhighestPair > PlayerhighestPair)
                {
                    GM.GetComponent<GameManager>().RoundsBestHand = "Full House of: " + FullHouse + " and " + EnemyhighestPair;
                    GM.GetComponent<GameManager>().didYouWin = false;
                    GM.GetComponent<GameManager>().didYouDraw = false;
                }
                if (EnemyhighestPair < PlayerhighestPair)
                {
                    GM.GetComponent<GameManager>().RoundsBestHand = "Full House of: " + FullHouse + " and " + PlayerhighestPair;
                    GM.GetComponent<GameManager>().didYouWin = true;
                    GM.GetComponent<GameManager>().didYouDraw = false;
                }
                if(EnemyhighestPair == PlayerhighestPair)
                {
                    GM.GetComponent<GameManager>().RoundsBestHand = "Full House of: " + FullHouse + " and " + PlayerhighestPair;
                    GM.GetComponent<GameManager>().didYouDraw = true;
                }
            }
        }else if (isFlush == true)
            {
                if (whichPlayer == "Enemy")
                    {
                        GM.GetComponent<GameManager>().EnemyBestHandIs = Flush + " high flush";
                        EnemyhighestFlush = highestFlush;
                    }
                if (whichPlayer == "Player")
                    {
                        GM.GetComponent<GameManager>().PlayersBestHandIs = Flush + " high flush";
                        PlayerhighestFlush = highestFlush;
                    }
                if(EnemyhighestFlush > PlayerhighestFlush)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = EnemyhighestFlush.ToString() + " high flush";
                GM.GetComponent<GameManager>().didYouWin = false;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
                if(EnemyhighestFlush < PlayerhighestFlush)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = PlayerhighestFlush.ToString() + " high flush";
                GM.GetComponent<GameManager>().didYouWin = true;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
                if(EnemyhighestFlush == PlayerhighestFlush)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = PlayerhighestFlush.ToString() + " high flush";
                GM.GetComponent<GameManager>().didYouDraw = true;
            }
            }else if (isStraight == true)
            {
                if(whichPlayer == "Enemy")
                    {
                        GM.GetComponent<GameManager>().EnemyBestHandIs = Straight + " High straight";
                EnemyhighestStraight = highestStraight;
                    }
                if (whichPlayer == "Player")
                    {
                        GM.GetComponent<GameManager>().PlayersBestHandIs = Straight + " High straight";
                PlayerhighestStraight = highestStraight;
                    }
                if(EnemyhighestStraight > PlayerhighestStraight)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = EnemyhighestStraight.ToString() + " High straight";
                GM.GetComponent<GameManager>().didYouWin = false;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if (EnemyhighestStraight < PlayerhighestStraight)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = PlayerhighestStraight.ToString() + " High straight";
                GM.GetComponent<GameManager>().didYouWin = true;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if(EnemyhighestStraight == PlayerhighestStraight)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = PlayerhighestStraight.ToString() + " High straight";
                GM.GetComponent<GameManager>().didYouDraw = true;
            }
        }else if (isSet == true)
            {
                if (whichPlayer == "Enemy")
                    {
                        GM.GetComponent<GameManager>().EnemyBestHandIs = "Set of: " + Set;
                EnemyhighestSet = highestSet;
                    }
                if (whichPlayer == "Player")
                    {
                        GM.GetComponent<GameManager>().PlayersBestHandIs = "Set of: " + Set;
                PlayerhighestSet = highestSet;
                    }
                if(EnemyhighestSet > PlayerhighestSet)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Set of: " + EnemyhighestSet.ToString();
                GM.GetComponent<GameManager>().didYouWin = false;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if (EnemyhighestSet < PlayerhighestSet)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Set of: " + PlayerhighestSet.ToString();
                GM.GetComponent<GameManager>().didYouWin = true;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if(EnemyhighestSet == PlayerhighestSet)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Set of: " + PlayerhighestSet.ToString();
                GM.GetComponent<GameManager>().didYouDraw = true;
            }
        }else if (isTwoPair == true)
            {
                if (whichPlayer == "Enemy")
                    {
                EnemyhighestTwoPair = highestTwoPair;
                EnemyHighestSecondPair = SecondHighestTwoPair;
                GM.GetComponent<GameManager>().EnemyBestHandIs = "Two pair of: " + EnemyhighestTwoPair.ToString() + " and " + SecondHighestTwoPair;
                
                    }
                if (whichPlayer == "Player")
                    {
                PlayerhighestTwoPair = highestTwoPair;
                PlayerHighestSecondPair = SecondHighestTwoPair;
                GM.GetComponent<GameManager>().PlayersBestHandIs = "Two pair of: " + PlayerhighestTwoPair.ToString() + " and " + SecondHighestTwoPair;
                
                    }
                if(EnemyhighestTwoPair > PlayerhighestTwoPair)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Two pair of: " + EnemyhighestTwoPair.ToString() + " and " + SecondHighestTwoPair;
                GM.GetComponent<GameManager>().didYouWin = false;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if (EnemyhighestTwoPair < PlayerhighestTwoPair)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Two pair of: " + PlayerhighestTwoPair.ToString() + " and " + SecondHighestTwoPair;
                GM.GetComponent<GameManager>().didYouWin = true;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if(EnemyhighestTwoPair == PlayerhighestTwoPair)
            {
                if(EnemyHighestSecondPair > PlayerHighestSecondPair)
                {
                    GM.GetComponent<GameManager>().RoundsBestHand = "Two pair of: " + EnemyhighestTwoPair.ToString() + " and " + SecondHighestTwoPair;
                    GM.GetComponent<GameManager>().didYouWin = false;
                    GM.GetComponent<GameManager>().didYouDraw = false;
                }
                if (EnemyHighestSecondPair < PlayerHighestSecondPair)
                {
                    GM.GetComponent<GameManager>().RoundsBestHand = "Two pair of: " + PlayerhighestTwoPair.ToString() + " and " + SecondHighestTwoPair;
                    GM.GetComponent<GameManager>().didYouWin = true;
                    GM.GetComponent<GameManager>().didYouDraw = false;
                }
                if(EnemyHighestSecondPair == PlayerHighestSecondPair)
                {
                    GM.GetComponent<GameManager>().RoundsBestHand = "Two pair of: " + PlayerhighestTwoPair.ToString() + " and " + SecondHighestTwoPair;
                    GM.GetComponent<GameManager>().didYouDraw = true;
                }
            }
        }else if (isPair == true)
            {
                if (whichPlayer == "Enemy")
                    {
                        GM.GetComponent<GameManager>().EnemyBestHandIs = "Pair of: " + Pair;
                EnemyhighestPair = highestPair;
                    }
                if (whichPlayer == "Player")
                    {
                        GM.GetComponent<GameManager>().PlayersBestHandIs = "Pair of: " + Pair;
                PlayerhighestPair = highestPair;
                    }
                if(EnemyhighestPair > PlayerhighestPair)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Pair of: " + EnemyhighestPair.ToString();
                GM.GetComponent<GameManager>().didYouWin = false;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if (EnemyhighestPair < PlayerhighestPair)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Pair of: " + PlayerhighestPair.ToString();
                GM.GetComponent<GameManager>().didYouWin = true;
                GM.GetComponent<GameManager>().didYouDraw = false;
            }
            if(EnemyhighestPair == PlayerhighestPair)
            {
                GM.GetComponent<GameManager>().RoundsBestHand = "Pair of: " + PlayerhighestPair.ToString();
                GM.GetComponent<GameManager>().didYouDraw = true;
            }
        }
        //}

        //if(whichPlayer == "Player")
        //{
        //    if (isStraightFlush == true)
        //    {
        //        GM.GetComponent<GameManager>().PlayersBestHandIs = StraightFlush + " High straight flush";
        //    }
        //    else if (isQuads == true)
        //    {
        //        GM.GetComponent<GameManager>().PlayersBestHandIs = "Four of a kind of: " + Quads;
        //    }
        //    else if (isFullHouse == true)
        //    {
        //        GM.GetComponent<GameManager>().PlayersBestHandIs = "Full House of: " + FullHouse;
        //    }
        //    else if (isFlush == true)
        //    {
        //        GM.GetComponent<GameManager>().PlayersBestHandIs = Flush + " high flush";
        //    }
        //    else if (isStraight == true)
        //    {
        //        GM.GetComponent<GameManager>().PlayersBestHandIs = Straight + " High straight";
        //    }
        //    else if (isSet == true)
        //    {
        //        GM.GetComponent<GameManager>().PlayersBestHandIs = "Set of: " + Set;
        //    }
        //    else if (isTwoPair == true)
        //    {
        //        GM.GetComponent<GameManager>().PlayersBestHandIs = "Two pair of: " + TwoPair;
        //    }
        //    else if (isPair == true)
        //    {
        //        GM.GetComponent<GameManager>().PlayersBestHandIs = "Pair of: " + Pair;
        //    }
        //}
    }

    public void ClearCurrentHandEval()
    {
        for(int i = 0; i < AllSevenCards.Length; i++)
        {
            AllSevenCards[i] = null;
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

}