using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

// ObjectSpawner - Samuel Harden - JuiceBox
// 
// 
// The main purpose of this script is to spawn Prefab objects into the scene
// It requires a gameobject to be set as the targetObject so that objects
// can be spawned within a given radius around (or in front) of this 
// referenced gameobject. Should a gameobject not be set, a debug log will
// be produced telling you so, and defaulting the pos to vector3.zero.
// All variables visible in the inspector have tooltips to give you more 
// information on what it is used for, the script will produce debug logs
// for most user errors when using the script, should you encounter an issue
// with the script or would like to suggest an improvement please contact
// Samuel Harden or any other member of the JuiceBox Team (Joe, Tom, Leo,
// Ziggy or Elliott), for more detailed information about this script.

namespace JB
{

public class ObjectSpawner : MonoBehaviour
{
    [Header("References")]
    [Tooltip(" the object you wish to spawn things around. Should an object not be set," +
        " this value will default to Vector3.zero. This value can also be" +
        " set through a public function - SetTargetObject(object)")]
    [SerializeField] GameObject targetObj;

    [Space]
    [Header("Variables")]

    [Space]
    [Tooltip("Min distance gameobjects can be spawned in (from targetObj")]
    [SerializeField] float minSpawnDistance;

    [Tooltip("Max distance gameobjects can be spawned in (from targetObj")]
    [SerializeField] float maxSpawnDistance;

    [Tooltip("distance to check when spawning a new object " +
        "(To stop objects spawning on top of each other." +
        " This value should be relative to the size of the objects you are spawning)")]
    [SerializeField] float collisionCheckDistance;

    [Tooltip("How often objects are cleared in seconds")]
    [SerializeField] int clearTime;

    [Space]
    [Tooltip("This offset is in relation to the targetObjects forward direction" +
        " (if you want objects to spawn in front of targetObject, try increasing this value!)")]
    [SerializeField] float offset;

    [Space]
    [Tooltip("The Max angle you wish to spawn objects, this number is added to the targetObjects" +
        " forward vector in both plus and minus. so by setting this to value to 90, objects will spawn" +
        " objects within a radius of the targetObjects Vector3.right and -Vector3.right." +
        " Set this value to 180 if you want objects to spawn all around the targetObj.")]
    [SerializeField] float maxAngle = 180.0f;

    [Space]
    [SerializeField] float offsetAngle = 90.0f;

    [Space]
    [Tooltip("Enable this if you are spawning objects that require a navmesh " +
        "(such as gameobjects with the NavAgent component, be sure that a navmesh has" +
        " also been baked for the scene otherwise nothing will spawn!)")]
    [SerializeField] bool usingNavMesh;

    [Space]
    [Header("JuiceBox Variables")]

    [Space]
    [Tooltip("JUICEBOX SPECIFIC - Enabling this will allow the targetPosition to compensate for" +
        " targetObj's speed, increasing and decreasing the Offset value based" +
        " on the targetObj's Speed")]
    [SerializeField]
    bool objectSpeed;

    [Space]
    [Tooltip("The max speed the object can go (or limited max speed if it can go faster)," +
        " will default to 25")]
    [SerializeField]
    float targetMaxSpeed = 10.0f;

    [Space]
    [Tooltip("The Max increase to the 'offset' variable, this will increase/decrease based" +
        " on the speed of the targetObj")]
    [SerializeField]
    float maxIncrease = 50.0f;

    [Space]
    [Header("List Variables")]
    [Tooltip("This list should contain SpawnableObjectSettings for the different" +
        " types of gameobjects you wish to spawn")]
    [SerializeField] List<SpawnableObjectSettings> spawnables;

    [Space]
    [Header("Enable/Disable Gizmos + Debugging")]
    [Tooltip("Colours for spheres are as follows: minSpawnDistance - blue," +
        " maxSpawnDistance - red, clearDistance - white")]
    [SerializeField] bool gizmos;

    [Space]
    [Tooltip("Enable this if you want to be prompted about warnings, ie (Debug.Log")]
    [SerializeField] bool debug;

    // Private Variables etc
    private int searchAttempts = 50;

    //Default values ( if no value is set)
    private int defaultMaxObjects = 5;
    private float defaultSpawnTime = 3;

    private float updateTimer = 0.0f;

    private bool defaultPos;

    private List<List<GameObject>> objectContainer;
    private List<float> spawnTimers;

    private float startingOffset;


    void Awake()
    {
        for (int i = 0; i < spawnables.Count; i++)
        {
            spawnables[i].objectID = i;
        }
    }


    void Start()
    {
        // Used to hold references to all the instantiated gameobjects
        objectContainer = new List<List<GameObject>>();

        spawnTimers = new List<float>();

        Validate();

        startingOffset = offset;
    }


    void Update()
    {
        // JB Specific, if enabled will adjust the spawn pos
        // based on the targetObj's speed
        if (objectSpeed)
            UpdateOffsetPosition();

        SpawnObjects(); // Spawn new stuff!

        //ClearObjects();

        // Update Spawn Timers for each object
        for (int i = 0; i < spawnTimers.Count; i++)
        {
            spawnTimers[i] += Time.deltaTime;
        }
    }


    void FixedUpdate()
    {
        ClearObjects();
    }


    public List<SpawnableObjectSettings> GetSpawnables()
    {
        return spawnables;
    }


    void Validate()
    {
        ValidateObjectPrefabs(); // Validate the objects to spawn (Check prefab is valid not NULL)

        ValidateListContents(); // validate contents (check values, set to defaults if 0(not set))

        SetPrivateLists(); // Initialise internal lists (timers & container for each object list)

        ValidateVariables(); // Check all variables

        ValidateTargetObj(); //  Check a targetObj has been set, if not default
    }


    void ValidateObjectPrefabs()
    {
        for (int i = spawnables.Count - 1; i >= 0; i--)
        {
            if (spawnables[i].objectPrefab == null)
            {
                if (debug)
                    Debug.Log("objectPrefabs was NULL at index: " + i + ". Removing Entry");

                spawnables.RemoveAt(i);
            }
        }
    }


    void ValidateListContents()
    {
        for (int i = 0; i < spawnables.Count; i++)
        {
            if (spawnables[i].objectMaxCount == 0)
            {
                if (debug)
                    Debug.Log("objectMaxCount is missing a value at index: "
                        + i + ", Reverting to Default value");

                spawnables[i].objectMaxCount = defaultMaxObjects;
            }
        }

        for (int i = 0; i < spawnables.Count; i++)
        {
            if (spawnables[i].objectSpawnTime == 0)
            {
                if (debug)
                    Debug.Log("objectSpawnTimes is missing a value at index: "
                        + i + ", Reverting to Default value");

                spawnables[i].objectSpawnTime = defaultSpawnTime;
            }
        }
    }


    void SetPrivateLists()
    {
        while (spawnTimers.Count != spawnables.Count)
        {
            spawnTimers.Add(0.0f);
        }

        while (objectContainer.Count != spawnables.Count)
        {
            objectContainer.Add(new List<GameObject>());
        }
    }


    void ValidateVariables()
    {
        if (minSpawnDistance < 1)
        {
            minSpawnDistance = 50.0f;

            if (debug)
                Debug.Log("minSpawnDistance was not set, defaulting to 50.0f");
        }

        if (maxSpawnDistance < 1)
        {
            maxSpawnDistance = minSpawnDistance * 2;

            if (debug)
                Debug.Log("maxSpawnDistance was not set," +
                    " defaulting to " + maxSpawnDistance);
        }

        if (minSpawnDistance >= maxSpawnDistance)
        {
            minSpawnDistance = maxSpawnDistance / 2;

            if (debug)
                Debug.Log("minSpawnDistance cannot be greater" +
                    " than maxSpawnDistance, defaulting");
        }

        if (maxAngle == 0)
        {
            maxAngle = 180.0f;

            if (debug)
                Debug.Log("maxAngle was not set, defaulting");
        }

        if (targetMaxSpeed > 25 || targetMaxSpeed < 0)
        {
            targetMaxSpeed = 25.0f;
            if (debug)
                Debug.Log("targetMaxSpeed Invalid, defaulting");
        }
    }


    void ValidateTargetObj()
    {
        if (targetObj == null)
        {
            defaultPos = true;

            if (debug)
                Debug.Log("targetObj has not been set, defaulting pos to Vector3(0, 0, 0)");
        }
    }


    void UpdateOffsetPosition()
    {
        if (targetObj != null)
        {
            // Update position based on targets Speed
            //float currentSpeed = targetObj.GetComponent<ShipControl>().GetForwardSpeed();
            float currentSpeed = 0;

            if (currentSpeed > targetMaxSpeed)
                currentSpeed = targetMaxSpeed;

            // Get % out of 100
            float percent = currentSpeed / targetMaxSpeed * 100;

            // Set Offset Value
            offset = startingOffset + ((maxIncrease / 100) * percent);
        }

        else
        {
            // Error Checking
            if (debug)
            {
                Debug.Log("Cannot update targetPos because targetObj is not set");
                objectSpeed = false;
            }
        }
    }


    void SpawnObjects()
    {
        // loop through all the object types that need to be spawned
        for (int i = 0; i < spawnables.Count; i++)
        {
            // If we can still spawn more if this object
            // and the timer has been reached
            if (objectContainer[i].Count < spawnables[i].objectMaxCount &&
                spawnTimers[i] >= spawnables[i].objectSpawnTime && spawnables[i].active)
            {
                // Spawn a new obj of this type
                Vector3 pos = GeneratePos(i);

                if (ValidatePos(pos))
                    SpawnObject(pos, i);

                // reset timer
                spawnTimers[i] = 0;
            }
        }
    }

    
    public void DisableObject(int _index)
    {
        var spawnable = spawnables.Find(elem => elem.objectID == _index);
        spawnable.active = false;
    }


    public void EnableObject(int _index)
    {
        var spawnable = spawnables.Find(elem => elem.objectID == _index);
        spawnable.active = true;
    }


    public void DisableAllObjects()
    {
        foreach (var spawnable in spawnables)
        {
            spawnable.active = false;
        }
    }


    void ClearObjects()
    {
        if (updateTimer > clearTime)
        {
            for (int i = 0; i < spawnables.Count; i++)
            {
                // clear any objects too far away
                ClearGameObjects(i);
            }

            updateTimer = 0.0f; // Reset Timer
        }

        updateTimer += Time.deltaTime;
    }


    void ClearGameObjects(int _objectType)
    {
        objectContainer[_objectType].RemoveAll(o => o == null);

        Vector3 pos = Vector3.zero;

        if (!defaultPos)
            pos = targetObj.transform.position + (targetObj.transform.forward * offset);

        for (int i = objectContainer[_objectType].Count - 1; i >= 0; i--)
        {
            if (Vector3.Distance(pos, objectContainer[_objectType][i].transform.position) > spawnables[_objectType].clearDistance)
            {
                if (spawnables[_objectType].cleanUp)
                {
                    // If another object has taken ownership, ObjectSpawner will
                    // remove the reference (& ownership)
                    if (objectContainer[_objectType][i].transform.parent != null)
                    {
                        Destroy(objectContainer[_objectType][i], 0.5f);

                        objectContainer[_objectType].RemoveAt(i);
                        continue;
                    }
                }

                Destroy(objectContainer[_objectType][i], 0.5f);

                objectContainer[_objectType].RemoveAt(i);
            }
        }
    }


    // Force clear all gameobjects, used for Debugging
    public void ForceClearGameObjects()
    {
        if (objectContainer != null)
        {
            for (int i = 0; i < objectContainer.Count; i++)
            {
                objectContainer[i].RemoveAll(o => o == null);

                if (objectContainer[i] != null)
                {
                    for (int j = objectContainer[i].Count - 1; j >= 0; j--)
                    {
                        Destroy(objectContainer[i][j], 0.5f);

                        objectContainer[i].RemoveAt(j);
                    }
                }
            }
        }
    }


    bool ValidatePos(Vector3 _pos)
    {
        if (_pos.sqrMagnitude > 0.0f)
        {
            return true;
        }

        // If no pos Found.
        if (debug)
            Debug.Log("No Pos Found");

        return false;
    }


    Vector3 GeneratePos(int _objectType)
    {
        // If a target object has not been set, use default pos (Vector3.zero)
        Vector3 pos = Vector3.zero;

        if (!defaultPos)
            pos = targetObj.transform.position + (targetObj.transform.forward * offset);

        if (spawnables[_objectType].offsetX != 0 || spawnables[_objectType].offsetZ != 0)
        {
            pos = targetObj.transform.position + (targetObj.transform.forward * spawnables[_objectType].offsetZ) + (targetObj.transform.right * spawnables[_objectType].offsetX);
        }

        // Loop until a position is found
        for (int i = 0; i < searchAttempts; i++)
        {
            // Take the targetObj forward direction, get random pos within user specified angle
            float angle = Random.Range(targetObj.transform.rotation.eulerAngles.y - maxAngle,
                targetObj.transform.rotation.eulerAngles.y + maxAngle);

            angle += offsetAngle;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

            Vector3 distToTarget = rotation * transform.forward * Random.Range(minSpawnDistance,
                maxSpawnDistance);

            Vector3 potentialPos = pos + distToTarget;

            if (usingNavMesh)
                potentialPos = ValidDestination(potentialPos);

            Debug.DrawRay(pos, distToTarget, Color.red, 3);

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(potentialPos.x, 100.0f,
                potentialPos.z),
                    Vector3.down, out hit, 100.0f, spawnables[_objectType].checkLayers))
            {
                potentialPos.y = hit.point.y;

                if (hit.collider.gameObject.layer == LayerMask.NameToLayer(spawnables[_objectType].spawnLayer))
                {
                    // Final Check to see if pos hits a collider (ie another Boat)
                    Collider[] colliders = Physics.OverlapSphere(potentialPos,
                        collisionCheckDistance, spawnables[_objectType].avoidLayer);

                    if (colliders.Length == 0)
                    {
                        return potentialPos;
                    }
                }
            }
        }

        return Vector3.zero;
    }


    Vector3 ValidDestination(Vector3 _desiredPos)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(_desiredPos, out hit, 1000, NavMesh.AllAreas))
            return _desiredPos;

        NavMesh.FindClosestEdge(_desiredPos, out hit, NavMesh.AllAreas);

        return hit.position;
    }


    // Spawn object
    void SpawnObject(Vector3 _pos, int _objectType)
    {
        Quaternion rotation = Quaternion.identity;

        // Use a Random Rotation on all Axis(s)
        if (spawnables[_objectType].randomRotation)
        {
            rotation = Random.rotation;
        }

            // Set a specific Rotation + targetObj Direction
            else if (!spawnables[_objectType].randomRotation)
        {
            rotation.eulerAngles = new Vector3
            (targetObj.transform.eulerAngles.x + spawnables[_objectType].objRotX,
                targetObj.transform.eulerAngles.y + spawnables[_objectType].objRotY,
                targetObj.transform.eulerAngles.z + spawnables[_objectType].objRotZ);
        }

        var gameObj = Instantiate(spawnables[_objectType].objectPrefab,
            new Vector3(_pos.x, _pos.y + spawnables[_objectType].spawnHeightOffset, _pos.z),
            Quaternion.identity);

        Vector3 randomPos = gameObj.transform.position;

        if (spawnables[_objectType].randomHeight)
        {
            randomPos.y = Random.Range(spawnables[_objectType].minHeight,
                spawnables[_objectType].maxHeight);
        }

        gameObj.transform.position = randomPos;
        gameObj.transform.rotation = rotation;

        objectContainer[_objectType].Add(gameObj);
    }


    // Set targetObj as a Game object
    public void SetTargetObject(GameObject _target)
    {
        targetObj = _target;

        defaultPos = false;
    }


    void OnDrawGizmos()
    {
        if (targetObj == null)
            return;

        if (gizmos)
        {
            Vector3 pos = Vector3.zero;

            if (targetObj != null)
                pos = targetObj.transform.position + (targetObj.transform.forward * offset);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(pos, minSpawnDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos, maxSpawnDistance);

            Vector3 pos1 = targetObj.transform.position +
                Quaternion.AngleAxis(targetObj.transform.eulerAngles.y - maxAngle + offsetAngle, Vector3.up)
                * Vector3.forward * maxSpawnDistance;

            Vector3 pos2 = targetObj.transform.position +
                Quaternion.AngleAxis(targetObj.transform.eulerAngles.y + maxAngle + offsetAngle, Vector3.up)
                * Vector3.forward * maxSpawnDistance;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(targetObj.transform.position + new Vector3(offset, 0, 0), pos1 + new Vector3(offset, 0, 0));
            Gizmos.DrawLine(targetObj.transform.position + new Vector3(offset, 0, 0), pos2 + new Vector3(offset, 0, 0));
        }
    }
}

} // namespace JB
