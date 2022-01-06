using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelManager levelManager = (LevelManager)target;

        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Standard Walking Encounter Total"))
            {
                Debug.Log($"Current Level Manager Standard Walking Encounter Total: {levelManager.GetStandardWalkingCount()}");
            }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Standard Surfing Encounter Total"))
            {
                Debug.Log($"Current Level Manager Standard Surfing Encounter Total: {levelManager.GetStandardWalkingCount()}");
            }

        GUILayout.EndHorizontal();
    }
}
