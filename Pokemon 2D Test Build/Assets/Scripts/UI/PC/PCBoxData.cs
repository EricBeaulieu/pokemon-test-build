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
    }

    public PCBoxData(PCBoxSaveData currentBoxData)
    {
        boxName = currentBoxData.boxName;
        pokemonInsideBox = new Pokemon[PC_MAX_BOX_SIZE];
        for (int i = 0; i < pokemonInsideBox.Length; i++)
        {
            if(currentBoxData.currentPokemon[i] != null)
            {
                pokemonInsideBox[i] = new Pokemon(currentBoxData.currentPokemon[i]);
            }
        }
    }
    //load all pokemon into boxes upon start of game, if pokemon is null just leave empty space

    public Pokemon[] PokemonInsideBox
    {
        get { return pokemonInsideBox; }
    }

    public void SavePokemonInsideBox(PCCurrentBoxInfo currentBox)
    {
        for (int i = 0; i < PC_MAX_BOX_SIZE; i++)
        {
            pokemonInsideBox[i] = currentBox.GetPCPokemonAtIndex(i).currentPokemon;
        }
    }

    public PCBoxSaveData GetSaveData()
    {
        PCBoxSaveData pcSaveData = new PCBoxSaveData();

        pcSaveData.boxName = boxName;
        pcSaveData.currentPokemon = new PokemonSaveData[PC_MAX_BOX_SIZE];
        for (int i = 0; i < pcSaveData.currentPokemon.Length; i++)
        {
            if(pokemonInsideBox[i] != null)
            {
                pcSaveData.currentPokemon[i] = pokemonInsideBox[i].GetSaveData();
            }
        }
        return pcSaveData;
    }

    public void SetBoxesWithPresetPokemon()
    {
        if (GameManager.instance.startWithPresetBoxes == true && GameManager.instance.startNewSaveEveryStart == true)
        {
            for (int i = 0; i < GameManager.instance.GetTestPokemon().Length; i++)
            {
                if (GameManager.instance.GetTestPokemon()[i] == null)
                {
                    pokemonInsideBox[i] = null;
                }
                else
                {
                    pokemonInsideBox[i] = GameManager.instance.GetTestPokemon()[i];
                }
            }
        }
    }

    public void DepositPokemonAtIndex(Pokemon pokemon,int index)
    {
        pokemonInsideBox[index] = pokemon;
    }
}
