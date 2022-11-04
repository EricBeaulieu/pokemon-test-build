using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DayTimeSpecific { NA, Day, Night }

[System.Serializable]
public class EvolutionBase
{
    [SerializeField] protected PokemonBase evolvedPokemon;
    [Tooltip("Level Requirement to Level Up")]
    [SerializeField] int levelRequirement = 0;
    [SerializeField] bool maxFriendShipRequired = false;
    [Tooltip("evolution stone required to evolve into specified pokemon")]
    [SerializeField] EvolutionStoneBase evolutionStone;
    [Tooltip("This will evolve the pokemon if theyre holding the specified item upon leveling up")]
    [SerializeField] ItemBase specifiedHoldItem;
    [SerializeField] DayTimeSpecific timeSpecific = DayTimeSpecific.NA;
    [SerializeField] MoveBase specifiedMove;

    public bool CanEvolve(Pokemon pokemon, EvolutionStoneBase currentStone)
    {
        foreach (EvolutionBase evolution in pokemon.pokemonBase.Evolutions)
        {
            if (evolutionStone == currentStone)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanEvolve(Pokemon pokemon)
    {
        if(evolvedPokemon == null)
        {
            Debug.Log($"{pokemon.pokemonBase.name} pokemon base has a null reference for evolved pokemon");
            return false;
        }

        if (maxFriendShipRequired == true)
        {
            //If max friendship then go into
        }

        if(specifiedHoldItem != null)
        {
            if(levelRequirement > 0)
            {
                if(specifiedHoldItem == pokemon.GetCurrentItem)
                {
                    return (pokemon.currentLevel >= levelRequirement);
                }
                return false;
            }
            else
            {
                if (specifiedHoldItem == pokemon.GetCurrentItem)
                {
                    return true;
                }
            }
            return false;
        }

        if(levelRequirement > 0)
        {
            return (pokemon.currentLevel >= levelRequirement);
        }
        return false;
    }

    public PokemonBase NewPokemonEvolution(Pokemon evolvingPokemon) 
    {
        if(PokemonNameList.PokemonHasUniqueEvolution(evolvingPokemon.pokemonBase.GetPokedexNumber()))
        {
            return PokemonNameList.ReturnUniqueEvolution(evolvingPokemon);
        }
        return evolvedPokemon; 
    }
    public EvolutionStoneBase RequiredStone
    {
        get { return evolutionStone; }
    }
}
