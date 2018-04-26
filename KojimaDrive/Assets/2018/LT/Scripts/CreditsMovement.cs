using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LT
{
    public class CreditsMovement : MonoBehaviour
    {

        [SerializeField]
        float speed;

        [SerializeField]
        float TimeForCredits;
        // Use this for initialization
        void Start()
        {
            StartCoroutine(loadScene());
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += Vector3.up * speed;
        }

        IEnumerator loadScene()
        {
            yield return new WaitForSeconds(TimeForCredits);
            SceneManager.LoadScene(1);
            yield return null;
        }
    }
}