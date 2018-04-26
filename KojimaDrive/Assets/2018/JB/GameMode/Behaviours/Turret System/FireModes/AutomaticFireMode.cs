using System.Collections;
using System.Collections.Generic;
using JB;
using UnityEngine;


namespace JB
{
    [CreateAssetMenu(fileName = "AutomaticFireMode", menuName = "JB/TurretSystem/Firemodes/AutomaticFireMode")]
    public class AutomaticFireMode : FireMode
    {
        private float bulletDelayTimer = 0.0f;


        public override void UpdateFiring()
        {
            if (bulletDelayTimer <= 0)
            {
                Fire();
                bulletDelayTimer = bulletDelay;
            }
            else
            {
                bulletDelayTimer -= Time.deltaTime;
            }
        }
    }

}
