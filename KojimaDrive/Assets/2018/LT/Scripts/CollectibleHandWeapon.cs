using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    public class CollectibleHandWeapon : MonoBehaviour
    {
        [SerializeField]
        Transform handlePosition;

        [SerializeField]
        public bool taken = false;

        public Transform GetHandlePosition()
        {
            return handlePosition;
        }

        public void Collect(GameObject collectedBy)
        {
            taken = true;
            var club = GetComponentInChildren<Club>();
            if (club != null)
            {
                club.SetSwinger(collectedBy); 
            }
        }

        public void Drop()
        {
            StartCoroutine(timer());
        }

        IEnumerator timer()
        {
            yield return new WaitForSeconds(3.0f);
            taken = false;
            yield return null;
        }
    }
}
