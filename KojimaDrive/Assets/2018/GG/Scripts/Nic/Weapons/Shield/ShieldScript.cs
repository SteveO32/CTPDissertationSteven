using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GG
{
    public class ShieldScript : WeaponClass
    {
        public float timer;
        GameObject tankObject;
       

        // Use this for initialization
        void Start()
        {

        }

        public override void onUpdate()
        {
            transform.position = tankObject.transform.position;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                tankObject.GetComponent<BasicHealthTest>().isShielded = false;
                Destroy(gameObject);
            }
        }

        public override float weaponFire(GameObject tank)
        {
            if (tank.GetComponent<BasicHealthTest>().isShielded == false)
            {
                GameObject newShield = (GameObject)Instantiate(this.gameObject, tank.transform.position, tank.transform.rotation);
                newShield.GetComponent<ShieldScript>().isInstantiate = true;
                newShield.GetComponent<ShieldScript>().tankObject = tank;
                newShield.GetComponent<ShieldScript>().timer = timer;
                tank.GetComponent<BasicHealthTest>().isShielded = true;

                decreaseWeaponCharges();
            }
            return 0;
        }
    }
}