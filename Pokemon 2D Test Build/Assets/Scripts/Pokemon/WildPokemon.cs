using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WildPokemon
{
    [SerializeField] PokemonBase pokemonBase;
    public PokemonBase PokemonBase { get { return pokemonBase; } }
    [SerializeField] int minLevelRange;
    [SerializeField] int maxLevelRange;
    public int Level
    {
        get
        {
            if(maxLevelRange <= minLevelRange)
            {
                return minLevelRange;
            }
            return Random.Range(minLevelRange, maxLevelRange);
        }
    }
    [Range(0, 100)]
    [SerializeField] int wildEncounterChance;
    public int WildEncounterChance { get { return wildEncounterChance; } }

    public ItemBase HoldItemUponBeingFound()
    {
        for (int i = 0; i < pokemonBase.WildPokemonHoldItems.Count; i++)
        {
            if(Random.Range(0,100) <= pokemonBase.WildPokemonHoldItems[i].ItemPercentChance)
            {
                return pokemonBase.WildPokemonHoldItems[i].HoldItem;
            }
        }
        return null;
    }
}

[System.Serializable]
public class WildPokemonHoldItems
{
    [SerializeField] ItemBase holdItem;
    public ItemBase HoldItem { get { return holdItem; } }
    [Range(0, 100)]
    [SerializeField] int itemPercentChance;
    public int ItemPercentChance { get { return itemPercentChance; } }
}
