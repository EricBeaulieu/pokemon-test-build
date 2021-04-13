using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryDisplay : MonoBehaviour
{
    [SerializeField] Text pokemonLevel;
    [SerializeField] Text currentName;
    Sprite[] _pokemonSpriteAnimations;
    [SerializeField] Image pokemonSprite;
    [SerializeField] Image gender;
    [SerializeField] Image pokeballSprite;

    public void SetupData(Pokemon pokemon)
    {
        pokemonLevel.text = $"Lv{pokemon.currentLevel.ToString()}";
        currentName.text = $"{pokemon.currentName}";
        _pokemonSpriteAnimations = pokemon.pokemonBase.GetFrontSprite(pokemon.isShiny, pokemon.gender);
        gender.sprite = StatusConditionArt.instance.ReturnGenderArt(pokemon.gender);
        //pokeballSprite.sprite =
    }
}
