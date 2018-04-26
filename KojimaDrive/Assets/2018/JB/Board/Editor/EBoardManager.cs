using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace JB
{

[CustomEditor(typeof(BoardManager))]
public class EBoardManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BoardManager script = (BoardManager)target;

        if (GUILayout.Button("Move Player 1 to next point"))
        {
            if (Application.isPlaying)
            {
                script.MovePlayerToNextPoint(0);
            }
        }

        if (GUILayout.Button("Move Player 2 to next point"))
        {
            if (Application.isPlaying)
            {
                script.MovePlayerToNextPoint(1);
            }
        }

        if (GUILayout.Button("Move Player 3 to next point"))
        {
            if (Application.isPlaying)
            {
                script.MovePlayerToNextPoint(2);
            }
        }

        if (GUILayout.Button("Move Player 4 to next point"))
        {
            if (Application.isPlaying)
            {
                script.MovePlayerToNextPoint(3);
            }
        }
    }
}

} // namespace JB
