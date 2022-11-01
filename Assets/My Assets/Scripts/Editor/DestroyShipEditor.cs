using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DestroyShip))]
public class DestroyShipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DestroyShip ship = (DestroyShip)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Blow up"))
        {
            ship.Detonate(0);
        }
    }
}
