using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Kojima Party - Hairy Devs 2018
 * Author: Owen Jackson
 * Purpose: Base class for in game zones; this class stores all of the visuals so you can inherit and add gamemode specfic logic separately
 * Namespace: HDev
 * Last Edited: Owen Jackson @ 27/02/2018
*/

namespace HDev
{
    public class Zone : MonoBehaviour
    {
        protected List<GameObject> playersInZone;   //list of players that are currently in the zone

        [SerializeField]
        protected bool isBoxZone;                   //zones are either box shaped or capsule
        [SerializeField]
        protected Vector3 zoneArea;                 //the area of this zone

        public GameObject linePrefab;               //the prefab used to render the lines from
        protected List<LineRenderer> boundaryLines; //line visuals so players can see this zone
        [SerializeField]
        protected float lineThickness = 0.2f;       //the length and width of the line renderer lines
        [SerializeField]
        protected int linePositionCount = 10;       //used for capsule shaped colliders for the line renderer setup

        //initialise values and render lines
        protected virtual void Awake()
        {
            ResetZone();
            if(isBoxZone)
            {
                BoxCollider box = GetComponent<BoxCollider>();
                //box.size = zoneArea;
                zoneArea = box.bounds.size;
                box.isTrigger = true;
            }
            else
            {
                CapsuleCollider capsule = GetComponent<CapsuleCollider>();
                //capsule.radius = zoneArea.x;
                //capsule.height = zoneArea.y;
                zoneArea = capsule.bounds.extents;
                if(capsule.height / capsule.bounds.extents.y >= 2)
                {
                    zoneArea.y = capsule.height / 2;
                }
                capsule.isTrigger = true;
            }
            SetupRenderLines();
        }

        protected virtual void OnEnable()
        {
            ResetZone();
        }

        /*
        //detects when players enter and adds them to the list of playersInZone
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (!playersInZone.Contains(other.gameObject))
                {
                    Debug.Log("adding new player");
                    playersInZone.Add(other.gameObject);
                }
            }
        }

        //removes players from playersInZone when they leave the area
        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                if (playersInZone.Contains(other.gameObject))
                {
                    playersInZone.Remove(other.gameObject);
                }
            }
        }
        */

        //Initialises values when the zone spawns and resets
        public virtual void ResetZone()
        {
            playersInZone = new List<GameObject>();
        }

        //call to create the shape of the render lines
        private void SetupRenderLines()
        {
            //add render lines based on the height of the zone
            if (linePrefab != null)
            {
                //instantiate the lines and position them
                int lineNum = 0;
                if(zoneArea.y % 2 == 0)
                {
                    lineNum = (int)zoneArea.y / 2;
                }
                else
                {
                    lineNum = (int)((zoneArea.y + 1) / 2);
                }

                for (int i = 0; i < lineNum; i++)
                {
                    GameObject go = Instantiate(linePrefab, transform);
                    Vector3 goPos = go.transform.position;
                    goPos.y = transform.position.y -(zoneArea.y / 2) + ((zoneArea.y / lineNum) * i) + 1;
                    go.transform.position = goPos;

                    //let the lines rotate if the shape is spherical since the collider position won't change
                    if(!isBoxZone)
                    {
                        if (go.GetComponent<SineBob>())
                        {
                            go.GetComponent<SineBob>().DoesRotate = true;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("line prefab is null, need to attach it");
            }

            //add the newly made lines to the list
            boundaryLines = GetComponentsInChildren<LineRenderer>().ToList();

            UpdateRenderLines();
        }

        //this can be used if the zone is going to change size during gameplay
        protected void UpdateRenderLines()
        {
            float startingPos = 0;
            if (zoneArea.y % 2 == 0)
            {
                startingPos = -(zoneArea.y / 2) - 1;
            }
            else
            {
                startingPos = -(zoneArea.y / 2) - 2;
            }
            //setup the lines for a box shape
            if (isBoxZone)
            {
                for (int i = 0; i < boundaryLines.Count; i++)
                {
                    //assign line properties
                    boundaryLines[i].positionCount = 5;
                    boundaryLines[i].useWorldSpace = false;
                    boundaryLines[i].receiveShadows = false;
                    boundaryLines[i].startWidth = lineThickness;
                    boundaryLines[i].endWidth = lineThickness;

                    //create a rectangular shape
                    BoxCollider rect = GetComponent<BoxCollider>();

                    //bottom left
                    Vector3 pos = -transform.right * (rect.size.x / 2) - transform.forward * (rect.size.z / 2);
                    boundaryLines[i].SetPosition(0, pos);

                    //top left
                    pos = -transform.right * (rect.size.x/2) + transform.forward * (rect.size.z/2);
                    boundaryLines[i].SetPosition(1, pos);

                    //top right
                    pos = transform.right * (rect.size.x / 2) + transform.forward * (rect.size.z/2);
                    boundaryLines[i].SetPosition(2, pos);

                    //bottom right
                    pos = transform.right * (rect.size.x / 2) - transform.forward * (rect.size.z / 2);
                    boundaryLines[i].SetPosition(3, pos);

                    //join back to the start
                    pos = -transform.right * (rect.size.x / 2) - transform.forward * (rect.size.z / 2);
                    boundaryLines[i].SetPosition(4, pos);
                }
            }
            //setup for a capsule/spherical shape
            else
            {
                float angle = 0f;
                float x, z;
                for (int i = 0; i < boundaryLines.Count; i++)
                {
                    //assign line properties
                    boundaryLines[i].positionCount = linePositionCount;
                    boundaryLines[i].useWorldSpace = false;
                    boundaryLines[i].receiveShadows = false;
                    boundaryLines[i].startWidth = lineThickness;
                    boundaryLines[i].endWidth = lineThickness;

                    //Create a circular shape with the lines
                    for (int j = 0; j < boundaryLines[i].positionCount; j++)
                    {
                        x = Mathf.Sin(Mathf.Deg2Rad * angle) * zoneArea.x;
                        z = Mathf.Cos(Mathf.Deg2Rad * angle) * zoneArea.z;

                        boundaryLines[i].SetPosition(j, new Vector3(x, 0, z));
                        angle += (360f / (linePositionCount - 1));
                    }
                }
            }
        }
    }
}
