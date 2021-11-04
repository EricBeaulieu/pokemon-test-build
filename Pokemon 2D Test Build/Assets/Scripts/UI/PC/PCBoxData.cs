using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBoxData
{
    public string boxName { get; set; }
    Pokemon[] pokemonInsideBox;

    public const int PC_MAX_BOX_SIZE = 30;

    public PCBoxData()
    {
        pokemonInsideBox = new Pokemon[PC_MAX_BOX_SIZE];
        for (int i = 0; i < pokemonInsideBox.Length; i++)
        {
            pokemonInsideBox[i] = GameManager.instance.GetTestPokemon();
        }
    }
    //load all pokemon into boxes upon start of game, if pokemon is null just leave empty space

    public Pokemon[] PokemonInsideBox
    {
        get { return pokemonInsideBox; }
    }
}
