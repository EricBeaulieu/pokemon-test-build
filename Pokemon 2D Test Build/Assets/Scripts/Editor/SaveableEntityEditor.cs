using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveableEntity))]
public class SaveableEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SaveableEntity saveableEntity = (SaveableEntity)target;

        if(GUILayout.Button("Generate New ID"))
        {
            saveableEntity.GenerateID();
        }
    }
}
