using System.Collections;
using System.Collections.Generic;
using LPWAsset;
using UnityEngine;

namespace JB
{

public class EnableWater : MonoBehaviour
{
    
    void Awake()
    {
        var water = GetComponent<LowPolyWaterScript>();
        if (water != null)
        {
            water.enabled = true;
        }
    }

}

} // namespace JB
