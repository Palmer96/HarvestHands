using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovementTracker))]
public class TrackerEditor : Editor {

    // Use this for initialization
    public override void OnInspectorGUI()
    {
        MovementTracker myTarget = (MovementTracker)target;

        DrawDefaultInspector();

        EditorGUILayout.HelpBox("Functions", MessageType.Info);


        if (GUILayout.Button("Load"))
        {
            myTarget.Load();
        }

        if (GUILayout.Button("Load2"))
        {
            myTarget.Load2();
        }

        if (GUILayout.Button("Load All"))
        {
            myTarget.LoadAll();
        }

        if (GUILayout.Button("Clear"))
        {
            myTarget.Clear();
        }
    }
}
