using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawnable Objects Settings")]
public class SpawnableObjectSettings : ScriptableObject 
{
    [Tooltip("Reference to the prefab GameObject you want to spawn in")]
    public GameObject objectPrefab = null;
    [Tooltip("Maximum number of this objectType you want to be present at any given time")]
    public int objectMaxCount = 0;
    [Tooltip("How often should a new gameobject be spawned")]
    public float objectSpawnTime = 0;
    [Tooltip("If you want the gameobject to spawn underwater or in the air adjust this value")]
    public float spawnHeightOffset = 0;

    public bool randomHeight;

    [Tooltip("if randomHeight is enabled, this will set the min potential height an obj can spawn (0.0f - default)")]
    public float minHeight = 0.0f;

    [Tooltip("if randomHeight is enabled, this will set the max potential height an obj can spawn (20.0f - default)")]
    public float maxHeight = 20.0f;

    [Space]
    [Tooltip("Add random rotation to objects upon Spawn")]
    public bool randomRotation;

    [Tooltip("If randomRotation is not set you can manually set a rotation here")]
    public float objRotX;
    public float objRotY;
    public float objRotZ;

    [Space]
    [Tooltip("Layers to check, ie the one(s) you wish to compare, this will then be used" +
    " to see which layer was hit first, if the layer matches spawnLayer it will" +
    " use the position, else it will look for another")]
    public LayerMask checkLayers;

    [Tooltip("Avoid layer, used to stop objects spawning on each other, be sure" +
    " to set a value for 'collisionCheckDistance' if you intent to use this feature")]
    public LayerMask avoidLayer;

    [Space]
    [Tooltip("The layer you want objects to spawn on (String)")]
    public string spawnLayer;

    [Space]
    [Tooltip("Offset from the targetObj's position (if you want to spawn objects further away etc")]
    public float offsetX;
    public float offsetZ;

    [Space]
    public bool active;

    [Space]
    public int objectID;

    [Space]
    [Tooltip("Gameobjects will be cleared at this distance")]
    public float clearDistance;

    [Space]
    [Header("JB Specific Variables")]

    [Space]
    [Tooltip("Enable this if you want this object to have a reference to the boat")]
    public bool enableRef;


    [Space]
    [Tooltip("Enabling this will mean that even if an object is set as a child of another object," +
        " it will still be Cleaned up. Leaving this unchecked, will ignore any obeject that are set" +
        " as children and NOT clean them up")]
    public bool cleanUp;
}
