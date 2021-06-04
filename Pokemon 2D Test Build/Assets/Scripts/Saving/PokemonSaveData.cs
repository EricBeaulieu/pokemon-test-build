using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PokemonSaveData
{
    static string split = "/";
    static string pokemonData;
    static string empty = "_";

    public static void SavePokemon(Pokemon pokemon,string location)
    {
        ResetPokemonData();
        AddData(location);

        AddData(pokemon.pokemonBase.GetPokedexNumber().ToString());

        AddData(pokemon.currentExp.ToString());

        for (int i = 0; i < PokemonBase.MAX_NUMBER_OF_MOVES; i++)
        {
            if(pokemon.moves[i] != null)
            {
                AddData(pokemon.moves[i].moveBase.GetInstanceID().ToString());
                AddData(pokemon.moves[i].pP.ToString());
            }
            else
            {
                AddData(empty);//move
                AddData(empty);//PP
            }
        }

        AddData(pokemon.isShiny.ToString());

        AddData(pokemon.gender.ToString());

        AddData(pokemon.nature.ToString());

        for (int i = 0; i < IndividualValues.VALUES_SAVED_LENGTH; i++)
        {
            AddData(pokemon.individualValues.ReturnValueAtIndex(i).ToString());
        }

        //for (int i = 0; i < length; i++)
        //{

        //}
    }
    
    public static Pokemon LoadPokemon(string Location)
    {

        return null;
    }

    static void ResetPokemonData()
    {
        pokemonData = "";
    }

    static void AddData(string data)
    {
        pokemonData += data;
        pokemonData += split;
    }

    ////Split
    //string n_s = "John_Smith";
    //string name = "";

    //void Start()
    //{

    //    //Split
    //    string[] splitArray = n_s.Split(char.Parse("_"));
    //    name = splitArray[0];
    //}
}
