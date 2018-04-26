using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftComponent : MonoBehaviour
{
    public AircraftController parentAircraft;
    private float maxAngle = 15;
    [SerializeField] private ComponentType _componentType;
    private float flapSpeed = 10;
    public float force;
    private int dir;
    public Vector3 LiftVector3;
    public float wingsurface;

    public float flapsurface;

    // Use this for initialization
    void Start()
    {
        parentAircraft = GetComponentInParent<AircraftController>();

        if (_componentType == ComponentType.AileronLeft)
        {
            dir = -1;
        }
        else if (_componentType == ComponentType.AileronRight)
        {
            dir = 1;
        }
    }

    void SetFlapsRotation(ComponentType type)
    {
        float currentAngle;
        float desiredAngle = maxAngle;

        switch (type)
        {
            case ComponentType.AileronLeft:
            case ComponentType.AileronRight:
                currentAngle = transform.localEulerAngles.x;
                desiredAngle *= parentAircraft._parameters.Flaps;
                if (parentAircraft._parameters.AircraftState != AircraftState.TAXI)
                    desiredAngle *= dir;
                break;
            case ComponentType.Elevator:
                currentAngle = transform.localEulerAngles.x;
                desiredAngle *= parentAircraft._parameters.Elevator;
                break;
            case ComponentType.Rudder:
                currentAngle = transform.localEulerAngles.y;
                desiredAngle *= parentAircraft._parameters.Rudder;
                break;
            default:
                return;
        }

        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * flapSpeed);
        Quaternion rotationQuaternion;

        if (_componentType == ComponentType.Rudder)
        {
            rotationQuaternion = Quaternion.AngleAxis(angle, Vector3.up);
        }
        else
        {
            rotationQuaternion = Quaternion.AngleAxis(angle, Vector3.right);
        }

        transform.localRotation = rotationQuaternion;
    }

    // Update is called once per frame
    void Update()
    {
        SetFlapsRotation(_componentType);
        if (_componentType == ComponentType.Propeller)
        {
            transform.Rotate(Vector3.forward, parentAircraft._parameters.Rpm);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + LiftVector3 * 10);
    }

    void FixedUpdate()
    {
        if (_componentType == ComponentType.AileronRight || _componentType == ComponentType.AileronLeft)
        {
            float curAngle = transform.localEulerAngles.x;
            if (curAngle >= 180)
            {
                curAngle -= 360;
            }

            flapsurface = curAngle / maxAngle * -1;
            wingsurface = 3.95f * flapsurface * parentAircraft._parameters.Velocity;
            LiftVector3 = wingsurface * transform.up;
            if (parentAircraft._parameters.AircraftState == AircraftState.TAXI )
            {
                parentAircraft.GetComponent<Rigidbody>().AddForceAtPosition(LiftVector3, parentAircraft.transform.position);
            }
            else
            {

                parentAircraft.GetComponent<Rigidbody>().AddForceAtPosition(LiftVector3, transform.position);
            }
        }

        if (_componentType == ComponentType.Elevator)
        {
            float curAngle = transform.localEulerAngles.x;
            if (curAngle >= 180)
            {
                curAngle -= 360;
            }

            flapsurface = curAngle / maxAngle;
            wingsurface = 2.95f * flapsurface * parentAircraft._parameters.Velocity;

            LiftVector3 = wingsurface * transform.up;
            parentAircraft.GetComponent<Rigidbody>().AddForceAtPosition(LiftVector3, transform.position);
        }

        if (_componentType == ComponentType.Rudder)
        {
            float curAngle = transform.localEulerAngles.y;
            curAngle = Mathf.Round(curAngle * 10f) / 10f;
            if (curAngle > 180)
            {
                curAngle -= 360;
            }

            flapsurface = curAngle / maxAngle;
            wingsurface = 1.95f * flapsurface * parentAircraft._parameters.Velocity;

            LiftVector3 = wingsurface * -transform.right;
            parentAircraft.GetComponent<Rigidbody>().AddForceAtPosition(LiftVector3, transform.position);
        }
    }


    void LateUpdate()
    {
    }
}