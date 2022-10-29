using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OverworldItem), true)]
public class OverworldItemEditor : Editor
{
    void OnEnable()
    {
        UpdateName();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying || GlobalTools.isInPrefabStage())
        {
            return;
        }

        UpdateName();
    }

    void UpdateName()
    {
        if (((OverworldItem)target).CurrentItem != null)
        {
            OverworldItem overworldItem = (OverworldItem)target;
            target.name = $"{overworldItem.CurrentItem.ItemName} ({overworldItem.CurrentCount})";
        }
    }
}
