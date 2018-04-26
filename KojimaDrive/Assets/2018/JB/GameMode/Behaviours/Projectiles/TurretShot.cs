using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public class TurretShot : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float travel_speed;
    [SerializeField] GameObject ricochet_prefab;
    [SerializeField] int damage = 5;
    [SerializeField] LayerMask hit_layers;

    private int playerID;


    public void Init(int _playerID)
    {
        playerID = _playerID;
    }


    void FixedUpdate()
    {
        Vector3 prev_pos = transform.position;

        transform.position += transform.forward * travel_speed * Time.fixedDeltaTime;

        Vector3 current_pos = transform.position;

        HitCheck(prev_pos, current_pos);
    }


    void HitCheck(Vector3 _prev_pos, Vector3 _current_pos)
    {
        Vector3 diff = (_current_pos - _prev_pos);
        RaycastHit hit;
        Physics.Raycast(_prev_pos, diff.normalized, out hit, diff.magnitude, hit_layers);

        if (hit.collider == null)
            return;

        var target = hit.collider.GetComponent<JB.Target>();

        if (target != null)
            target.TargetHit(playerID, gameObject.transform.parent, damage);

        Instantiate(ricochet_prefab, hit.point, Quaternion.LookRotation(hit.normal));

        AudioManager.PlayOneShot("ricochet");

        Destroy(this.gameObject);
    }

}

} // namespace JB
