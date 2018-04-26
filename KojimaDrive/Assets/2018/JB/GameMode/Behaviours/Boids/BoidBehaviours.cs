using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public static class BoidBehaviours
{
    // Seperation
    // Method checks for nearby boids and steers away
    public static Vector3 Seperate(Boid _boid, List<Boid> _boids, BoidManager.BoidData _data)
    {
        Vector3 steer = Vector3.zero;

        int count = 0;

        // check through every other boid
        for (int i = 0; i < _boids.Count; i++)
        {
            float d = Vector3.Distance(_boid.transform.position, _boids[i].transform.position);
            // if boid is a neighbour
            if (d > 0 && d < _data.desiredSeperation)
            {
                // Calculate vector pointing away from neighbour
                Vector3 diff = (_boid.transform.position - _boids[i].transform.position);

                diff.Normalize();

                diff = (diff / d); // Weight by distance

                steer = (steer + diff);

                count++;
            }
        }

        // Average -- divided by how many
        if (count > 0)
        {
            steer = (steer / count);
        }

        // as long as the vector is greater than 0
        if (steer != Vector3.zero)  // if(steer.mag() > 0)
        {
            //steer.setMag (maxSpeed);
            steer = Vector3.ClampMagnitude(steer, _data.maxSpeed);

            // implement Reynolds: steering = desired - velocity
            steer.Normalize();

            steer = (steer * _data.maxSpeed);

            steer = (steer - _boid.Velocity());

            //steer.limit (maxForce);
            steer = Vector3.ClampMagnitude(steer, _data.maxForce);
        }
        return steer;
    }


    // Alignment
    // For every nearby boid in the system, calculate the average velocity
    public static Vector3 Align(Boid _boid, List<Boid> _boids, BoidManager.BoidData _data)
    {
        // The position the boid want to be
        Vector3 sum = Vector3.zero;

        int count = 0;

        // check through every other boid
        for (int i = 0; i < _boids.Count; i++)
        {
            float d = Vector3.Distance(_boid.transform.position, _boids[i].transform.position);

            // if boid is a neighbour
            if ((d > 0) && (d < _data.neighbourDistance))
            {
                sum = (sum + _boids[i].Velocity());

                count++;
            }
        }

        if (count > 0)
        {
            sum = (sum / count);
            //sum.setMag (maxSpeed);
            sum = Vector3.ClampMagnitude(sum, _data.maxSpeed);

            // Implement Reynolds: Steering = desired - velocity
            sum.Normalize();

            sum = (sum * _data.maxSpeed);

            Vector3 steer = (sum - _boid.Velocity());
            
            //steer.limit (maxForce);
            steer = Vector3.ClampMagnitude(steer, _data.maxForce);
            return steer;
        }
        else
            return new Vector3();
    }


    // Cohesion
    // For the average position (I.E. center) of all nearby boids, calculate steering
    // vector towards that position
    public static Vector3 Cohesion(Boid _boid, List<Boid> boids, BoidManager.BoidData _data)
    {
        Vector3 sum = Vector3.zero; // start with empty vector to accumulate all positions
        int count = 0;

        for (int i = 0; i < boids.Count; i++)
        {
            float d = Vector3.Distance(_boid.transform.position, boids[i].transform.position);

            if ((d > 0) && (d < _data.neighbourDistance))
            {
                sum = (sum + boids[i].transform.position); // add position
                count++;
            }
        }

        if (count > 0)
        {
            sum = (sum / count);

            return Seek(_boid, sum, _data); // Steer towards the position
        }

        else
        {
            return new Vector3();
        }
    }


    // Fight/Flight
    // Method checks for nearby predator and steers away
    public static Vector3 FightFlight (Boid _boid, GameObject _targetObject, BoidManager.BoidData _data)
    {
        Vector3 steer = Vector3.zero;

	    float d = Vector3.Distance(_boid.transform.position, _targetObject.transform.position);

	    Vector3 diff = (_boid.transform.position - (_targetObject.transform.position));

		diff = Vector3.Normalize(diff);

        diff /= d; // Weight by distance

        steer += diff;

	    // As long as the vector is greater than 0
	    if (steer != Vector3.zero)
	    {
            steer = Vector3.ClampMagnitude(steer, _data.maxSpeed);

		    steer = Vector3.Normalize(steer);
            steer *= _data.maxSpeed;
            steer -= _boid.Velocity();

		    steer = Vector3.ClampMagnitude(steer, _data.maxSpeed);
	    }
	    return steer;
    }


    // STEER = DESIRED MINUS VELOCITY
	public static Vector3 Seek(Boid _boid, Vector3 target, BoidManager.BoidData _data)
	{
		Vector3 desired = (target - _boid.transform.position); //  a vector pointing from the position to the target
		
        // scale to max speed
		desired.Normalize();

		desired = (desired * _data.maxSpeed);

		//desired.setMag (maxSpeed);
		desired = Vector3.ClampMagnitude(desired,_data. maxSpeed);

		// steering = desired minus velocity
		Vector3 steer = (desired - _boid.Velocity());

		//steer.limit (maxForce); // Limit to maximum steering force
		steer = Vector3.ClampMagnitude(steer, _data.maxForce);

		return steer;
	}
}

}
