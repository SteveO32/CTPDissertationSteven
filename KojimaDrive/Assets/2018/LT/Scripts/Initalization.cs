using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{

    public class Initalization : MonoBehaviour
    {
        private static Initalization instance;
        [SerializeField] GameObject rewiredGameObject;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                rewiredGameObject.SetActive(true);
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.Log("Duplicate Initialisation Object, deleting");
                Destroy(this.gameObject);
            }
        }

    }

} // namespace LT
