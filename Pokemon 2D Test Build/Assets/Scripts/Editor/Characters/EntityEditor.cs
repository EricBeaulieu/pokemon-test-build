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

        Entity entity = (Entity)target;
        string spritePath;

        spritePath = $"{AssetDatabase.GetAssetPath(entity.CharacterArt.GetOverworldSpriteSheet)}/Down_Idle";
        entity.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath);
    }
}
