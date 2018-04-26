using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LT
{
    public class PlayerMaterialsApplier : MonoBehaviour {

        public bool applyOnStart;

        public int playerIndex;

        [SerializeField]
        List<SkinnedMeshRenderer> playerModels;

        [SerializeField]
        List<MaterialSelector> mainColorMaterials;

        [SerializeField]
        List<MaterialSelector> secondaryColorMaterials;

        [SerializeField]
        List<MaterialSelector> accentsColorMaterials;

        // Use this for initialization
        void Start() {
            if (applyOnStart)
            {
                ApplyMaterial();
            }
        }

        public void ApplyMaterial()
        {
            
            PlayerCollection coll = PlayerMaterialsManager.getPlayerCollection(playerIndex);

            foreach (var rend in playerModels)
            {
                rend.sharedMesh = coll.humanoidMesh;
                rend.material = coll.humanoidMaterial;
            }
            foreach (var r in mainColorMaterials)
            {
                var materials = r.rend.materials;
                materials[r.materialIndex] = coll.mainColor;
                r.rend.materials = materials;
            }

            foreach (var r in secondaryColorMaterials)
            {
                var materials = r.rend.materials;
                materials[r.materialIndex] = coll.secondaryColor;
                r.rend.materials = materials;
            }
            foreach (var r in accentsColorMaterials)
            {
                var materials = r.rend.materials;
                materials[r.materialIndex] = coll.accents;
                r.rend.materials = materials;
            }
        }
    }
}