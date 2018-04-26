//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyBehaviour : MonoBehaviour
//{

//    //GameObject Eval;
//    GameObject HUD;
//    GameObject HUSPF;
//    GameObject HUSF;
//    GameObject HUST;
//    GameObject HUSR;

//    [SerializeField]
//    private GameObject[] myHand;


//    // Use this for initialization
//    void Start()
//    {
//        //Eval = GameObject.FindGameObjectWithTag("HandEval");
//        HUD = GameObject.FindGameObjectWithTag("HeadsUp");
//        HUSPF = GameObject.FindGameObjectWithTag("HUSPF");
//        HUSF = GameObject.FindGameObjectWithTag("HUSF");
//        HUST = GameObject.FindGameObjectWithTag("HUST");
//        HUSR = GameObject.FindGameObjectWithTag("HUSR");
//    }

//    void EnemyAI()
//    {
//        myHand[0] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[1];
//        myHand[1] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[3];
//        myHand[2] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[5];
//        myHand[3] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[6];
//        myHand[4] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[7];
//        myHand[5] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[9];
//        myHand[6] = HUD.GetComponent<HeadsUpDeal>().CurrentCards[11];


//        if (HUSPF.GetComponent<HeadsUpStart>().isPreFlop == true)
//        {
//            Debug.Log("preflop");
//        }
//        if (HUSF.GetComponent<HeadsUpStart>().isFlop == true)
//        {
//            Debug.Log("flop");
//        }
//        if (HUST.GetComponent<HeadsUpStart>().isTurn == true)
//        {
//            Debug.Log("turn");
//        }
//        if (HUSR.GetComponent<HeadsUpStart>().isRiver == true)
//        {
//            Debug.Log("river");
//        }
//    }



//    // Update is called once per frame
//    void Update()
//    {


//    }
//}
