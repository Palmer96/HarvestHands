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


        if (GUILayout.Button("Load Single"))
        {
            myTarget.LoadSingle();
        }

        if (GUILayout.Button("Load All"))
        {
            myTarget.LoadAll();
        }

        if (GUILayout.Button("Fake Pos"))
        {
            myTarget.FakePos();
        }
        if (GUILayout.Button("Fake Load"))
        {
            myTarget.LoadFake();
        }

        if (GUILayout.Button("Clear"))
        {
            myTarget.Clear();
        }
    }
}
