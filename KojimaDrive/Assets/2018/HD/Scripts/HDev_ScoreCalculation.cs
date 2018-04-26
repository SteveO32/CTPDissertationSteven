using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Kojima Party - Team Hairy Devs 2018
// Author: Elliott Joseph Phillips
// Purpose: Score Calculation for the crazy taxi 
// Namespace: Hairy Devs
// Script Created: 17/02/2018 16:03
// Last Edited by Elliott Phillips 27/03/18 16:40

namespace HDev
{
    public class HDev_ScoreCalculation : MonoBehaviour
    {


        Text scoreText;

        [SerializeField]
        private int score;



        private void Start()
        {
            scoreText = GetComponent<Text>();
            score = 0;
        }


        // Update is called once per frame
        void Update()
        {
            scoreText.text = score.ToString();
        }

        public void CalculateScore(int stolenPackages , int ownPackages)
        {
            for (int i = 0; i < ownPackages; i++)
            {
                score += 100;
            }
            for (int i = 0; i < stolenPackages; i++)
            {
                score += 200;
            }
        }

        public void SetScore(int newScore)
        {
            score = newScore;
        }

        public int GetScore()
        {
            return score;
        }

        public void AddScore(int ScoreAdd)
        {
            score += ScoreAdd;
        }

        public void TakeScore(int ScoreTake)
        {
            score -= ScoreTake;
        }
    }
}