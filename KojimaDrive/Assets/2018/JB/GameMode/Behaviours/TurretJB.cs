using System.Collections;
using System.Collections.Generic;
using JB;
using UnityEngine;

public class TurretJB : MonoBehaviour
{
    public Transform TurretTransform;
    public Transform SpawnPoint;
    public GameObject projectile;
    public Vector3 offset;
    private Vector3 forward;

    public bool ready;
    // Use this for initialization
    void Start()
    {
        forward = SpawnPoint.transform.forward;
    }

    void OndrawgizmosSelected()
    {
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawLine(TurretTransform.transform.position, TurretTransform.transform.position +);
    }

    // Update is called once per frame
    void Update()
    {
        Transform temp = JBSceneRefs.boat.transform;
        temp.position = JBSceneRefs.boat.transform.position + offset;
        TurretTransform.LookAt(temp);


       
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(TurretTransform.transform.position, TurretTransform.transform.forward, out hit))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ship"))
            {
                ready = true;
            }
        }

        if (ready && !transform.GetChild(0).GetComponent<MeshRenderer>().isVisible)
        {
            ready = false;
        }
    }

    public void Shoot(Transform target)
    {
        GameObject clone = (GameObject)Instantiate(projectile, SpawnPoint.position, TurretTransform.rotation);
        var prj = clone.GetComponent<TurretJBProjectile>();
        prj.Target = target;
    }
}