using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LT
{
    [System.Serializable]
    public class MaterialSelector
    {
        public Renderer rend;
        public int materialIndex;
    }

    public class CharacterSelection : MonoBehaviour
    {
        public int PlayerIndex;

        public int selected;

        public CharacterSelectionManager manager;

        [HideInInspector]
        public PlayerCollection currentCollection;

        public GameObject humanoid;
        public GameObject tank;
        public GameObject car;
        public GameObject fireEngine;
        public GameObject plane;

        public Transform humanoidPos;
        public Transform tankPos;
        public Transform carPos;
        public Transform fireEnginePos;
        public Transform planePos;

        public List<MaterialSelector> mainMatRend;
        public List<MaterialSelector> secondMatRend;
        public List<MaterialSelector> accentsMatRend;
        public List<Text> texts;

        public GameObject confetti;

        public Rewired.Player input;

        public bool ready = false;

        public Text readyText;

        // Use this for initialization
        void Start()
        {
            currentCollection = manager.GetNextAvailableCharacter(null);
            UpdateGameObjects(currentCollection);
            Animate();
            input = Rewired.ReInput.players.GetPlayer(PlayerIndex);
        }

        // Update is called once per frame
        void Update()
        {
            if (input.GetButtonDown("NextChar"))
            {
                if (!ready)
                    NextCharacter();
            }
            if (input.GetButtonDown("PrevChar"))
            {
                if (!ready)
                    PreviousCharacter();
            }

            if (input.GetButtonDown("Ready"))
            {
                ready = !ready;
                if (ready)
                {
                    readyText.text = "Ready!";
                    readyText.GetComponentInChildren<Image>().enabled = false;
                }
                else
                {
                    readyText.text = "Select:";
                    readyText.GetComponentInChildren<Image>().enabled = true;
                }
                manager.SetReady(PlayerIndex, ready);
            }

            humanoid.GetComponentInChildren<HumanCharacterControl>().GetUp(true);
        }

        void NextCharacter()
        {
            currentCollection = manager.GetNextAvailableCharacter(currentCollection);
            UpdateGameObjects(currentCollection);
            Animate();
        }

        void PreviousCharacter()
        {
            currentCollection = manager.GetPreviousAvailableCharacter(currentCollection);
            UpdateGameObjects(currentCollection);
            Animate();
        }

        void UpdateGameObjects(PlayerCollection newCollection)
        {
            humanoid.GetComponent<HumanCharacterControl>().UpdateCharacter(newCollection.humanoidMesh, newCollection.humanoidMaterial);
            foreach (var r in mainMatRend)
            {
                var materials = r.rend.materials;
                materials[r.materialIndex] = newCollection.mainColor;
                r.rend.materials = materials;
            }

            foreach (var r in secondMatRend)
            {
                var materials = r.rend.materials;
                materials[r.materialIndex] = newCollection.secondaryColor;
                r.rend.materials = materials;
            }
            foreach (var r in accentsMatRend)
            {
                var materials = r.rend.materials;
                materials[r.materialIndex] = newCollection.accents;
                r.rend.materials = materials;
            }

            foreach (var t in texts)
            {
                t.color = newCollection.playerColor;
            }
        }

        void Animate()
        {
            //Smoke Particles
            Destroy(Instantiate(confetti, humanoidPos.position + new Vector3(0, 0, -1.0f), Quaternion.identity), 2.0f);
            //Pirouette
            humanoid.GetComponent<HumanCharacterControl>().Pirouette();
            
            //Move objects UP
            humanoid.transform.position = humanoidPos.position;
            tank.transform.position = tankPos.position;
            plane.transform.position = planePos.position;
            fireEngine.transform.position = fireEnginePos.position;
            car.transform.position = carPos.position;


        }
    }
}
