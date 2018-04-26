using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LT
{
    public class ButtonManager : MonoBehaviour
    {

        public Button resume_button;
        public Button backToIsland_button;
        public Button exitGame_Button;

        // Use this for initialization
        void Start()
        {
            resume_button.onClick.AddListener(ResumeGame);
            backToIsland_button.onClick.AddListener(BackToIsland);
            exitGame_Button.onClick.AddListener(ExitGame);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ResumeGame()
        {
            Menu.instance.ChangeMenuState();
        }
        private void BackToIsland()
        {

        }
        private void ExitGame()
        {

        }

    }

}