using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JB
{
    public class MassMaterialSet : EditorWindow
    {
        [Serializable]
        class MaterialSwap
        {
            public Material newMaterial;
            public Material targetMaterial;
        }

        [Serializable]
        class MaterialList : ScriptableObject
        {
            [SerializeField] public List<MaterialSwap> materialsToSwap = new List<MaterialSwap>();
        }

        static MaterialList materialList = null;
        static SerializedObject serializedObject = null;
        

        //[MenuItem("Window/Mass Material Set")]
        public static void Init()
        {
            InitRefs();

            MassMaterialSet window = (MassMaterialSet)GetWindow(typeof(MassMaterialSet));
            window.Show();
            
        }


        public static void InitRefs()
        {
            materialList = new MaterialList();
            serializedObject = new SerializedObject(materialList);
        }


        public void OnGUI()
        {
            if (serializedObject == null || materialList == null)
                InitRefs();

            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("materialsToSwap"), true);      

            if (GUILayout.Button("Swap Selected Materials"))
                SwapAllMaterialsForSelection(Selection.gameObjects);

            serializedObject.ApplyModifiedProperties();
        }


        private void SwapAllMaterialsForSelection(GameObject[] _selection)
        {
            
            if (_selection == null)
                return;

            Undo.RecordObjects(_selection, "Undo Materials Swap");

            foreach (var gameObject in _selection)
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();

                if (renderer == null)
                    continue;

                if (renderer.sharedMaterials == null)
                    continue;

                for(uint i = 0; i < renderer.sharedMaterials.Length; ++i)
                {
                    foreach (MaterialSwap materialSwap in materialList.materialsToSwap)
                    {
                        Debug.Log(renderer.sharedMaterials[i].name);
                        if (renderer.sharedMaterials[i].name == materialSwap.targetMaterial.name)
                        {
                            renderer.sharedMaterials[i] = materialSwap.newMaterial;
                        }
                    }
                }
            }
        }

    }
}// Namespace JB