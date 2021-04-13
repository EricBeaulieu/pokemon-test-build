using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryInfo : MonoBehaviour
{
    [SerializeField] Text pokedexNumber;
    [SerializeField] Text pokedexPokemonName;
    [SerializeField] Image pokemonType1;
    [SerializeField] Image pokemonType2;
    [SerializeField] Text originalTrainerName;
    [SerializeField] Text originalTrainerIDNumber;
    [SerializeField] Text currentHeldItem;
    [SerializeField] Text trainerMemo;

    public void SetupData(Pokemon pokemon)
    {
        pokedexNumber.text = pokemon.pokemonBase.GetPokedexNumber().ToString("000");
    }
}
