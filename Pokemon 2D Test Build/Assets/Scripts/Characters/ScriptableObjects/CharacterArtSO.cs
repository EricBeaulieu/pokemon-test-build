using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/ArtDataEntry")]
public class CharacterArtSO : ScriptableObject
{
    [SerializeField] Sprite[] frontBattleSprite;
    [SerializeField] Sprite[] backBattleSprite;
    [SerializeField] Sprite overworldSpriteSheet;

    public Sprite[] GetFrontBattleSprite
    {
        get { return frontBattleSprite; }
    }

    public Sprite[] GetBackBattleSprite
    {
        get { return backBattleSprite; }
    }

    public Sprite GetOverworldSpriteSheet
    {
        get { return overworldSpriteSheet; }
    }
}
