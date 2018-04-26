using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public class PhysicsManager : MonoBehaviour
{
    private Vector3 gravity;
    private float bounceThreshold;
    private float sleepThreshold;
    private float defaultContactOffset;
    private int defaultSolverIterations;
    private int defaultSolverVelocityIterations;

	void Start()
    {
        gravity = Physics.gravity;
        bounceThreshold = Physics.bounceThreshold;
        sleepThreshold = Physics.sleepThreshold;
        defaultContactOffset = Physics.defaultContactOffset;
        defaultSolverIterations = Physics.defaultSolverIterations;
        defaultSolverVelocityIterations = Physics.defaultSolverIterations;

        Physics.gravity = new Vector3 (0,-9.81f,0);
        Physics.bounceThreshold = 2;
        Physics.sleepThreshold = 0.005f;
        Physics.defaultContactOffset = 0.01f;
        Physics.defaultSolverIterations = 6;
        Physics.defaultSolverVelocityIterations = 1;
	}
	

	void Update()
    {
		
	}


    void OnDestroy()
    {
        Physics.gravity = gravity;
        Physics.bounceThreshold = bounceThreshold;
        Physics.sleepThreshold = sleepThreshold;
        Physics.defaultContactOffset = defaultContactOffset;
        Physics.defaultSolverIterations = defaultSolverIterations;
        Physics.defaultSolverVelocityIterations = defaultSolverVelocityIterations;
    }

}

} // namespace JB
