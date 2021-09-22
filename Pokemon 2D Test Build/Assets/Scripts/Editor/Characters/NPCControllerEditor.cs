using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(NpcController))]
public class NPCControllerEditor : Editor
{
    public string npcBase;

    void OnEnable()
    {
        npcBase = CurrentSpriteInfo();
    }

    public override void OnInspectorGUI()
    {
        string previousValue = npcBase;

        base.OnInspectorGUI();

        npcBase = CurrentSpriteInfo();

        if (previousValue != npcBase)
        {
            NpcController trainerController = (NpcController)target;
            Entity entity = (Entity)target;

            entity.characterArt = trainerController.GetNPCBase.GetCharacterArt;
            entity.GetComponentInChildren<SpriteRenderer>().sprite = (Sprite)Array.Find<Object>(Resources.LoadAll
                (SavingSystem.GetAssetPath(entity.characterArt.GetOverworldSpriteSheet)), x => x.name == "Down_Idle");
        }
    }

    string CurrentSpriteInfo()
    {
        return SavingSystem.GetAssetPath(((NpcController)target).GetNPCBase);
    }
}