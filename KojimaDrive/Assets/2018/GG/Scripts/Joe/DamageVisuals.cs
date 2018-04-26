using UnityEngine;
using System.Collections;
using Rewired;


//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Joe Plant
// Purpose:		Visual Damage for the tanks from the particles
// Namespace:	GG
//
//============================================================================//

namespace GG
{
    public class DamageVisuals : MonoBehaviour
    {
        public BasicHealthTest hp;

        public ParticleSystem Smoke;
        public ParticleSystem Fire;

        void Update()
        {
            if (hp.health < 3)
            {
                Smoke.Play();
            }
            if (hp.health < 2)
            {
                Fire.Play();
            }
        
            else if (hp.health == 3)
            {
                Smoke.Stop();
                Fire.Stop();
            }
        }
    }
}