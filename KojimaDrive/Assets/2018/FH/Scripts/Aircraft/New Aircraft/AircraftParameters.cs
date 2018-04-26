using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public struct AircraftParameters
{
    public AircraftState AircraftState { get; set; }
    public float AngleOfAttack { get; set; }
    public float Velocity { get; set; }
    public float AngularVelocity { get; set; }
    public float Rpm { get; set; }
    public float Throttle { get; set; }
    public float Rudder { get; set; }
    public float Elevator { get; set; }

    private float _flaps;
    public float Flaps
    {
        get { return _flaps; }
        set
        {
            _flaps = AircraftState == AircraftState.TAXI ? Elevator : value;
        }
    }


    public void SetThrottlePosition(float throttle)
    {
        Throttle += throttle;
        if (Throttle > 1000)
        {
            Throttle = 1000;
        }
        else if (Throttle < 0)
        {
            Throttle = 0;
        }
    }
    
}