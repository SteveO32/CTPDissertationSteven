using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FH
{
public class Missile : MonoBehaviour
{
    public GameObject m_explosionPrefab;


    [HideInInspector]
    public Vector3 Forward;
    [HideInInspector]
    public Vector3 Thrust;
    [SerializeField]
    private float m_speed = 10f;

    private Rigidbody m_rb;


    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(DestroyAfterSeconds(this.gameObject));
    }


    private void FixedUpdate()
    {
        var power = Forward * m_speed * Time.fixedDeltaTime;
        m_rb.velocity += power;
    }


    // Destory on collision with everything.
    private void OnCollisionEnter(Collision other)
    {
        Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }


    private static IEnumerator DestroyAfterSeconds(GameObject obj)
    {
        yield return new WaitForSeconds(15f);
        Destroy(obj);
    }
}
}
