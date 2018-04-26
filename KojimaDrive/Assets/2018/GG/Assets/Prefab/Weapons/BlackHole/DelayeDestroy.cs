using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayeDestroy : MonoBehaviour {

    int lifeTime = 4;
    void Start()
    {
        StartCoroutine(WaitThenDie());
    }
    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
