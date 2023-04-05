using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldZones)), CanEditMultipleObjects]
public class WorldZonesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WorldZones worldZones = (WorldZones)target;

        if(GUILayout.Button("Go To Check Point"))
        {
            worldZones.GoToCurrentCheckPoint();
        }
    }
}
