using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to handle the instantiation of gameobjects, cards for the game
public class HeadsUpDeal : GameManager
{
    //to access other objects and classes, public, assigned in editor
    public GameObject[] TileHolders;
    public GameObject[] Cards;
    public GameObject[] CurrentCards;
    public GameObject[] CardCover;
    public GameObject[] CurrentCardCover;

    //function to shuffle the deck using a modified
    //fisher-yates algorithm as a way of swapping through
    //each element in the card array, switching elements
    //going through each elemtn and swapping with a different 
    //randomised element
    public void ShuffleDeck()
    {
        for (int k = 0; k < 13; k++)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                int rnd = Random.Range(0, Cards.Length);

                GameObject temp = Cards[i];

                Cards[i] = Cards[rnd];

                Cards[rnd] = temp;
            }
        }
    }
    //function that instantiates the appropriate card objects in the right place
    //location from placeholders
    public void DealHoleCards()
    {
        for (int j = 0; j < 4; j++)
        {
            CurrentCards[j] = (GameObject)GameObject.Instantiate(Cards[j], TileHolders[j].transform.position, Cards[j].transform.rotation);
        }
    }
    //function that instantiates the appropriate card objects in the right place
    //location from placeholders
    public void DealFlop()
    {
        for (int j = 4; j < 8; j++)
        {
            if(j == 4)
            {
                CurrentCardCover[0] = (GameObject)GameObject.Instantiate(CardCover[0], TileHolders[j].transform.position + new Vector3(1.5f, 0.0f, -0.2f), Cards[j].transform.rotation);
            }
            if(j > 4 && j < 8)
            {
                CurrentCards[j] = (GameObject)GameObject.Instantiate(Cards[j], TileHolders[j].transform.position + new Vector3(-1.0f, 0.0f, -0.2f), Cards[j].transform.rotation);
            }
            
        }
    }
    //function that instantiates the appropriate card objects in the right place
    //location from placeholders
    public void DealTurn()
    {
        for (int j = 8; j <= 9; j++)
        {
            if (j == 8)
            {
                CurrentCardCover[1] = (GameObject)GameObject.Instantiate(CardCover[1], TileHolders[j].transform.position + new Vector3(0.75f, 0.0f, -0.2f), Cards[j].transform.rotation);
            }
            if(j > 8 && j == 9)
            {
                CurrentCards[j] = (GameObject)GameObject.Instantiate(Cards[j], TileHolders[j].transform.position + new Vector3(-1.0f, 0.0f, -0.2f), Cards[j].transform.rotation);
            }
            
        }
    }
    //function that instantiates the appropriate card objects in the right place
    //location from placeholders
    public void DealRiver()
    {
        for (int j = 10; j <= 11; j++)
        {
            if (j == 10)
            {
                CurrentCardCover[2] = (GameObject)GameObject.Instantiate(CardCover[2], TileHolders[j].transform.position + new Vector3(0.0f, 0.0f, -0.2f), Cards[j].transform.rotation);
            }
            if(j > 10 && j == 11)
            {
                CurrentCards[j] = (GameObject)GameObject.Instantiate(Cards[j], TileHolders[j].transform.position + new Vector3(-1.0f, 0.0f, -0.2f), Cards[j].transform.rotation);
            }
           
        }
    }
    //function that destroys the cloned objects that were instantiated
    //to reset for the next hands
    public void DestroyCards()
    {
        for (int i = 0; i < TileHolders.Length; i++)
        {
            Destroy(CurrentCards[i]);
        }
        for(int j = 0; j < CardCover.Length; j++)
        {
            Destroy(CurrentCardCover[j]);
        }
    }
}
