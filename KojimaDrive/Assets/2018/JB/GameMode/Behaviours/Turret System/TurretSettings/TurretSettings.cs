using System;
using System.Collections.Generic;
using UnityEngine;


namespace JB
{
    [Serializable]
    [CreateAssetMenu(fileName = "TurretSetting", menuName = "JB/TurretSystem/TurretSetting", order = 1)]
    public class TurretSettings : ScriptableObject
    {
        public int damageModifier = 1;
        public float cooldownDuration = 2;
        public float overheatTemperature = 5;
        public AnimationCurve cooldownCurve = new AnimationCurve();
        public AnimationCurve overheatCurve = new AnimationCurve();

        [Space]
        [SerializeField] GunReticule reticulePrefab = null;
        [SerializeField] Projectile projectilePrefab = null;

        [Space]
        [SerializeField] FireMode fireMode = null;
        [SerializeField] TargetMode targetMode = null;


        public GunReticule ReticulePrefab
        {
            get { return reticulePrefab; }
        }


        public Projectile ProjectilePrefab
        {
            get { return projectilePrefab; }
        }


        public FireMode FireMode
        {
            get { return fireMode; }
            set { fireMode = value; }
        }


        public TargetMode TargetMode
        {
            get { return targetMode; }
            set { targetMode = value; }
        }

    }
}
