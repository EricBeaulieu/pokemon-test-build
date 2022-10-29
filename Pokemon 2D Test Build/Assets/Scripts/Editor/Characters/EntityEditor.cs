using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Entity),true)]
public class EntityEditor : Editor
{
    public string characterArt;

    void OnEnable()
    {
        if (Application.isPlaying) return;
        if(characterArt != null)
        characterArt = CurrentSpriteInfo();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying || GlobalTools.isInPrefabStage())
        {
            return;
        }

        if (target is WildPokemonController)
        {
            return;
        }
        Entity entity = (Entity)target;
        string previousValue = characterArt;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Snap To Grid"))
        {
            entity.SnapToGrid();
        }

        GUILayout.EndHorizontal();

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

        characterArt = CurrentSpriteInfo();

        if (previousValue != characterArt)
        {
            ChangeSprite();
        }
    }

    string CurrentSpriteInfo()
    {
        return SavingSystem.GetAssetPath(((Entity)target).CharacterArt);
    }

    void ChangeSprite(FacingDirections facingDirections)
    {
        EntityAI entity = (EntityAI)target;

        entity.GetComponentInChildren<SpriteRenderer>().sprite = (Sprite)Array.Find<Object>(Resources.LoadAll
            (SavingSystem.GetAssetPath(entity.CharacterArt.GetOverworldSpriteSheet)), x => x.name ==
            GlobalTools.FacingDirectionEditorHelper(facingDirections));
    }

    void ChangeSprite()
    {
        EntityAI entity = (EntityAI)target;

        entity.GetComponentInChildren<SpriteRenderer>().sprite = (Sprite)Array.Find<Object>(Resources.LoadAll
            (SavingSystem.GetAssetPath(entity.CharacterArt.GetOverworldSpriteSheet)), x => x.name ==
            GlobalTools.FacingDirectionEditorHelper(GlobalTools.GetDirectionFacingOnStart(entity)));
    }
}
