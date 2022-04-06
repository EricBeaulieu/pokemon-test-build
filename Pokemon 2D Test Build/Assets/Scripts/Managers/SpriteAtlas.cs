using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpriteAtlas : MonoBehaviour
{
    [SerializeField] Texture2D gen1;
    [SerializeField] Texture2D gen2;

    [SerializeField] List<Texture2D> specializedSprites;
    static List<Sprite> pokemonSprites = new List<Sprite>();

    void Start()
    {
        string spriteSheet = AssetDatabase.GetAssetPath(gen1);
        List<Sprite> kantoSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToList();
        kantoSprites.ForEach(x => pokemonSprites.Add(x));

        spriteSheet = AssetDatabase.GetAssetPath(gen2);
        List<Sprite> johtoSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToList();
        johtoSprites.ForEach(x => pokemonSprites.Add(x));

        foreach (Texture2D texture in specializedSprites)
        {
            spriteSheet = AssetDatabase.GetAssetPath(texture);
            List<Sprite> newSheet = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToList();
            newSheet.ForEach(x => pokemonSprites.Add(x));
        }
    }

    public static Sprite GetSprite(string spriteName)
    {
        return pokemonSprites.Find(x => x.name == spriteName);
    }
}
