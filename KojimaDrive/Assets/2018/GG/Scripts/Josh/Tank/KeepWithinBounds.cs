using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===================== Kojima Party - GrizzledGames 2018 ====================//
//
// Author:		Josh Fenlon
// Purpose:		Basic functions that keep objects within a play area
// Namespace:	GG
//
//============================================================================//

namespace GG
{

    public class KeepWithinBounds : MonoBehaviour
    {

        //stores basic bounds data requireed
        [System.Serializable]
        public class boundsData
        {
            public GameObject boundsObj = null;
            public Vector3 originalRotation = Vector3.zero;
            public Vector3 centerPos = Vector3.zero;
            public float yOffset = 0;
            public float maxDistance = 0;
            public float marginDistance = 0;
            public float resetMarginDistance = 0;
            public bool grabObjPos = false;
            public bool objectHeadingToCenter = false;
        }

        //stores basic object data required
        [System.Serializable]
        public class objBoundsData
        {
            public GameObject objToKeepWithin;
            public bool goingBack;
            public Vector3 aim;
            public int boundsToGoBack;
            public float timeGoingBack;
            public bool forceReset = false;
        }

        [SerializeField]
        public List<boundsData> _boundData = new List<boundsData>();
        [SerializeField]
        public List<objBoundsData> objsToKeepWithin = new List<objBoundsData>();
        [SerializeField]
        private bool enableDebug = false;
        [SerializeField]
        private bool debugSolid = false;
        [SerializeField]
        public float timeGoingBackBeforeReset = 5;

        // Use this for initialization
        void Start()
        {
            for (int a = 0; a < _boundData.Count; a++)
            {
                if (_boundData[a].boundsObj != null)
                {
                    if (_boundData[a].grabObjPos)
                    {
                        _boundData[a].centerPos = _boundData[a].boundsObj.transform.position;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            //loop through all objects to be tested
            foreach (objBoundsData objBound in objsToKeepWithin)
            {
                bool within = false;
                int notWithin = -1;
                float closestDist = float.MaxValue;
                bool needWithin = false;

                if (!objBound.goingBack)
                {
                    //loop through all bounds and check if the object is within range of it
                    for (int a = 0; a < _boundData.Count; a++)
                    {
                        float distance = Vector3.Distance(_boundData[a].centerPos, objBound.objToKeepWithin.transform.position);
                        if (distance > _boundData[a].maxDistance)
                        {
                            if (distance < closestDist)
                            {
                                if (!_boundData[a].objectHeadingToCenter)
                                {
                                    notWithin = a;
                                    closestDist = distance;
                                }
                                else
                                {
                                    needWithin = true;
                                }
                            }
                        }
                        else
                        {
                            within = true;
                            break;
                        }
                    }

                    if (needWithin)
                    {
                        if (notWithin == -1)
                        {
                            notWithin = Random.Range(0, _boundData.Count);
                        }
                    }
                }

                //check if the object is within bounds or not
                if ((notWithin != -1 && within == false) || objBound.goingBack)
                {
                    //check if the object has been given random rotation force
                    if (!objBound.goingBack)
                    {
                        objBound.goingBack = true;
                        objBound.boundsToGoBack = notWithin;
                        objBound.objToKeepWithin.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                    }
                    else
                    {
                        //check if the distance from the object to the bounds center pos is within bounds margin distance
                        if (Vector2.Distance(new Vector2(objBound.objToKeepWithin.transform.position.x, objBound.objToKeepWithin.transform.position.z),
                            new Vector2(_boundData[objBound.boundsToGoBack].centerPos.x, _boundData[objBound.boundsToGoBack].centerPos.z)) <= _boundData[objBound.boundsToGoBack].marginDistance || objBound.forceReset)
                        {
                            //set the objects position to bounds position at object y height
                            objBound.objToKeepWithin.transform.position =
                                new Vector3(_boundData[objBound.boundsToGoBack].centerPos.x, objBound.objToKeepWithin.transform.position.y, _boundData[objBound.boundsToGoBack].centerPos.z);

                            //reset the angular velocity
                            objBound.objToKeepWithin.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                            //check if the object is close to the original center point
                            if (Mathf.Abs(objBound.objToKeepWithin.transform.position.y - _boundData[objBound.boundsToGoBack].centerPos.y) < _boundData[objBound.boundsToGoBack].resetMarginDistance || objBound.forceReset)
                            {
                                //set the velocity to zero and then set the rotation back to normal
                                objBound.objToKeepWithin.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                objBound.goingBack = false;
                                Quaternion temp = objBound.objToKeepWithin.transform.rotation;
                                temp.eulerAngles = _boundData[objBound.boundsToGoBack].originalRotation;
                                objBound.objToKeepWithin.transform.rotation = temp;
                                objBound.forceReset = false;
                                objBound.timeGoingBack = 0;
                                objBound.boundsToGoBack = -1;
                            }
                        }
                        else
                        {
                            objBound.timeGoingBack += Time.deltaTime;

                            if (objBound.timeGoingBack >= timeGoingBackBeforeReset)
                            {
                                objBound.forceReset = true;
                            }

                            //set the velocity of the object to zero and lerp to center pos
                            objBound.objToKeepWithin.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            objBound.objToKeepWithin.transform.position =
                                Vector3.Lerp(objBound.objToKeepWithin.transform.position, _boundData[objBound.boundsToGoBack].centerPos + new Vector3(0, _boundData[objBound.boundsToGoBack].yOffset, 0), Time.deltaTime);
                        }
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            if (enableDebug)
            {
                for (int a = 0; a < _boundData.Count; a++)
                {
                    if (debugSolid)
                    {
                        Gizmos.DrawSphere(_boundData[a].centerPos, _boundData[a].maxDistance);
                    }
                    else
                    {
                        Gizmos.DrawWireSphere(_boundData[a].centerPos, _boundData[a].maxDistance);
                    }
                }
            }
        }
    }

}