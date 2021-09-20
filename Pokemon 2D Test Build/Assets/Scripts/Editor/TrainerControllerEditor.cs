using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrainerController))]
public class TrainerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TrainerController trainerController = (TrainerController)target;
        Entity entity = (Entity)target;
        string spritePath;

        if (trainerController != null)
        {
            entity.characterArt = trainerController.GetTrainerBase.GetCharacterArt;
            spritePath = $"{SavingSystem.GetAssetPath(entity.characterArt.GetOverworldSpriteSheet)}";
            Object[] sprites;
            sprites = Resources.LoadAll(spritePath);
            entity.GetComponentInChildren<SpriteRenderer>().sprite = (Sprite)sprites[1];
        }
    }
}
