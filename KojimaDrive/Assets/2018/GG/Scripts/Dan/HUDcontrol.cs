using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Dan Rowland
// Purpose:		Placeholder HUD for tanks
// Namespace:	GG
//
//============================================================================//

namespace GG
{
    public class HUDcontrol : MonoBehaviour
    {

        public Text Timer;
		public Text Score;
        public GameObject Tank;
        private BasicHealthTest healthTest;
        RawImage Healthbar = null;
        public Texture FullHp;
        public Texture HalfHp;
        public Texture LowHp;

        void Start()
        {
            Healthbar = gameObject.GetComponentInChildren<RawImage>();
            Timer = gameObject.GetComponentInChildren<Text>();
            healthTest = Tank.GetComponent<BasicHealthTest>();
        }

        void Update()
        {
            float currentHealth = healthTest.GetHealth();

            if (currentHealth == 3)
            {
                Healthbar.texture = FullHp;
            }

            if (currentHealth == 2)
            {
                Healthbar.texture = HalfHp;
            }

            if (currentHealth == 1)
            {
                Healthbar.texture = LowHp;
            }
        }
    }
}