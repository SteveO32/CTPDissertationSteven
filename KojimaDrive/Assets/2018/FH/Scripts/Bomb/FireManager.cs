using System.Collections.Generic;
using UnityEngine;



namespace FH
{
    public class FireManager : ScriptableObject
    {
        public static List<SphereCollider> FirePoints = new List<SphereCollider>();
        public static float DecreaseRate = 1f;


    }
}