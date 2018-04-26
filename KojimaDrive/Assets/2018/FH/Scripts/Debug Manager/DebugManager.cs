using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FH
{
    public class DebugManager : MonoBehaviour
    {
        [SerializeField]
        private List<GameAction> cheatList = new List<GameAction>();
        [SerializeField]
        private const float MAX_TIME = 1f;
        [SerializeField]
        private float timer = 0f;

        private List<GameAction> resetAircraft = new List<GameAction>(new GameAction[] { GameAction.A_Down, GameAction.B_Down });


        private void OnEnable()
        {
            InputManager.inputDetected += LogInput;
        }


        private void OnDisable()
        {
            InputManager.inputDetected -= LogInput;
        }


        private void Update()
        {
            if(timer < MAX_TIME)
            {
                timer += Time.deltaTime;
            }
            else
            {
                CheckForSequence(resetAircraft);
                cheatList.Clear();
                timer = 0f;
            }
        }



        private void LogInput(GameAction gameAction, float value, ControllerID ID)
        {
            // Lock whilst searching.
            if(timer < MAX_TIME)
            {
                switch(gameAction)
                {
                    case GameAction.RB_Held:
                        cheatList.Add(gameAction);
                        break;
                    case GameAction.LB_Held:
                        cheatList.Add(gameAction);
                        break;
                    case GameAction.A_Down:
                        cheatList.Add(gameAction);
                        break;
                    case GameAction.B_Down:
                        cheatList.Add(gameAction);
                        break;
                }
            }
        }


        // S - search
        // W - sought


        private void CheckForSequence(List<GameAction> sequence)
        {
            List<int> listIDs = new List<int>();
            int currMatch = 0;

            if(cheatList.Count <= 0)
                return;

            for(int i = 0; i < cheatList.Count; i++)
            {
                if(currMatch > sequence.Count)
                    break;
                if((currMatch + i) >= sequence.Count)
                    break;

                if(sequence[i + currMatch] == cheatList[i])
                {
                    listIDs.Add(i);
                    currMatch++;
                }
                else
                {
                    listIDs.Clear();
                    currMatch = 0;
                }
            }



        }
    }
}