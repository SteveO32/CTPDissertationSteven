using System.Collections;
using System.Collections.Generic;
using JB;
using UnityEngine;

public class TurretJBProjectile : MonoBehaviour
{
    public Transform Target { get; set; }

    private float speed = 150;

    private Rigidbody myRigidbody;
    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(transform.position, Target.position);
        if (distance > 350)
            distance = 350;

        var mod = JHelper.Remap(distance, 0, 350, 1, 5);
        transform.localScale = Vector3.one * mod;
    }

    void FixedUpdate()
    {
        Transform cpy = transform;
        cpy.LookAt(Target);
        myRigidbody.MoveRotation(cpy.rotation);
        cpy.position += cpy.forward * speed * Time.fixedDeltaTime;
        myRigidbody.MovePosition(cpy.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.GetComponentInParent<DuckHuntBoat>())
        {
            Destroy(gameObject);
        }
    }
}