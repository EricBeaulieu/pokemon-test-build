using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class GlobalArt
{

    public static Sprite nothing { get { return (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Nothing.png", typeof(Sprite)); } }
    public static Sprite blankWhite { get { return (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/White1x1.jpg", typeof(Sprite)); } }

    //Conditions
    public static Sprite faintedStatus { get { return (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Battle/BattleHUD/Conditions/Fainted.png", typeof(Sprite)); } }

    //Status Condition
    static Color conditionPoisonColour { get { return new Color(0.576232f, 0, 1); } }
    static Color conditionBurnColour { get { return new Color(1, 0.4218157f, 0); } }
    static Color conditionParalyzedColour { get { return new Color(1, 0.8065821f, 0); } }
    static Sprite[] conditionParalyzedStatic { get { return AssetDatabase.LoadAllAssetsAtPath("Assets/Art/Battle/Status/Afflictions/StatusAfflictionParalyzedAnimations.png").OfType<Sprite>().ToArray(); } }
    static Color conditionFrozenColour { get { return new Color(0, 1, 1); } }

    //HitPoint Colours
    static Color highHP { get { return new Color(0, 0.7843138f, 0); } }
    static Color mediumHP { get { return new Color(0.8962264f, 0.7208233f, 0); } }
    static Color lowHP { get { return new Color(1, 0, 0); } }

    static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    static Sprite overworldSpriteNullReference = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/OverworldNull.png", typeof(Sprite));

    public static Sprite ReturnStatusConditionArt(ConditionID currentCondition)
    {
        if(currentCondition >= ConditionID.Poison && currentCondition <= ConditionID.ToxicPoison)
        {
            if (currentCondition == ConditionID.ToxicPoison)
            {
                return (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Battle/BattleHUD/Conditions/Poison.png", typeof(Sprite));
            }
            return (Sprite)AssetDatabase.LoadAssetAtPath($"Assets/Art/Battle/BattleHUD/Conditions/{currentCondition}.png", typeof(Sprite));
        }
        return nothing;
    }

    public static Color GetStatusConditionAnimationColour(ConditionID conditionID)
    {
        switch (conditionID)
        {
            case ConditionID.Poison:
                return conditionPoisonColour;
            case ConditionID.Burn:
                return conditionBurnColour;
            case ConditionID.Paralyzed:
                return conditionParalyzedColour;
            case ConditionID.Frozen:
                return conditionFrozenColour;
            case ConditionID.ToxicPoison:
                return conditionPoisonColour;
        }
        return Color.white;
    }

    public static Sprite GetRandomParalzedConditionAnimationArt()
    {
        return conditionParalyzedStatic[Random.Range(0, conditionParalyzedStatic.Length)];
    }

    public static Sprite ReturnGenderArt(bool? gender)
    {
        if(gender.HasValue)
        {
            if(gender.Value == true)
            {
                return (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Battle/BattleHUD/Male.png", typeof(Sprite));
            }
            else
            {
                return (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Battle/BattleHUD/Female.png", typeof(Sprite));
            }
        }
        return nothing;
    }

    public static Sprite ReturnElementArt(ElementType type)
    {
        return (Sprite)AssetDatabase.LoadAssetAtPath($"Assets/Art/Pokemon/Types/{type}.png", typeof(Sprite));
    }

    public static Sprite ReturnMoveCategoryArt(MoveType type)
    {
        return (Sprite)AssetDatabase.LoadAssetAtPath($"Assets/Art/Pokemon/MoveCategories/{type}.png", typeof(Sprite));
    }

    public static Sprite ReturnStatusChangesArt(StatAttribute stat)
    {
        return (Sprite)AssetDatabase.LoadAssetAtPath($"Assets/Art/Battle/Status/{stat}.jpg", typeof(Sprite));
    }

    public static Color ReturnHitPointsColor(float healthNormalized)
    {
        if(healthNormalized >= HPBar.meduimColourThreshold)
        {
            return highHP;
        }
        else if(healthNormalized < HPBar.meduimColourThreshold && healthNormalized >= HPBar.lowColourThreshold)
        {
            return mediumHP;
        }
        else //healthNormalized < HPBar.lowColourThreshold
        {
            return lowHP;
        }
    }

    public static Sprite ReturnTMArt(ElementType type)
    {
        if(type == ElementType.NA)
        {
            return nothing;
        }
        return (Sprite)AssetDatabase.LoadAssetAtPath($"Assets/Art/Items/TMHM/{type}.png", typeof(Sprite));
    }

    #region

    public static Sprite GetSprite(string spriteName)
    {
        if (sprites.TryGetValue(spriteName, out Sprite spriteValue))
        {
            return spriteValue;
        }
        Debug.LogError($"Current Sprite is missing from Sprite Atlas {spriteName}");
        return overworldSpriteNullReference;
    }

    public static void AddSpriteSheetToList(Sprite newSpriteSheet)
    {
        Debug.Log(newSpriteSheet.name);
        if (sprites.ContainsKey(newSpriteSheet.name))
        {
            Debug.Log($"{newSpriteSheet.name} already exists");
            return;
        }

        string spriteSheet = AssetDatabase.GetAssetPath(newSpriteSheet);
        List<Sprite> currentSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToList();
        foreach (Sprite newSprite in currentSprites)
        {
            sprites.Add(newSprite.name, newSprite);
        }
    }

    #endregion
}
