using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCamera : MonoBehaviour
{
    [SerializeField] float follow_lerp_speed = 20;
    [SerializeField] float fov_lerp_speed = 5;
    public Vector3 follow_offset;

    [Header("References")]
    [SerializeField] Camera cam;

    private float target_fov;
    private float target_follow_speed;

    [SerializeField] Transform follow_target;
    [SerializeField] Transform lookat_target;


    public void SetFollowSpeed(float _speed)
    {
        target_follow_speed = _speed;
    }


    public void SetFOV(float _fov)
    {
        target_fov = _fov;
    }


    public void SetTargets(Transform _follow, Transform _lookat)
    {
        follow_target = _follow;
        lookat_target = _lookat;
    }


    void Start()
    {
        target_follow_speed = follow_lerp_speed;
        target_fov = cam.fieldOfView;
    }

    
    void Update()
    {
        follow_lerp_speed = Mathf.Lerp(follow_lerp_speed, target_follow_speed, 15 * Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, target_fov, fov_lerp_speed * Time.deltaTime);
    }


    void LateUpdate()
    {
        Follow();
    }


    void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, follow_target.position + follow_offset, follow_lerp_speed * Time.deltaTime);
        transform.LookAt(lookat_target.position);
    }

}
