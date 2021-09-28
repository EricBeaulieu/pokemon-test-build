using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(NpcController))]
public class NPCControllerEditor : Editor
{
    public string trainerBase;

    void OnEnable()
    {
        trainerBase = CurrentSpriteInfo();
    }

    public override void OnInspectorGUI()
    {
        string previousValue = trainerBase;

        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Face Up"))
        {
            Debug.Log("Face Up");
            ChangeSprite(FacingDirections.Up);
        }

        if (GUILayout.Button("Face Down"))
        {
            Debug.Log("Face Down");
            ChangeSprite(FacingDirections.Down);
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Face Left"))
        {
            Debug.Log("Face Left");
            ChangeSprite(FacingDirections.Left);
        }

        if (GUILayout.Button("Face Right"))
        {
            Debug.Log("Face Right");
            ChangeSprite(FacingDirections.Right);
        }

        GUILayout.EndHorizontal();

        trainerBase = CurrentSpriteInfo();

        if (previousValue != trainerBase)
        {
            ChangeSprite();
        }
    }

    string CurrentSpriteInfo()
    {
        return SavingSystem.GetAssetPath(((TrainerController)target).GetTrainerBase);
    }

    void ChangeSprite(FacingDirections facingDirections)
    {
        NpcController npcController = (NpcController)target;
        Entity entity = (Entity)target;

        entity.characterArt = npcController.GetNPCBase.GetCharacterArt;
        entity.GetComponentInChildren<SpriteRenderer>().sprite = (Sprite)Array.Find<Object>(Resources.LoadAll
            (SavingSystem.GetAssetPath(entity.characterArt.GetOverworldSpriteSheet)), x => x.name ==
            GlobalTools.FacingDirectionEditorHelper(facingDirections));
    }

    void ChangeSprite()
    {
        NpcController npcController = (NpcController)target;
        Entity entity = (Entity)target;

        entity.characterArt = npcController.GetNPCBase.GetCharacterArt;
        entity.GetComponentInChildren<SpriteRenderer>().sprite = (Sprite)Array.Find<Object>(Resources.LoadAll
            (SavingSystem.GetAssetPath(entity.characterArt.GetOverworldSpriteSheet)), x => x.name ==
            GlobalTools.FacingDirectionEditorHelper(GlobalTools.GetDirectionFacingOnStart(entity)));
    }
}