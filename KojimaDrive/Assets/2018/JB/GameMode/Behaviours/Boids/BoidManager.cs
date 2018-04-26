using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JB
{

public class BoidManager : MonoBehaviour
{
    [SerializeField] int noBoids;

    [SerializeField] int minSwarmSize = 5;

    [SerializeField] GameObject boidPrefab;

    [SerializeField] GameObject followObj;

    [SerializeField] Transform boidContainer;

    [SerializeField] BoidData boidData;

    [SerializeField] float swarmLifeTimeMax;

    private List<Boid> boids;

    private float swarmLifeTime;

    private bool initialised;

    private bool setupComplete;

    private bool cleanupDone;



    [Serializable]
    public class BoidData
    {
        [SerializeField] public float maxForce;
        [SerializeField] public float maxSpeed;

        [SerializeField] public float neighbourDistance;
        [SerializeField] public float desiredSeperation;

        [SerializeField] public float sepWeight;
        [SerializeField] public float aliWeight;
        [SerializeField] public float cohWeight;
        [SerializeField] public float ffWeight;
    }

    // Use this for initialization
    private void Awake()
    {
        transform.position = JBSceneRefs.boat.transform.position;

        transform.parent = JBSceneRefs.boat.transform;

        boids = new List<Boid>();

        for (int i = 0; i < noBoids; i++)
        {
            GenerateBoid();
        }
    }


    private void Update()
    {
        boids.RemoveAll(o => o == null);

        foreach (Boid boid in boids)
        {
            boid.Run(boids, followObj, boidData);
        }

        if (swarmLifeTime >= swarmLifeTimeMax || boids.Count <= minSwarmSize)
        {
            boidData.ffWeight = 1.0f;

            // Destroy this game object and children ect
            if (!cleanupDone)
                CleanUp();
        }
    }


    private void GenerateBoid()
    {
        var boid = Instantiate(boidPrefab, followObj.transform.position, Quaternion.identity);

        boids.Add(boid.GetComponent<Boid>());

        //boid.GetComponent<Boid>().Initialise(boidData);

        //boid.transform.parent = boidContainer.transform;
    }


    private void CleanUp()
    {
        foreach (Boid boid in boids)
        {
            Destroy(boid.gameObject, 5.0f);
        }

        Destroy(followObj, 5.0f);

        Destroy(this.gameObject, 5.0f);
        cleanupDone = true;
    }
}

}
