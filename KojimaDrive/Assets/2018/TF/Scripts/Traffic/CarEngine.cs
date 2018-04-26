using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CarEngine : MonoBehaviour
{
    public Transform path;
    public float maxSteerAngle = 45f;
    public float turnSpeed = 5f;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    public float maxMotorTorque = 80f;
    public float maxBrakeTorque = 150f;
    public float currentSpeed;
    public float maxSpeed = 100f;
    public Vector3 centerOfMass;
    public bool isBraking = false;
    public Texture2D textureNormal;
    public Texture2D textureBraking;
    public Renderer carRenderer;

    [Header("Sensors")] public float sensorLength = 1f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.4f, 4f);
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30f;
    public float avoidMultiplier;

    private List<Transform> nodes;
    private int currectNode = 0;
    private bool avoiding = false;
    private float targetSteerAngle = 0;

    private float _targetSteerAngleNextNode = 0;
    private int layer_mask;

    private Rigidbody myRigidbody;
    private float AngleLimit;
    private float _prevRPM;

    public List<Light> BrakeLights;
    public List<Light> IndicatorLights;

    private bool _cancelIndicator = false;
    private bool _leftIndicator = false;
    private bool _rightIndicator = false;
    private bool _brakeLights = false;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.centerOfMass = centerOfMass;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        layer_mask = LayerMask.GetMask("Default", "Player", "Water", "Car");
    }

    private void FixedUpdate()
    {
        Sensors();
        PowerSteering();
        ApplySteer();
        CheckWaypointDistance();
        LerpToSteerAngle();
        Braking();
        Drive();
        TractionControl();
    }

    private void Sensors()
    {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;

        avoidMultiplier = 0;
        avoiding = false;

        //front right sensor
        sensorStartPos += transform.right * frontSideSensorPosition;

        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength, layer_mask))
        {
            if (hit.transform.tag != "Terrain" || hit.transform.tag != "IgnoreMe")
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                Debug.Log(hit.collider.name);
                avoiding = true;
                avoidMultiplier -= 1f;

                if (hit.transform.CompareTag("Player"))
                {
                    avoidMultiplier -= 1.5f;
                }
            }
        }

        //front right angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward,
            out hit, sensorLength, layer_mask))
        {
            if (hit.transform.tag != "Terrain" || hit.transform.tag != "IgnoreMe")
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                Debug.Log(hit.collider.name);
                avoiding = true;
                avoidMultiplier -= 0.5f;
                if (hit.transform.tag == "Player")
                {
                    avoidMultiplier -= 1.5f;
                }
            }
        }

        //front left sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength, layer_mask))
        {
            if (hit.transform.tag != "Terrain" || hit.transform.tag != "IgnoreMe")
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                Debug.Log(hit.collider.name);
                avoiding = true;
                avoidMultiplier += 1f;
                if (hit.transform.tag == "Player")
                {
                    avoidMultiplier += 1.5f;
                }
            }
        }

        //front left angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward,
            out hit, sensorLength, layer_mask))
        {
            if (hit.transform.tag != "Terrain" || hit.transform.tag != "IgnoreMe")
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                Debug.Log(hit.collider.name);
                avoiding = true;
                avoidMultiplier += 0.5f;
                if (hit.transform.tag == "Player")
                {
                    avoidMultiplier += 1.5f;
                }
            }
        }

        //front center sensor
        if (avoidMultiplier == 0)
        {
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength, layer_mask))
            {
                if (hit.transform.tag != "Terrain" || hit.transform.tag != "IgnoreMe")
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    Debug.Log(hit.collider.name);
                    avoiding = true;
                    if (hit.normal.x < 0)
                    {
                        avoidMultiplier = -1;
                    }
                    else
                    {
                        avoidMultiplier = 1;
                    }
                }
            }
        }

        if (avoiding)
        {
            targetSteerAngle = maxSteerAngle * avoidMultiplier;
        }
    }

    private void ApplySteer()
    {
        if (avoiding) return;

        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currectNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        targetSteerAngle = newSteer;
    }

    private void Drive()
    {
        // Debug.Log("RPM: " + wheelFL.rpm + ", MT: " + wheelFL.motorTorque);
        if (wheelFL.isGrounded && wheelFR.isGrounded && wheelRL.isGrounded && wheelRR.isGrounded)
        {
            //currentSpeed = 2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000;
            //get velocity of the rigidbody
            //transform its direction to local space
            //take z component, at is it blue local axis, i.e. forward velocity
            currentSpeed = transform.InverseTransformDirection(myRigidbody.velocity).z;

            //lets convert it to mph
            currentSpeed *= 2.23694f;

            if (currentSpeed < maxSpeed && !isBraking)
            {
                wheelFL.motorTorque = maxMotorTorque;
                wheelFR.motorTorque = maxMotorTorque;
            }
            else
            {
                wheelFL.motorTorque = 0;
                wheelFR.motorTorque = 0;
            }
        }
    }

    private void CheckWaypointDistance()
    {
        float distance = Vector3.Distance(transform.position, nodes[currectNode].position);

        //swap the node prior to reaching destination
        if (distance < 1f)
        {
            if (currectNode == nodes.Count - 1)
            {
                currectNode = 0;
            }
            else
            {
                currectNode++;
            }

            //reset max Speed
            maxSpeed = 100;
        }

        if (currentSpeed > 10 && distance / currentSpeed < 1.0f)
        {
            //if close to the checkpoint, calculate steer angle required for the next one
            var nextnode = currectNode + 1;
            if (currectNode == nodes.Count - 1)
            {
                nextnode = 0;
            }

            Vector3 relativeVector = transform.InverseTransformPoint(nodes[nextnode].position);
            float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
            _targetSteerAngleNextNode = newSteer;
        }
        else
        {
            _targetSteerAngleNextNode = 0;
        }
    }

    private void Braking()
    {
        if (Mathf.Abs(_targetSteerAngleNextNode) > AngleLimit)
        {
            wheelFL.brakeTorque = maxBrakeTorque;
            wheelFR.brakeTorque = maxBrakeTorque;
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;

            isBraking = true;
        }
        else
        {
            if (isBraking)
            {
                maxSpeed = currentSpeed;
            }

            wheelFL.brakeTorque = 0;
            wheelFR.brakeTorque = 0;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;

            isBraking = false;
        }
    }


    private void LerpToSteerAngle()
    {
        if (targetSteerAngle > AngleLimit)
        {
            targetSteerAngle = AngleLimit;
        }

        wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
        wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle, targetSteerAngle, Time.deltaTime * turnSpeed);
    }

    private static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return ((value - from1) / (to1 - from1) * (to2 - from2)) + from2;
    }

    private void PowerSteering()
    {
        var x = currentSpeed;
        if (x >= 30)
        {
            x = 30;
        }
        else if (x <= 5)
        {
            x = 5;
        }

        AngleLimit = Remap(x, 5, 30, maxSteerAngle, 2);
    }

    private void TractionControl()
    {
        WheelHit xHit;
        wheelFL.GetGroundHit(out xHit);
        if (xHit.forwardSlip > 0.3f)
        {
            wheelFL.motorTorque *= 0.25f;
        }

        wheelFR.GetGroundHit(out xHit);
        if (xHit.forwardSlip > 0.3f)
        {
            wheelFL.motorTorque *= 0.25f;
        }
    }

    void Update()
    {
        HandleLights();
    }

    void HandleLights()
    {
        if (_targetSteerAngleNextNode > 1)
        {
            if (!_rightIndicator)
                RightIndicator();
        }
        else if (_targetSteerAngleNextNode < -1)
        {
            if (!_leftIndicator)
                LeftIndicator();
        }

        ToggleBrakeLights(isBraking);

        if (maxSpeed == 100 && Mathf.Abs(targetSteerAngle) < 1.0f)
        {
            if (!_cancelIndicator)
                CancelIndicators();
        }
    }

    void ToggleBrakeLights(bool on)
    {
        if (isBraking && !_brakeLights)
        {
            foreach (var breakLight in BrakeLights)
            {
                breakLight.intensity = 3;
            }

            _brakeLights = true;
        }
        else if (!isBraking && _brakeLights)
        {
            foreach (var breakLight in BrakeLights)
            {
                breakLight.intensity = 1;
            }

            _brakeLights = false;
        }
    }

    void LeftIndicator()
    {
        CancelIndicators();
        _leftIndicator = true;
        StartCoroutine(Indicator(IndicatorLights[0]));
    }

    void RightIndicator()
    {
        CancelIndicators();
        _rightIndicator = true;
        StartCoroutine(Indicator(IndicatorLights[1]));
    }

    void HazardLights()
    {
        CancelIndicators();
        StartCoroutine(Indicator(IndicatorLights));
    }

    void CancelIndicators()
    {
        StopAllCoroutines();
        _leftIndicator = false;
        _rightIndicator = false;
        foreach (var indicatorLight in IndicatorLights)
        {
            indicatorLight.intensity = 0;
        }
    }

    IEnumerator Indicator(Light light)
    {
        while (!_cancelIndicator)
        {
            light.intensity = 3;
            yield return new WaitForSeconds(0.5f);
            light.intensity = 0f;
            yield return new WaitForSeconds(0.5f);
        }

        _cancelIndicator = false;
    }

    IEnumerator Indicator(List<Light> lights)
    {
        while (!_cancelIndicator)
        {
            foreach (var light1 in lights)
            {
                light1.intensity = 3;
            }

            yield return new WaitForSeconds(0.5f);
            foreach (var light1 in lights)
            {
                light1.intensity = 0f;
            }

            yield return new WaitForSeconds(0.5f);
        }

        _cancelIndicator = false;
    }
}