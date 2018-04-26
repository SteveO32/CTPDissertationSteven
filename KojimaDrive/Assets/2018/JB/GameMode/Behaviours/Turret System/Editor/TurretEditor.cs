using UnityEditor;
using UnityEngine;

namespace JB
{
    [CustomEditor(typeof(Turret))]
    public class TurretEditor : Editor
    {
        Turret turret = null;
        Editor defaultSettingsEditor = null;
        Editor overchargedSettingsEditor = null;


        private void GUIStart()
        {
            serializedObject.Update();
            turret = (Turret)target;
        }


        private void GUIEnd()
        {
            serializedObject.ApplyModifiedProperties();//apply the changed properties
            SceneView.RepaintAll();//to update draw bounds gizmo
            defaultSettingsEditor = null;
            overchargedSettingsEditor = null;
            Repaint();
        }


        private void StartGroup(string _groupName)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");//create a box field in the inspector
            EditorGUILayout.LabelField(_groupName, EditorStyles.largeLabel);
            EditorGUILayout.Space();
        }


        private void EndGroup()
        {
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();//end current box field
        }


        public override void OnInspectorGUI()
        {
            GUIStart();
            DrawCustomInspector();
            GUIEnd();
        }


        private void DrawCustomInspector()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("barrelExits"), true);
            DrawDefaultSettings();
            DrawOverchargedSettings();
        }


        private void DrawDefaultSettings()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultSettings"));
            StartGroup("Default Settings");      
            EditorGUILayout.Space();

            if (defaultSettingsEditor == null)
            {
                Object defaultSettings = serializedObject.FindProperty("defaultSettings").objectReferenceValue;

                if (defaultSettings != null)
                    defaultSettingsEditor = Editor.CreateEditor(defaultSettings);
            }

            if (defaultSettingsEditor != null)
                defaultSettingsEditor.DrawDefaultInspector();

            EndGroup();
        }


        private void DrawOverchargedSettings()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("overchargedSettings"));
            StartGroup("Overcharged Settings");
            EditorGUILayout.Space();

            if (overchargedSettingsEditor == null)
            {
                Object overchargedSettings = serializedObject.FindProperty("overchargedSettings").objectReferenceValue;

                if (overchargedSettings != null)
                    overchargedSettingsEditor = Editor.CreateEditor(overchargedSettings);
            }

            if (overchargedSettingsEditor != null)
                overchargedSettingsEditor.DrawDefaultInspector();

            EndGroup();
        }

    }
}
