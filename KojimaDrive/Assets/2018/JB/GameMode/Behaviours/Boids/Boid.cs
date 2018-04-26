using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public class Boid : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 acceleration;


    public Vector3 Velocity()
    {
        return velocity;
    }


    public void Run(List<Boid> _boids, GameObject _targetObject, BoidManager.BoidData _data)
    {
        Flock(_boids, _targetObject, _data);

        UpdateBoid(_data);
    }


    private void Flock(List<Boid> _boids, GameObject _targetObject, BoidManager.BoidData _data)
    {
        Vector3 sep = JB.BoidBehaviours.Seperate(this, _boids, _data);
        Vector3 ali = JB.BoidBehaviours.Align(this, _boids, _data);
        Vector3 coh = JB.BoidBehaviours.Cohesion(this, _boids, _data);
        Vector3 ff  = JB.BoidBehaviours.FightFlight(this, _targetObject, _data);

        sep = (sep * _data.sepWeight);
        ali = (ali * _data.aliWeight);
        coh = (coh * _data.cohWeight);
        ff  = (ff  * _data.ffWeight);

        ApplyForce(sep);
        ApplyForce(ali);
        ApplyForce(coh);
        ApplyForce(ff);
    }


    private void ApplyForce(Vector3 _force)
    {
        acceleration += _force;
    }


    private void UpdateBoid(BoidManager.BoidData _data)
    {
        // update velocity
        velocity += acceleration;

        // limit speed
        velocity = Vector3.ClampMagnitude(velocity, _data.maxSpeed);

        transform.position = (transform.position + velocity * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(velocity);

        // reset acceleration to 0 each cycle
        acceleration = Vector3.zero;
    }
}

}
