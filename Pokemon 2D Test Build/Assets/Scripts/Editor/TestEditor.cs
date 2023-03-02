using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Test))]
public class TestEditor : Editor
{
    PokemonBase currentPokemon;

    void OnEnable()
    {
        currentPokemon = ((Test)target).CurrentPokemon;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying || GlobalTools.isInPrefabStage())
        {
            return;
        }
        PokemonBase previousValue = ((Test)target).CurrentPokemon;

        if (previousValue != currentPokemon)
        {
            currentPokemon = previousValue;
            ((Test)target).UpdateInfo(currentPokemon);
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Update Pokemon"))
        {
            ((Test)target).UpdateInfo(currentPokemon);
        }

        GUILayout.EndHorizontal();
    }

}