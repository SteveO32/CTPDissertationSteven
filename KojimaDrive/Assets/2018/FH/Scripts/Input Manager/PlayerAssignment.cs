using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//===================== Kojima PArty - FluffyHedgehog 2018 ====================//
//
// Author:		Dudley 
// Purpose:		This script should be assigned to an object in the Mini-Game menu. It will 
//                  randomly assign players their respective roles in the mini-game, depending
//                  on the information assigned in the editor.
// Namespace:	FH
//
//===============================================================================//


namespace FH
{
    [System.Flags]
    public enum MapCatergories
    {
        None = 0,
        Car = 1,
        Aircraft = 2,
        Boat = 4,
        Turret = 8,
        UI = 16,
        Assignment = 32
    }

    public class PlayerAssignment : MonoBehaviour
    {
        
    }
}
