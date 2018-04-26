using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//===================== Kojima Drive - FluffyHedgehog 2018 ====================//
//
// Author:		Dudley 
// Purpose:		This handles the amount of feul the aircraft has.
// Namespace:	FH
//
//===============================================================================//



namespace FH
{
    public class AircraftFuelTank : MonoBehaviour
    {
        [SerializeField]
        private float defaultFuelSupply = 10000f;
        [SerializeField]
        private float fuelSupply = 0;
        [SerializeField]
        private float speedMultiplier = 0f;

        public bool FuelTankEmpty { get; private set; }



        private void Start()
        {
            Resupply();
        }



        private void Update()
        {
            if(fuelSupply > 0f)
            {
                fuelSupply -= speedMultiplier * Time.deltaTime;
                FuelTankEmpty = false;
            }
            else
            {
                Debug.Log("MESSAGE: Warning aircraft has run out of fuel.");
                FuelTankEmpty = true;
            }
        }


        public void UpdateMultiplier(float speedMultiplier)
        {
            this.speedMultiplier = speedMultiplier;
        }


        public bool Alert()
        {
            if(fuelSupply <= (defaultFuelSupply * 0.25f))
                return true;
            return false;
        }


        public void Resupply()
        {
            fuelSupply = defaultFuelSupply;
        }
    }
}