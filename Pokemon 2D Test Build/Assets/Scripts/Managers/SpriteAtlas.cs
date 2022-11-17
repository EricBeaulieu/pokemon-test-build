using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpriteAtlas : MonoBehaviour
{
    [Header("Standard Battle Sprites")]
    [SerializeField] Texture2D gen1;
    [SerializeField] Texture2D gen2;
    [SerializeField] Texture2D unown;
    [SerializeField] Texture2D gen3;
    [SerializeField] Texture2D gen4;
    [SerializeField] Texture2D arceus;
    [Header("Standard Battle Sprites")]
    [SerializeField] Texture2D gen1Overworld;
    [SerializeField] Texture2D gen2Overworld;

    [SerializeField] List<Texture2D> specializedSprites;
    public static List<Sprite> pokemonSprites = new List<Sprite>();

    void Awake()
    {
        AddTextureSpritesToList(gen1);
        AddTextureSpritesToList(gen2);
        AddTextureSpritesToList(unown);
        AddTextureSpritesToList(gen3);
        AddTextureSpritesToList(gen4);
        AddTextureSpritesToList(arceus);
        AddTextureSpritesToList(gen1Overworld);
        AddTextureSpritesToList(gen2Overworld);

        foreach (Texture2D texture in specializedSprites)
        {
            AddTextureSpritesToList(texture);
        }
    }

    public static Sprite GetSprite(string spriteName)
    {
        return pokemonSprites.Find(x => x.name == spriteName);
    }

    static void AddTextureSpritesToList(Texture2D texture)
    {
        string spriteSheet = AssetDatabase.GetAssetPath(texture);
        List<Sprite> currentSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToList();
        currentSprites.ForEach(x => pokemonSprites.Add(x));
    }
}
