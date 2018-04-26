using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//===================== Kojima Drive - FluffyHedgehog 2018 ====================//
//
// Author:		Dudley 
// Purpose:		Base class for vehicle physics scripts, so any vehicle can be followed by the FHCamera
// Namespace:	FH
//
//===============================================================================//

namespace FH
{
    public class VehiclePhysics : MonoBehaviour
    {
        public float m_fCurrentMagnitude { get; protected set; }
        public float m_fYaw              { get; protected set; }
    }
}
