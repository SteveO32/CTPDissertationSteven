using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/*
 * Kojima Party - Hairy Devs 2018
 * Author: Owen Jackson
 * Purpose: Temp class for the scores at the end of a round
 * Namespace: HDev
 * Last Edited: Owen Jackson @ 13/03/2018
*/

namespace HDev
{
    public class HDev_Scoreboard : MonoBehaviour
    {
        public Text[] positionTexts;

        Dictionary<int, string> positionStrings;

        private void OnEnable()
        {
            if (positionTexts == null)
            {
                positionTexts = GetComponentsInChildren<Text>();
            }

            if (positionStrings == null)
            {
                positionStrings = new Dictionary<int, string>()
                {
                    {1, "st" },
                    {2, "nd" },
                    {3, "rd" },
                    {4, "th" }
                };
            }
        }        
        
        public void SetPlayerScores(ScoreBoardInfo[] scoresInfo)
        {
            //disable the score texts then only re-enable for the number of players
            for(int i = 0; i < positionTexts.Length; i++)
            {
                positionTexts[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < scoresInfo.Length; i++)
            {
                Debug.Log("positiontexts length: " + positionTexts.Length);
                Debug.Log("current index: " + i);
                positionTexts[i].gameObject.SetActive(true);
                positionTexts[i].text = scoresInfo[i].Position.ToString() + positionStrings[scoresInfo[i].Position] + " P" + (scoresInfo[i].PlayerID + 1) + " " + scoresInfo[i].Score;
            }
        }

        IEnumerator Stagger(float waitFor)
        {
            yield return new WaitForSeconds(waitFor);
        }
    }
}