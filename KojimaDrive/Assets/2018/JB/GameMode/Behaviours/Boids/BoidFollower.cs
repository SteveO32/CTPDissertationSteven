using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public class BoidFollower : MonoBehaviour
{
    [SerializeField] GameObject followPoint;
    [SerializeField] GameObject startPoint;

    [SerializeField] float speed = 10.0f;


    private void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, followPoint.transform.position) > 1.0f)
        {
            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, followPoint.transform.position, step);
        }

        else
            GetNewPos();
    }


    private void GetNewPos()
    {
        // Using Magic numbers :'( need to tie this into screen space!
        float offsetZ = Random.Range(-70, 70);
        float offsetY = Random.Range(0, 20);

        Vector3 pos = startPoint.transform.position +
                (startPoint.transform.forward * offsetZ) + (startPoint.transform.up * offsetY);

        followPoint.transform.position = pos;
    }
}

}
