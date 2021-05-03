using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpriteAtlas : MonoBehaviour
{
    [SerializeField] Texture2D gen1;
    [SerializeField] Texture2D gen2;
    List<Sprite> pokemonSprites = new List<Sprite>();

    private void Start()
    {
        string spriteSheet = AssetDatabase.GetAssetPath(gen1);
        List<Sprite> kantoSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToList();

        spriteSheet = AssetDatabase.GetAssetPath(gen2);
        List<Sprite> johtoSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToList();

        kantoSprites.ForEach(x => pokemonSprites.Add(x));
        johtoSprites.ForEach(x => pokemonSprites.Add(x));
    }

    public Sprite GetSprite(string spriteName)
    {
        return pokemonSprites.Find(x => x.name == spriteName);
    }

}
