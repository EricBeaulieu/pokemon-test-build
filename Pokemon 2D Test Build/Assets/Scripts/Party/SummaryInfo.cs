using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryInfo : SummaryUIBase
{
    [SerializeField] Text pokedexNumber;
    [SerializeField] Text pokedexPokemonName;
    [SerializeField] Image pokemonType1;
    [SerializeField] Image pokemonType2;
    [SerializeField] Text originalTrainerName;
    [SerializeField] Text originalTrainerIDNumber;
    [SerializeField] Text currentHeldItem;
    [SerializeField] Text trainerMemo;

    public override float offsetXPosDifference()
    {
        return GetComponent<RectTransform>().sizeDelta.x;
    }

    public override void SetupData(Pokemon pokemon)
    {
        pokedexNumber.text = pokemon.pokemonBase.GetPokedexNumber().ToString("000");
        pokedexPokemonName.text = pokemon.pokemonBase.GetPokedexName();
        pokemonType1.sprite = StatusConditionArt.instance.ReturnElementArt(pokemon.pokemonBase.pokemonType1);
        if (pokemon.pokemonBase.pokemonType2 != ElementType.NA)
        {
            pokemonType2.sprite = StatusConditionArt.instance.ReturnElementArt(pokemon.pokemonBase.pokemonType2);
        }
        else
        {
            pokemonType2.sprite = StatusConditionArt.instance.Nothing;
        }
        originalTrainerName.text = pokemon.originalTrainer;
        originalTrainerIDNumber.text = pokemon.originalTrainerID;
        currentHeldItem.text = "NONE";
        trainerMemo.text = $"{pokemon.nature.natureName} Nature";
    }
}