using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Josh Fenlon
// Purpose:		Basic health script
// Namespace:	GG
//
//============================================================================//

namespace GG
{

    public class BasicHealthTest : MonoBehaviour
    {
        public bool isAlive = true;
        public bool isShielded = false;
        public float maxHealth = 3;
        public float health = 3;
        private int playerID = -1;
        public List<GameObject> destroyableParts = new List<GameObject>();

        void Start()
        {
            playerID = GetComponent<TurretRotation>().GetPlayerID();
        }

        public void controlParts(bool heal)
        {
            for (int a = 0; a < destroyableParts.Count; a++)
            {
                destroyableParts[a].SetActive(heal);
            }
        }

        public int takeDamage(float input)
        {
            if (isAlive && !isShielded)
            {
                health -= input;

                if (health == 0)
                {
                    Debug.Log("Dead");
                    isAlive = false;
                    if (playerID != -1)
                    {
                        if (GameObject.FindObjectOfType<TankGameManager>().FindNewSpawn(playerID))
                        {
                            isAlive = true;
                            health = maxHealth;
                        }
                    }
                    return 2;
                }
                return 1;
            }
            return 0;
        }

        public float GetHealth()
        {
            return health;
        }
    }

}