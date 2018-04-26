using UnityEngine;
using System.Collections.Generic;


public class RespawnManager : MonoBehaviour
{
    public List<SplineDecor> allrespawnPaths;
    public List<Transform> allRespawnPoints;

    public void SetUp()
    {
        allRespawnPoints = new List<Transform>();

        foreach (SplineDecor sD in allrespawnPaths)
        {
            sD.SetupRespawn();
            allRespawnPoints.AddRange(sD.CreatedObjects);
        }
    }

    public Transform FindClosest(Transform _inTr)
    {
        float currentClosest = 9999;
        int closestPos = 0;
        int i = 0;

        foreach (Transform tr in allRespawnPoints)
        {
            float testDist = Vector3.Distance(tr.position, _inTr.position);

            if (testDist < currentClosest)
            {
                currentClosest = testDist;
                closestPos = i;
            }

            i++;
        }

        return allRespawnPoints[closestPos];
    }

    //This would be triggered by the event manager triggering the begining of the event
    void Awake()
    {
        SetUp();
    }
}