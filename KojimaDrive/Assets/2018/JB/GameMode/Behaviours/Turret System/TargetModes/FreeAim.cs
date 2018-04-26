using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{
    [CreateAssetMenu(fileName = "FreeAim", menuName = "JB/TurretSystem/Targetmodes/FreeAim")]
    public class FreeAim : TargetMode
    {
        public override List<TargetInfo> GetTargets(TurretTargetInfo _turretTargetInfo)
        {
            List<TargetInfo> targets = new List<TargetInfo>
            {
                new TargetInfo()// Free aim only returns null target
                {
                    trackTarget = null,
                    vectorTarget = null
                }
            };
            return targets;
        }
    }
}// namespace JB
