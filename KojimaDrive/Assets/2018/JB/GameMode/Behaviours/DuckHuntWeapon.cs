using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public class DuckHuntWeapon : MonoBehaviour
{
    public bool can_shoot { get { return Time.time >= next_shot_timestamp; } }

    [Header("Parameters")]
    [SerializeField] float shoot_delay = 2;
    [SerializeField] float shot_spread = 0.025f;

    [Header("References")]
    [SerializeField] Transform gun_transform;
    [SerializeField] Transform shoot_point;
    [SerializeField] GameObject projectile_prefab;
    [SerializeField] GameObject ejection_prefab;
    [SerializeField] AudioClip shot_sound;

    [Header("Debug")]
    [SerializeField] bool debug_shoot;

    private float next_shot_timestamp;
    private int controllingPlayerID;


    public void Init(int _controllingPlayerID)
    {
        controllingPlayerID = _controllingPlayerID;
    }


    public void LookAt(Vector3 _lookat)
    {
        gun_transform.LookAt(_lookat);
    }


    public void Shoot()
    {
        next_shot_timestamp = Time.time + shoot_delay;

        Vector3 shot_forward = shoot_point.forward;
        Vector3 variance = new Vector3(
            Random.Range(-shot_spread, shot_spread),
            Random.Range(-shot_spread, shot_spread),
            Random.Range(-shot_spread, shot_spread));
        shot_forward += variance;

        var rot = Quaternion.LookRotation(shot_forward);

        GameObject shot = Instantiate(projectile_prefab, shoot_point.transform.position, rot);
        shot.GetComponent<TurretShot>().Init(controllingPlayerID);

        Instantiate(ejection_prefab, shoot_point.position, rot);

        AudioManager.PlayOneShot(shot_sound);
    }


    void Update()
    {
        if (debug_shoot)
        {
            debug_shoot = false;
            Shoot();
        }
    }

}

} // namespace JB
