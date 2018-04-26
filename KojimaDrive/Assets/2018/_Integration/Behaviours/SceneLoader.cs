using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KojimaParty
{

public class SceneLoader : MonoBehaviour
{
    [SerializeField] int sceneIndex;


    void Awake()
    {
        SceneManager.LoadScene(sceneIndex);
    }

}

} // namespace KojimaParty
