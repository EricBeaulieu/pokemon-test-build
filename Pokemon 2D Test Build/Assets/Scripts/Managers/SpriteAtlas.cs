using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpriteAtlas : MonoBehaviour
{
    [SerializeField] Texture2D gen1;
    List<Sprite> pokemonSprites;

    private void Start()
    {
        string spriteSheet = AssetDatabase.GetAssetPath(gen1);
        pokemonSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToList();
    }

    public Sprite GetSprite(string spriteName)
    {
        return pokemonSprites.Find(x => x.name == spriteName);
    }

}
