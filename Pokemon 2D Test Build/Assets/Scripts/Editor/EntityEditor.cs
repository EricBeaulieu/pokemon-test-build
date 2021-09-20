using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Entity))]
public class EntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TrainerController trainerController = (TrainerController)target;
        NpcController npcController = (NpcController)target;
        HealerNpcController healerNpcController = (HealerNpcController)target;

        Entity entity = (Entity)target;
        string spritePath;

        if (trainerController != null)
        {
            entity.characterArt = trainerController.GetTrainerBase.GetCharacterArt;
            spritePath = $"{AssetDatabase.GetAssetPath(entity.characterArt.GetOverworldSpriteSheet)}/Down_Idle";
            entity.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath);
        }
        else if(npcController != null)
        {
            entity.characterArt = npcController.GetNPCBase.GetCharacterArt;
            spritePath = $"{AssetDatabase.GetAssetPath(entity.characterArt.GetOverworldSpriteSheet)}/Down_Idle";
            entity.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath);
        }
        else if (healerNpcController != null)
        {
            entity.characterArt = healerNpcController.GetHealerNpcBase.GetCharacterArt;
            spritePath = $"{AssetDatabase.GetAssetPath(entity.characterArt.GetOverworldSpriteSheet)}/Down_Idle";
            entity.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath);
        }
    }
}
