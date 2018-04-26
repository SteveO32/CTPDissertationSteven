using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LT
{
    public class TempSelectionSceneController : MonoBehaviour
    {

        public void loadscene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }


        public void loadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}