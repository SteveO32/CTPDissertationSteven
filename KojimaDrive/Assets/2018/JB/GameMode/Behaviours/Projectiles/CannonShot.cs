﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public class CannonShot : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] int damage = 200;
    [SerializeField] float starting_force = 200;
    [SerializeField] float explosion_radius = 3;

    [Space]
    [SerializeField] GameObject explosion_prefab;
    [SerializeField] GameObject sploosh_prefab;
    
    [Space]
    [SerializeField] float hit_shake_strength;
    [SerializeField] float hit_shake_duration;

    [Space]
    [SerializeField] float water_hit_shake_strength;
    [SerializeField] float water_hit_shake_duration;

    [Space]
    [SerializeField] AudioClip hit_sound;
    [SerializeField] AudioClip water_hit_sound;
    
    [Header("References")]
    [SerializeField] Rigidbody rigid_body;
    [SerializeField] BuoyantObject boy;

    private int water_layer;
    private List<LifeForce> affected_entities = new List<LifeForce>();

    [SerializeField] bool ignore_water;


    void Start()
    {
        rigid_body.AddForce(transform.forward * starting_force, ForceMode.Impulse);
    }


    void Update()
    {
        if (boy.in_water && !ignore_water)
        {
            if (sploosh_prefab != null)
                Instantiate(sploosh_prefab, transform.position, Quaternion.identity);

            //CameraShake.Shake(water_hit_shake_strength, water_hit_shake_duration);
            AudioManager.PlayOneShot(water_hit_sound);

            Destroy(this.gameObject);
        }
    }


    void OnCollisionEnter(Collision _other)
    {
        ManualDetonation();
    }


    void DamageAllInSphere()
    {
        var elems = Physics.OverlapSphere(transform.position, explosion_radius);

        foreach (var elem in elems)
        {
            LifeForce life = elem.GetComponent<LifeForce>();

            if (life == null && elem.transform.parent != null)
                life = elem.transform.GetComponentInParent<LifeForce>();

            if (life == null || affected_entities.Contains(life))
                continue;

            life.Damage(damage);
            affected_entities.Add(life);
        }
    }

    
    public void ManualDetonation()
    {
        if (explosion_prefab != null)
            Instantiate(explosion_prefab, transform.position, Quaternion.identity);

        //CameraShake.Shake(hit_shake_strength, hit_shake_duration);
        AudioManager.PlayOneShot(hit_sound);

        DamageAllInSphere();

        Destroy(this.gameObject);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosion_radius);
    }

}

}
