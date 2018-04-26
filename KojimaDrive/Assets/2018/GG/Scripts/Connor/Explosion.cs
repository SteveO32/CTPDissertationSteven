using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Connor Rhone
// Purpose:		Scales the explosion prefab and then removes it
// Namespace:	GG
//
//============================================================================//

namespace GG
{
    public class Explosion : MonoBehaviour
    {

        public float lifeSpan = 1.0f;
        public float maxSize = 10.0f;

        float lifeTime = 0.0f;

        bool activated = false;

        //controls whether or not tanks can shoot themselves
        bool friendlyFire = true;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            lifeTime += Time.deltaTime;

            float percentage = (lifeSpan - (lifeTime * 4.0f)) / (lifeSpan * 4.0f);

            transform.localScale = new Vector3(percentage * maxSize, percentage * maxSize, percentage * maxSize);

            if(lifeTime > lifeSpan)
            {
                Destroy(gameObject);
            }
        }

        public void Activate(int playerID, float damageValue, BasicHealthTest parent)
        {
            if (activated) return;
            activated = true;

            List<BasicHealthTest> alreadyHit = new List<BasicHealthTest>();

            foreach(Collider mightBeTank in Physics.OverlapSphere(transform.position, maxSize / 3.0f))
            {
                BasicHealthTest tank = mightBeTank.GetComponentInParent<BasicHealthTest>();
                if(tank != null && !alreadyHit.Contains(tank) && (tank != parent || friendlyFire))
                {
                    if (tank != parent)
                    {
                        alreadyHit.Add(tank);
                        int damageDone = tank.takeDamage(damageValue);
                        TankGameManager tGM = GameObject.FindObjectOfType<TankGameManager>();
                        switch (damageDone)
                        {
                            case 1:
                                tGM.addDamageToPD(damageValue, playerID);
                                break;
                            case 2:
                                if (tank == parent)
                                {
                                    tGM.addSuicideToPD(playerID);
                                }
                                else
                                {
                                    tGM.addKillToPD(playerID);
                                }
                                break;
                        }
                    }
                }
            }
        }

    }
}