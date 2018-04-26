using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace JB
{

[CustomEditor(typeof(ObjectSpawner))]
public class ObjectSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        JB.ObjectSpawner spawner = (JB.ObjectSpawner)target;

        if (GUILayout.Button("Force Clear Objects"))
        {
            spawner.ForceClearGameObjects();
        }
    }
}

} // namespace JB
