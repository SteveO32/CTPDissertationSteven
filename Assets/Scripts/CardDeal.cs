//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CardDeal : MonoBehaviour {

//    public GameObject[] TileHolders;
//    public GameObject[] Cards;
//    public GameObject[] CurrentCards;

//	// Use this for initialization
//	void Start ()
//    {
//        for (int i = 0; i < Cards.Length; i++)
//        {
//            CurrentCards[i] = (GameObject)GameObject.Instantiate(Cards[i], TileHolders[i].transform.position, Cards[i].transform.rotation);
//        }
//    }

//    void ShuffleDeck()
//    {
//        for(int i = 0; i < Cards.Length ; i++)
//        {
//            int rnd = Random.Range(0, Cards.Length );

//            GameObject temp = Cards[i];

//            Cards[i] = Cards[rnd];

//            Cards[rnd] = temp;
//        }
//    }
//	// Update is called once per frame
//	void Update ()
//    {
//        if(Input.GetKeyDown(KeyCode.Space))
//        {
//            for(int i = 0; i < Cards.Length; i++)
//            {
//                Destroy(CurrentCards[i]);
//            }
//            for (int k = 0; k < 7; k++)
//            {
//                ShuffleDeck();
//                Debug.Log("Shuffled Deck");
//            }
//            for (int j = 0; j < Cards.Length; j++)
//            {
//                CurrentCards[j] = (GameObject)GameObject.Instantiate(Cards[j], TileHolders[j].transform.position, Cards[j].transform.rotation);
//            }
//        }
		
//	}
//}
