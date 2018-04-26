using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LT
{
    public class CharacterSelectionManager : MonoBehaviour
    {
        [SerializeField]
        List<PlayerCollection> collections;

        [SerializeField]
        List<CharacterSelection> characters;

        List<bool> playersReady;

        int numberOfPlayers;

        // Use this for initialization
        void Awake()
        {
            if (collections.Count < 4)
            {
                Debug.LogWarning("Too few collections in Character selection manager, needs at least 4");
            }
            foreach (var coll in collections)
            {
                coll.selected = false;
            }
            playersReady = new List<bool>();
            for (int i = 0; i < 4; i++)
            {
                playersReady.Add(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (playersReady.Count == 4)
            {
                foreach (var p in playersReady)
                {
                    if (p == false)
                    {
                        return;
                    }
                }
                StartGame();
            }
        }

        public PlayerCollection GetNextAvailableCharacter(PlayerCollection currentCollection)
        {
            if (currentCollection == null)
            {
                for (int i = 0; i < collections.Count; i++)
                {
                    if (!collections[i].selected)
                    {
                        collections[i].selected = true;
                        return collections[i];
                    }
                }
            }
            else
            {
                int index = collections.FindIndex(coll => coll == currentCollection);
                collections[index].selected = false;

                for (int i = index+1; i < collections.Count; i++)
                {
                    if (!collections[i].selected)
                    {
                        collections[i].selected = true;
                        return collections[i];
                    }
                }
                for (int i = 0; i < collections.Count; i++)
                {
                    if (!collections[i].selected)
                    {
                        collections[i].selected = true;
                        return collections[i];
                    }
                }
                collections[index].selected = true;
                return collections[index];
            }

            return null;
        }

        public PlayerCollection GetPreviousAvailableCharacter(PlayerCollection currentCollection)
        {
            if (currentCollection == null)
            {
                for (int i = collections.Count-1; i >= 0; i--)
                {
                    if (!collections[i].selected)
                    {
                        collections[i].selected = true;
                        return collections[i];
                    }
                }
            }
            else
            {
                int index = collections.FindIndex(coll => coll == currentCollection);
                collections[index].selected = false;

                for (int i = index - 1; i >= 0; i--)
                {
                    if (!collections[i].selected)
                    {
                        collections[i].selected = true;
                        return collections[i];
                    }
                }
                for (int i = collections.Count-1; i >= 0; i--)
                {
                    if (!collections[i].selected)
                    {
                        collections[i].selected = true;
                        return collections[i];
                    }
                }
                collections[index].selected = true;
                return collections[index];
            }

            return null;
        }

        public void nextCharacter()
        {
            
        }

        public void previousCharacter()
        {
            
        }

        public void SetReady(int playerID, bool ready)
        {
            playersReady[playerID] = ready;
        }

        public void StartGame()
        {
            List<PlayerCollection> selectedColl = new List<PlayerCollection>();
            foreach (var charSel in characters)
            {
                selectedColl.Add(charSel.currentCollection);
            }
            PlayerMaterialsManager.setPlayerCollections(selectedColl);
            SceneManager.LoadScene(3);
        }
    }
}
