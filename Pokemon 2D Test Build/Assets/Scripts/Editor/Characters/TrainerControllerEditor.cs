using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(TrainerController))]
public class TrainerControllerEditor : EntityEditor
{
    string trainerName;

    void OnEnable()
    {
        trainerName = CurrentTrainerInfo(((TrainerController)target).TrainerName);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying || GlobalTools.isInPrefabStage())
        {
            return;
        }
        string previousValue = target.name;

        trainerName = CurrentTrainerInfo(((TrainerController)target).TrainerName);

        if (previousValue != trainerName)
        {
            target.name = trainerName;
        }
    }

    string CurrentTrainerInfo(string name)
    {
        return $"{name}_Trainer";
    }

}