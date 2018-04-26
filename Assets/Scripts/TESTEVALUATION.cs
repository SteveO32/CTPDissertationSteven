//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TESTEVALUATION : MonoBehaviour
//{

//    [SerializeField]
//    private GameObject[] PlayersSevenCards;
//    [SerializeField]
//    private GameObject[] EnemiesSevenCards;

//    GameObject HUD;
//    GameObject GM;

//    string StraightFlush;
//    string Quads;
//    string FullHouse;
//    string Flush;
//    string Straight;
//    string Set;
//    string TwoPair;
//    string Pair;
//    string HighCard;

//    bool isStraightFlush = false;
//    bool isQuads = false;
//    bool isFullHouse = false;
//    bool isFlush = false;
//    bool isStraight = false;
//    bool isSet = false;
//    bool isTwoPair = false;
//    bool isPair = false;
//    bool isHighCard = false;


//    // Use this for initialization
//    void Start()
//    {
//        HUD = GameObject.FindGameObjectWithTag("HeadsUp");
//        GM = GameObject.FindGameObjectWithTag("GameManager");
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }


//    public void GrabCards(string name)
//    {
//        if (name == "Player")
//        {
//            PlayersSevenCards[0] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[0];
//            PlayersSevenCards[1] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[2];
//            PlayersSevenCards[2] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[5];
//            PlayersSevenCards[3] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[6];
//            PlayersSevenCards[4] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[7];
//            PlayersSevenCards[5] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[9];
//            PlayersSevenCards[6] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[11];
//        }
//        if (name == "Enemy")
//        {
//            EnemiesSevenCards[0] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[1];
//            EnemiesSevenCards[1] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[3];
//            EnemiesSevenCards[2] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[5];
//            EnemiesSevenCards[3] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[6];
//            EnemiesSevenCards[4] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[7];
//            EnemiesSevenCards[5] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[9];
//            EnemiesSevenCards[6] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[11];
//        }
//    }


//    public void TESTEVAL()
//    {
//        int a = 0;

//        switch (a)
//        {
//            case 0:
//                {

//                    break;
//                }
//            case 1:
//                {

//                    break;
//                }
//            case 2:
//                {

//                    break;
//                }
//            case 3:
//                {

//                    break;
//                }
//            case 4:
//                {

//                    break;
//                }
//            case 5:
//                {

//                    break;
//                }
//        }
//    }


//}
