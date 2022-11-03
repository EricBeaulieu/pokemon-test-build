using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DestroyableObject), true)]
public class DestroyableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying || GlobalTools.isInPrefabStage())
        {
            return;
        }

        if (GUILayout.Button("Snap To Grid"))
        {
            DestroyableObject destroyableObject = (DestroyableObject)target;
            destroyableObject.transform.SnapToGrid();
        }
    }
}
