using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

public class BoatCircuitNode : MonoBehaviour
{
    public CustomEvents.BoatEvent onBoatArrived;
    public CustomEvents.BoatEvent onBoatDeparted;

    [HideInInspector] public BoatCircuitNode prevNode;
    [HideInInspector] public BoatCircuitNode nextNode;

    [SerializeField] float targetSpeed = 3;
    [SerializeField] float waitDelay = 0;

    public List<SpawnableObjectSettings> objectsToEnable;


    public void BoatArrived(DuckHuntBoat _boat)
    {
        onBoatArrived.Invoke(_boat);

        JBSceneRefs.objectSpawner.DisableAllObjects();

        foreach (var spawnable in objectsToEnable)
        {
            JBSceneRefs.objectSpawner.EnableObject(spawnable.objectID);
        }
    }


    public void BoatDeparted(DuckHuntBoat _boat)
    {
        onBoatDeparted.Invoke(_boat);
    }

}

} // namespace JB
