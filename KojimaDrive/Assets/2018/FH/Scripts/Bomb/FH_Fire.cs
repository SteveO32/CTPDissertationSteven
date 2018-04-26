using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FH
{
    public class FH_Fire : MonoBehaviour
    {
        private ParticleSystem m_particleSystem;
        private bool m_updateScore = false;


        private void Awake()
        {
            m_particleSystem = GetComponent<ParticleSystem>();
            // TODO: Error check
        }




        private void OnTriggerEnter(Collider other)
        {
            if(other.tag != "Water")
                return;

            var emission = m_particleSystem.emission;
            emission.rateOverTime = emission.rateOverTime.constant - FireManager.DecreaseRate;

            if(emission.rateOverTime.constant <= -50f)
            {
                ScoreCheck();
                StartCoroutine(CountDown(a =>
               {
                   if(a)
                       Destroy(this.gameObject);
               }));
            }
        }

        /// <summary>
        /// Destroy fire after x seconds. 
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        private IEnumerator CountDown(System.Action<bool> flag)
        {
            flag(false);
            yield return new WaitForSeconds(10f);
            flag(true);
        }


        // TODO: Turn this and the other similar call in the BombCast script, 
        //          into events instead of being coupled like this. 
        private void ScoreCheck()
        {
            if(!m_updateScore)
            {
                m_updateScore = true;
                FH_GameManager.Score--;
            }
        }
    }
}
