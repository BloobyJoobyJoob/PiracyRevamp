using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

[CustomEditor(typeof(Clouds))]
public class CloudsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Clouds clouds = (Clouds)target;

        if (GUILayout.Button("Create Pool"))
        {
            clouds.GeneratePool();
        }
    }
}
