using System;
using System.Collections.Generic;
using UnityEngine;


namespace JB
{
    [Serializable]
    public abstract class TargetMode : ScriptableObject
    {        
        public abstract List<TargetInfo> GetTargets(TurretTargetInfo _turretTargetInfo);
        public virtual void UpdateTargeting(TurretTargetInfo _turretTargetInfo) { }
        public virtual void TargetStart(TurretTargetInfo _turretTargetInfo) { }
        public virtual void TargetHeld(TurretTargetInfo _turretTargetInfo) { }
        public virtual void TargetEnd(TurretTargetInfo _turretTargetInfo) { }
    }


    public struct TargetInfo
    {
        public Transform trackTarget;
        public Vector3? vectorTarget;
    }



    public struct TurretTargetInfo
    {
        public Vector3 gunForward;
        public Vector3 worldPosition;
        public Vector2 reticulePosition;
    }
}
