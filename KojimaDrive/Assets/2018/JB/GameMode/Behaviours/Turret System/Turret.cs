using System.Collections.Generic;
using UnityEngine;


namespace JB
{
    /*===================== Kojima Party - Team Juice Box 2018 ====================
     Author:	    Tom Turner
     Purpose:	    Modular turret framework, uses sciptable object settings to
                    create new turret behaviours.
     Namespace:	    JB
    ===============================================================================*/
    public class Turret : MonoBehaviour
    {
        [SerializeField] List<Transform> barrelExits = new List<Transform>();
        [SerializeField] TurretSettings defaultSettings = null;
        [SerializeField] TurretSettings overchargedSettings = null;

        private TurretSettings currentSettings = null;
        private GunReticule reticule = null;
        private bool overcharged = false;
        private bool firing = false;
        private bool overheated = false;
        private float temperature = 0;
        private int? playerID = null;


        public int? PlayerID
        {
            set { playerID = value; }
            get { return playerID; }
        }



        public GunReticule Reticule
        {
            get { return reticule; }
            set { reticule = value; }
        }
        

        public void Mount(Vector2 _reticulePos)
        {
            gameObject.SetActive(true);

            if (reticule == null)
                return;

            reticule.AnchoredPosition = _reticulePos;
            reticule.gameObject.SetActive(true);
        }


        public void UnMount()
        {
            reticule.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }



        public void Install(Color _turretColor, RectTransform _rect)
        {
            Install(_rect);// Regular Install
            SetColour(_turretColor);// Set turrets colour
        }


        public virtual void Install(RectTransform _rect)
        {
            InstantiateSettings();// Unique instances allows tweaking per gun at runtime
            CreateReticule(_rect);
        }


        private void InstantiateSettings()
        {
            if (defaultSettings != null)
            {
                FireMode fireTemp = defaultSettings.FireMode;
                TargetMode targetTemp = defaultSettings.TargetMode;
                defaultSettings = Instantiate(defaultSettings);
                defaultSettings.FireMode = Instantiate(fireTemp);
                defaultSettings.FireMode.FireAction = FireProjectile;// Give fire mode fire projectile delegate 
                defaultSettings.TargetMode = Instantiate(targetTemp);
            }

            if (overchargedSettings != null)
            {
                FireMode fireTemp = overchargedSettings.FireMode;
                TargetMode targetTemp = overchargedSettings.TargetMode;
                overchargedSettings = Instantiate(overchargedSettings);
                overchargedSettings.FireMode =Instantiate(fireTemp);
                overchargedSettings.FireMode.FireAction = FireProjectile;// Give fire mode fire projectile delegate 
                overchargedSettings.TargetMode = Instantiate(targetTemp);
            }

            currentSettings = defaultSettings;
        }


        void CreateReticule(RectTransform _rect)
        {
            var clone = Instantiate(defaultSettings.ReticulePrefab);
            if (_rect != null)
                clone.transform.SetParent(_rect);

            reticule = clone.GetComponent<GunReticule>();
            reticule.InitPlayerReticule();
        }


        public void SetColour(Color _turretColour)
        {
            // TODO set material colours

            if (reticule != null)
                reticule.SetColour(_turretColour);
        }


        public void Aim(Vector2 _aimDir, float _aimSpeed)
        {
            reticule.MoveReticule(_aimDir, _aimSpeed);
            transform.LookAt(reticule.GetGunLookTarget());
        }


        public void Fire()
        {
            if (!firing)
            {
                OnTriggerPulled();
            }
            else
            {
                OnTriggerHeld();// If previously firing the trigger is held
            }

            firing = true;
        }


        public void ReleaseFire()
        {
            firing = false;
            OnTriggerReleased();
        }


        private void Update()
        { 
            UpdateTemperature();
            UpdateFiring();           
        }


        private void UpdateFiring()
        {
            if (overheated || !firing)// Only fire if not on cool down and firing
                return;
            currentSettings.TargetMode.UpdateTargeting(GetTurretTargetInfo());
            currentSettings.FireMode.UpdateFiring();
        }


        private void UpdateTemperature()
        {
            if (firing && !overheated)
                RaiseTemperature();// Raise temperature when firing

            if (overheated)
                LowerTemperature();// Lower temperature when overheated
        }


        private void RaiseTemperature()
        {
            temperature += Time.deltaTime /** currentSettings.overheatCurve.Evaluate(temperature)*/;
            if (temperature >= currentSettings.overheatTemperature)
            {
                temperature = currentSettings.cooldownDuration;// Has overheated, start cooldown process
                overheated = true;
            }
        }


        private void LowerTemperature()
        {
            temperature -= Time.deltaTime /** currentSettings.cooldownCurve.Evaluate(temperature)*/;
            if (temperature <= 0)
            {
                temperature = 0;
                overheated = false;// Finished cooldown, no longer overheated
            }
        }


        public void FireProjectile()
        {
            if (currentSettings == null || currentSettings.ProjectilePrefab == null)
                return;

            reticule.GrowReticule();// Grow reticule for every shot

            if (currentSettings.TargetMode == null)// Abort if no target mode
                return;

            List<TargetInfo> targets = currentSettings.TargetMode.GetTargets(GetTurretTargetInfo());
            if (targets.Count <= 0)// If no targets generated abort
                return;

            foreach (Transform barrelExit in barrelExits)
            {
                if (targets.Count <= 0)// If no targets left abort
                    return;

                TargetInfo target = targets[0];// Get first projectile target
                Projectile projectileInstance = Instantiate(currentSettings.ProjectilePrefab,
                    barrelExit.position, Quaternion.LookRotation(barrelExit.forward));// Create projectile

                projectileInstance.Init(new ProjectileRequest()// Create projectile request using target info
                {
                    damageMod = currentSettings.damageModifier,
                    playerID = playerID,
                    tTarget = target.trackTarget,
                    vTarget = target.vectorTarget
                });

                if (targets.Count > 1)// If multiple targets distribute across multiple barrels
                    targets.Remove(target);
            }
        }


        private TurretTargetInfo GetTurretTargetInfo()
        {
            return new TurretTargetInfo()// Create turret information for target module
            {
                gunForward = transform.forward,
                worldPosition = reticule.GetGunLookTarget(),
                reticulePosition = reticule.AnchoredPosition
            };
        }


        public void OnTriggerPulled()
        {
            currentSettings.FireMode.FireStart();
            currentSettings.TargetMode.TargetStart(GetTurretTargetInfo());
        }
       

        public void OnTriggerHeld()
        {
            currentSettings.FireMode.FireHeld();
            currentSettings.TargetMode.TargetHeld(GetTurretTargetInfo());
        }


        public void OnTriggerReleased()
        {
            currentSettings.FireMode.FireEnd();
            currentSettings.TargetMode.TargetEnd(GetTurretTargetInfo());
        }

    }
}
