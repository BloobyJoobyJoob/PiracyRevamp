using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Shatter))]
public class ShatterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Shatter shatter = (Shatter)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Initialize Fragments From Children"))
        {
            shatter.InitializeFrags(shatter.explodePosition.position);
        }

        if (GUILayout.Button("Explode Fragments"))
        {
            shatter.ExplodeFrags(shatter.explosionForce, shatter.upwardsMultiplier);
        }

        if (GUILayout.Button("Create Fragment Colliders"))
        {
            shatter.CreateCols();
        }

        if (GUILayout.Button("Destroy Fragment Colliders"))
        {
            shatter.DestroyColliders();
        }
    }
}
