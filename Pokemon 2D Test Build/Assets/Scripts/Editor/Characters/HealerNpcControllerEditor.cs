using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

[CustomEditor(typeof(HealerNpcController))]
public class HealerNpcControllerEditor : Editor
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
            HealerNpcController trainerController = (HealerNpcController)target;
            Entity entity = (Entity)target;

            entity.characterArt = trainerController.GetHealerNpcBase.GetCharacterArt;
            entity.GetComponentInChildren<SpriteRenderer>().sprite = (Sprite)Array.Find<Object>(Resources.LoadAll
                (SavingSystem.GetAssetPath(entity.characterArt.GetOverworldSpriteSheet)), x => x.name == "Down_Idle");
        }
    }

    string CurrentSpriteInfo()
    {
        return SavingSystem.GetAssetPath(((HealerNpcController)target).GetHealerNpcBase);
    }
}
