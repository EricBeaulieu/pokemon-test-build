using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonOverworldSpriteSheetSwap : MonoBehaviour
{
    Dictionary<string, Sprite> _spriteSheet;
    SpriteRenderer _spriteRenderer;
    string nameReference;
    [SerializeField] List<Sprite> test;

    void Start()
    {
        PokemonBase basePokemon = GetComponentInParent<WildPokemonController>().pokemon.PokemonBase;
        nameReference = $"{basePokemon.GetPokedexNumber().ToString("000")}_{basePokemon.GetPokedexName()}_";

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteSheet = LoadNewSpriteSheet(nameReference);
    }

    private void LateUpdate()
    {
        _spriteRenderer.sprite = _spriteSheet[_spriteRenderer.sprite.name];
    }

    Dictionary<string, Sprite> LoadNewSpriteSheet(string nameReference)
    {
        Dictionary<string, Sprite> newSheet = new Dictionary<string, Sprite>();

        string temp = $"{nameReference}Up0";
        newSheet.Add("Up0", SpriteAtlas.GetSprite(temp));

        temp = $"{nameReference}Up1";
        newSheet.Add("Up1", SpriteAtlas.GetSprite(temp));

        temp = $"{nameReference}Down0";
        newSheet.Add("Down0", SpriteAtlas.GetSprite(temp));

        temp = $"{nameReference}Down1";
        newSheet.Add("Down1", SpriteAtlas.GetSprite(temp));

        temp = $"{nameReference}Left0";
        newSheet.Add("Left0", SpriteAtlas.GetSprite(temp));

        temp = $"{nameReference}Left1";
        newSheet.Add("Left1", SpriteAtlas.GetSprite(temp));

        temp = $"{nameReference}Right0";
        newSheet.Add("Right0", SpriteAtlas.GetSprite(temp));

        temp = $"{nameReference}Right1";
        newSheet.Add("Right1", SpriteAtlas.GetSprite(temp));

        return newSheet;
    }
}
