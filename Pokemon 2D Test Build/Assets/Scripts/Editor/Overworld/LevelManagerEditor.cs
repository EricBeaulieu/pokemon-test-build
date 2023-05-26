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

        if (GUILayout.Button("Encounter Totals"))
        {
            Debug.Log(levelManager.GetCurrentListCount());
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Spawn Pokemon"))
        {
            levelManager.SpawnInPokemon();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Auto Set Scaling"))
        {
            levelManager.CleanUpLevelSize();
        }

        GUILayout.EndHorizontal();

    }
}
