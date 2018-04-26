using UnityEngine;
using System.Collections.Generic;

// Kojima Party - Team Hairy Devs 2018
// Author: Curtis Wiseman
// Purpose: Navigation system
// Namespace: Hairy Devs
// Script Created: 20/02/2018 16:00
// Last Modified: 13/03/2018 12:24

namespace HDev
{
    public class SatnavController : MonoBehaviour
    {
        private HDev_Arrow arrow;
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private List<Bird.BezierSpline> allSplines;
        [SerializeField] private GameObject player;
        [SerializeField] private float fadeOutDisctanceThreshold = 5.0f;
        [SerializeField] private PickupZoneManager pickupZoneManager;
        [SerializeField] private DropoffZoneManager dropoffZoneManager;

        private List<Transform> dropoffZones;
        private List<Transform> pickupZones;
        private List<HDev_Arrow> nonSolidBarriers;
        private Vector3 currentTargetWaypoint;
        private Transform overallTarget;
        private List<Vector3> allRoadPoints;
        private bool isSetUp = false;
        private float setUpTimer = 0.0f;
        private bool hasPackages = false;

        private void Update()
        {
            if(isSetUp)
            {
                RefreshZoneLists();
                if (allRoadPoints.Count > 0)
                {
                    if(player.GetComponent<PackageManager>().packages.Count < 1)
                    {
                        overallTarget = FindClosestTransformFromList(player.transform, pickupZones);
                        Debug.Log(overallTarget.position);
                    }
                    else
                    {
                        overallTarget = FindClosestTransformFromList(player.transform, dropoffZones);
                        Debug.Log(overallTarget.position);
                    }

                    Vector3 tempTargetWaypoint = FindNextWaypointFromPosition(player.transform, overallTarget);
                    arrow.m_target.transform.position = tempTargetWaypoint;

                    if (tempTargetWaypoint != currentTargetWaypoint)
                    {
                        currentTargetWaypoint = tempTargetWaypoint;    
                        UpdateBarriers();
                    }

                }
            }
            else
            {
                //Delayed startup
                setUpTimer += Time.deltaTime;
                if (setUpTimer > 0.1f)
                {
                    isSetUp = true;
                    SetUp();
                    //Debug.Log("Set up Satnav system");
                }
            }
        }

        private void SetUp()
        {
            arrow = this.gameObject.GetComponent<HDev_Arrow>();
            allRoadPoints = new List<Vector3>();
            nonSolidBarriers = new List<HDev_Arrow>();
            RefreshZoneLists();

            foreach (Bird.BezierSpline bS in allSplines)
            {
                allRoadPoints.AddRange(bS.points);
            }

            /*
            foreach (Vector3 rP in allRoadPoints)
            {
                GameObject tempArrow = Instantiate(arrowPrefab);
                tempArrow.transform.position = rP;

                GameObject arrowTar = new GameObject();
                arrowTar.transform.position = rP;
                arrowTar.transform.parent = tempArrow.transform;

                nonSolidBarriers.Add(tempArrow.GetComponent<HDev_Arrow>());
            }
            */
        }

        private void RefreshZoneLists()
        {
            dropoffZones = new List<Transform>();
            pickupZones = new List<Transform>();

            dropoffZones.Add(dropoffZoneManager.currentZone.transform);
            pickupZones.Add(pickupZoneManager.currentZone.transform);
        }

        private Vector3 FindNextWaypoint(Transform _target, float _distanceThreshold)
        {
            allRoadPoints.Sort((p1, p2) =>
                                Vector3.Distance(p1, _target.position).CompareTo(
                                Vector3.Distance(p2, _target.position)));
 
            if(Vector3.Distance(allRoadPoints[0], _target.position) > _distanceThreshold)
            {
                //Debug.Log("FoundClosestWaypoint");
                return allRoadPoints[0];
            }
            else
            {
                //Debug.Log("FoundNextWaypoint");
                return allRoadPoints[1];
            }
        }

        private Transform FindClosestTransformFromList(Transform _t1, List<Transform> _t2)
        {
            Transform temp = _t2[0];
            foreach(Transform tra in _t2)
            {
                if(Vector3.Distance(_t1.position, tra.position) < Vector3.Distance(_t1.position, temp.position))
                {
                    temp = tra;
                }
            }

            return temp;
        }

        private Vector3 FindNextWaypointFromPosition(Transform _start, Transform _target)
        {
            allRoadPoints.Sort((p1, p2) =>
                                Vector3.Distance(p1, _start.position).CompareTo(
                                Vector3.Distance(p2, _start.position)));

            int currentClosest = 0;
            for(int i = 0; i < allRoadPoints.Count; i++)
            {
                if(Vector3.Distance(allRoadPoints[i], _target.position) 
                    < Vector3.Distance(allRoadPoints[0], _target.position))
                {
                    currentClosest = i;
                    break;
                }
            }

            //Debug.Log("FoundNextWaypoint");
            return allRoadPoints[currentClosest];
        }

        private Vector3 FindClosestTo(Transform _target)
        {
            float currentClosest = 9999;
            int closestPos = 0;
            int i = 0;

            foreach (Vector3 vR in allRoadPoints)
            {
                float testDist = Vector3.Distance(vR, _target.position);

                if (testDist < currentClosest)
                {
                    currentClosest = testDist;
                    closestPos = i;
                }

                i++;
            }

            return allRoadPoints[closestPos];
        }

        private List<Vector3> FindClosestMultiplePoints(int _numberOfPoints, Transform _target)
        {
            allRoadPoints.Sort((p1, p2) => 
                                Vector3.Distance(p1, _target.position).CompareTo(
                                Vector3.Distance(p2, _target.position)));

            List<Vector3> tempList = new List<Vector3>();

            for(int i = 0; i < _numberOfPoints; i++)
            {
                tempList.Add(allRoadPoints[0]);
            }

            return tempList;
        }

        private void UpdateBarriers()
        {
            for(int i = 0; i < nonSolidBarriers.Count; i++)
            {
                nonSolidBarriers[i].m_target.transform.position = FindNextWaypointFromPosition(nonSolidBarriers[i].gameObject.transform, overallTarget);
            }
        }
    }
}