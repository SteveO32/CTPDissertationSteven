using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum AircraftState
{
    PARKED = 0,
    TAXI = 1,
    FLY = 2,
    STALL = 3,
    CRASH = 4
}

public class AircraftController : MonoBehaviour
{
    [SerializeField] public float Power;
    public float Vel;
    public Vector3 LiftVector3;
    public AircraftState state;
    [SerializeField] public float AOA;
    [SerializeField] public AircraftParameters _parameters;
    [SerializeField] private float maxRPM = 25;
    [SerializeField] private float y;
    //public List<WheelCollider> WheelsColliders;
    public Text text;
    private Vector2 xx;
    public float liftCoe;
    public List<AircraftComponent> AircraftComponents;

    private Rigidbody myRigidbody;

    private float _timer = 0.0f;

    [SerializeField]
    private FH.ControllerID m_controllerID;
    private Rewired.Player m_rewiredPlayer;
    private bool m_dropBomb = false;

    private FH.MissileController m_missleController;



    // Use this for initialization
    void Start()
    {
        m_missleController = GetComponent<FH.MissileController>();
        m_rewiredPlayer = Rewired.ReInput.players.GetPlayer((int)m_controllerID);
        myRigidbody = GetComponent<Rigidbody>();
        AircraftComponents = GetComponentsInChildren<AircraftComponent>().ToList();
        foreach (var aircraftComponent in AircraftComponents)
        {
            aircraftComponent.parentAircraft = this;
        }
    }


    void GetInput()
    {
        _parameters.Elevator = m_rewiredPlayer.GetAxis("Pitch");
        _parameters.Rudder = m_rewiredPlayer.GetAxis("Yaw");
        _parameters.Flaps = m_rewiredPlayer.GetAxis("Roll");
        _parameters.SetThrottlePosition(m_rewiredPlayer.GetAxis("Throttle"));

        m_dropBomb = m_rewiredPlayer.GetButton("Drop Bomb");



        _parameters.Rpm += _parameters.Throttle;
        if (_parameters.Rpm > maxRPM)
        {
            _parameters.Rpm = maxRPM;
        }

        if (_parameters.Rpm < 0)
        {
            _parameters.Rpm = 0;
        }
    }

    public bool air = false;

    // Update is called once per frame
    void Update()
    {
        GetInput();
        // _parameters.PropellerRPM -= Input.GetAxis("Throttle") * Time.deltaTime * 7.55f;
        //_parameters.ThrottlePosition += -Input.GetAxis("Throttle") * 10;

        if(m_dropBomb) m_missleController.FireBombs();


        if(text)
        {
            text.text = "Speed: " + GetComponent<Rigidbody>().velocity.magnitude + "\nAttitude: " + transform.position.y +
                      "\nThrottle: " + _parameters.Throttle;
        }
    }

    void FixedUpdate()
    {
        ProcessAircraftState();

        _parameters.Velocity = CalculateForwardVelocity(myRigidbody.velocity);

        CalculateForces(_parameters);
    }

    public void OnTriggerEnter(Collider col)
{
    if(col.gameObject.tag == "Border")
    {
        this.GetComponentInChildren<LT.UIManager>().warningText_reference.DeactivateWarning();
    }
}

public void OnTriggerExit(Collider col)
{
    if (col.gameObject.tag == "Border")
    {

        this.GetComponentInChildren<LT.UIManager>().warningText_reference.ActivateWarning();
    }
}

    private void ProcessAircraftState()
    {
        state = _parameters.AircraftState;

        var x = GetComponentsInChildren<WheelCollider>();
        air = x[0].isGrounded;
        for (int i = 1; i < x.Length; i++)
        {
            air = air || x[i].isGrounded;
        }

        if (air)
        {
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            foreach (var wheelCollider in x)
            {
                wheelCollider.motorTorque = 0.01f;
            }

            _parameters.AircraftState =
                Math.Abs(_parameters.Velocity) < 0.1f ? AircraftState.PARKED : AircraftState.TAXI;

            x[2].steerAngle = _parameters.Rudder * -5;
        }
        else
        {
            if (_parameters.AircraftState == AircraftState.TAXI)
            {
                _timer += Time.fixedDeltaTime;
                if (_timer >= 2.0f)
                {
                    _parameters.AircraftState = (AircraftState.FLY);
                    _timer = 0;
                }
            }

            myRigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    private float CalculateForwardVelocity(Vector3 velocityVector3)
    {
        var localVelocity = transform.InverseTransformDirection(velocityVector3);
        var velocity = localVelocity.z;
        velocity = Mathf.Round(velocity * 10f) / 10f;
        return velocity;
    }


    private static float CalculateAngleOfAttack(Vector3 forwardVector3, Vector3 rightVector3)
    {
        var forwardWithoutY = new Vector3(forwardVector3.x, 0, forwardVector3.z);

        var angle = 0.0f;

        #region Unity 2017+ - comment out if using earlier version
        //angle = Vector3.SignedAngle(forwardVector3, forwardWithoutY, rightVector3);
        #endregion
        #region Unity pre 2017 - comment out if using 2017 or newer
        angle = Vector3.Angle(forwardVector3, forwardWithoutY);
        var cross = Vector3.Cross(forwardVector3, forwardWithoutY);
        if (Vector3.Dot(rightVector3, cross) < 0)
        {
            angle = -angle;
        }
        #endregion

        return angle;
    }

    private void CalculateForces(AircraftParameters aircraftParameters)
    {
        var liftForce = CalculateLiftForce(CalculateAngleOfAttack(transform.forward, transform.right),
            CalculateAirDensity(101325, 15),
            _parameters.Velocity, 7.95f);
        var thrustForce = _parameters.Throttle * Power;

        //apply forces
        myRigidbody.AddForce(liftForce * transform.up);
        myRigidbody.AddForce(thrustForce * transform.forward);
    }

    /// <summary>
    /// Function calculating air density from given parameters;
    /// https://www.brisbanehotairballooning.com.au/calculate-air-density/
    /// </summary>
    /// <param name="airPressure">in Pa</param>
    /// <param name="temperature">in Celsius</param>
    /// <returns></returns>
    private float CalculateAirDensity(float airPressure, float temperature)
    {
        // todo: const for dry air
        const float gasConst = 287.05f;
        var constTemp = gasConst * CelsiusToKelvin(temperature);
        var densityOfDryAir = airPressure / constTemp;
        return densityOfDryAir;
    }

    /// <summary>
    /// Function converting temperature unit from Celsius to Kelvin
    /// https://en.wikipedia.org/wiki/Kelvin
    /// </summary>
    /// <param name="celsiusTemperature">Temperature in Celsius</param>
    /// <returns></returns>
    private static float CelsiusToKelvin(float celsiusTemperature)
    {
        return celsiusTemperature + 273.15f;
    }

    /// <summary>
    /// Function calculating lift force for the airfoil from given parameters
    /// https://wright.nasa.gov/airplane/lifteq.html
    /// </summary>
    /// <param name="attackOfAngle">Attack angle of the airfoil in degrees</param>
    /// <param name="airDensity">Density of air in kg/m3</param>
    /// <param name="velocity">Forward) velocity in m/s </param>
    /// <param name="surfaceArea">Surface area of an airfoil in square meters</param>
    /// <returns></returns>
    private float CalculateLiftForce(float attackOfAngle, float airDensity, float velocity, float surfaceArea)
    {
        var liftCoefficient = CalculateLiftCoefficient(attackOfAngle);

        var dynamicPressure = airDensity * velocity * velocity * 0.5f;

        var liftForce = liftCoefficient * dynamicPressure * surfaceArea;

        return liftForce;
    }

    /// <summary>
    /// Function calculating lift coefficient for a cambered airfoil from given attack angle
    /// Based on lift curve of SM701 airfoil
    /// https://en.wikipedia.org/wiki/Lift_coefficient#/media/File:Lift_curve.svg
    /// </summary>
    /// <param name="attackOfAngle">Attack angle of the airfoil</param>
    /// <returns></returns>
    private static float CalculateLiftCoefficient(float attackOfAngle)
    {
        var liftCoefficient = 0.0f;

        if (attackOfAngle >= -5 && attackOfAngle < 12.5)
        {
            liftCoefficient = attackOfAngle * 0.1f + 0.5f;
        }
        else if (attackOfAngle >= 12.5 && attackOfAngle < 17)
        {
            liftCoefficient = 1.7f;
        }
        else if (attackOfAngle >= 17 && attackOfAngle < 25)
        {
            liftCoefficient = -0.03125f * attackOfAngle + 2.23125f;
        }
        else
        {
            liftCoefficient = 0;
        }

        return liftCoefficient;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);

        Vector3 drawVector3 = LiftVector3;
        drawVector3.Normalize();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + drawVector3);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);

        Vector3 xx = new Vector3(transform.forward.x, 0, transform.forward.z);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + xx);
    }

    void OnCollisionEnter(Collision collision)
    {
    }
}
