using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    [CreateAssetMenu(fileName = "PlayerCollection", menuName = "New Player Collection", order = 1)]
    public class PlayerCollection : ScriptableObject
    {
        public bool selected;

        // --- Humanoid ---
        public Mesh humanoidMesh;
        public Material humanoidMaterial;


        public Material mainColor;
        public Material secondaryColor;
        public Material accents;


        public Color playerColor;
        //// --- Plane ---
        //public Material plane;
        
        //// --- Fire Engine ---
        //public Material fireEngine;
       
        //// --- Cars ---
        //public Material car;
        
        //// --- Tanks ---
        //public Material tank;



    }
}