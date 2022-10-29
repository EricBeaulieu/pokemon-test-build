using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(EntityAI), true)]
public class EntityAIEditor : EntityEditor
{
    //EntityAI formBase = null;
    //protected void OnEnable()
    //{
    //    formBase = (EntityAI)target;
    //}

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying || GlobalTools.isInPrefabStage())
        {
            return;
        }

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Path"))
        {
            ((EntityAI)target).GeneratePathToPosition();
        }

        GUILayout.EndHorizontal();
    }
}

#if UNITY_EDITOR
//public class DogiFormBaseDrawer : Editor
//{


//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        var source = formBase._base;
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("_base"));
//        MakeRow(formBase.name, source.Name, "name", false);
//        MakeRow(formBase.form, source.Form, "form", false);
//        MakeRow(formBase.evolutions, source.Evolutions, "evolutions", false);
//        MakeRow(formBase.description, source.Description, "description", true);
//        MakeRow(formBase.sprite, source.Sprite, "sprite", false);
//        MakeRow(formBase.type1, source.Type1, "type1", false);
//        MakeRow(formBase.type2, source.Type2, "type2", false);
//        MakeRow(formBase.growthRate, source.GrowthRate, "growthRate", false);
//        MakeRow(formBase.tameType, source.TameType, "tameType", false);
//        MakeRow(formBase.tameDifficulty, source.TameDifficulty, "tameDifficulty", false);
//        MakeRow(formBase.maxHp, source.MaxHp, "maxHp", false);
//        MakeRow(formBase.maxSp, source.MaxSp, "maxSp", false);
//        MakeRow(formBase.attack, source.Attack, "attack", false);
//        MakeRow(formBase.defense, source.Defense, "defense", false);
//        MakeRow(formBase.spAttack, source.SpAttack, "spAttack", false);
//        MakeRow(formBase.spDefense, source.SpDefense, "type2", false);
//        MakeRow(formBase.speed, source.Speed, "speed", false);
//        MakeRow(formBase.expYield, source.ExpYield, "expYield", false);
//        MakeRow(formBase.learnableMoves, source.LearnableMoves, "learnableMoves", false);
//        MakeRow(formBase.learnableTMs, source.LearnableTMs, "learnableTMs", false);
//        MakeRow(formBase.possibleAbilities, source.PossibleAbilities, "possibleAbilities", false);
//        MakeRow(formBase.rareAbility, source.RareAbility, "rareAbility", false);
//        serializedObject.ApplyModifiedProperties();
//    }

//    private void MakeRow<T>(CheckedField<T> property, T defaultVal, string name, bool textArea)
//    {
//        EditorGUILayout.BeginHorizontal();
//        property.Checked = EditorGUILayout.Toggle(GUIContent.none, property.Checked, GUILayout.Width(30));
//        var valField = serializedObject.FindProperty(name + ".Value");
//        var previousGUIState = GUI.enabled;
//        if (!property.Checked)
//        {
//            property.Value = defaultVal;
//            GUI.enabled = false;
//        }
//        if (textArea && typeof(T) == typeof(string))
//        {
//            EditorGUILayout.LabelField(name, GUILayout.Width(100));
//            property.Value = (T)(object)EditorGUILayout.TextArea(property.Value as string, GUILayout.Height(50));
//        }
//        else
//            EditorGUILayout.PropertyField(valField, new GUIContent(name), true);
//        GUI.enabled = previousGUIState;
//        EditorGUILayout.EndHorizontal();
//    }
//}
#endif
