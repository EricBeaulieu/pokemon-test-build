using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DayTimeSpecific { NA, Day, Night }
public enum GenderSpecific { NA, Male, Female }
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
    [SerializeField] GenderSpecific genderSpecific = GenderSpecific.NA;
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
        if(evolvedPokemon == null)//This has been changed to go through everything and if it hits the end then always put true
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
            if(specifiedHoldItem != pokemon.GetCurrentItem)
            {
                return false;
            }
        }

        if(pokemon.currentLevel < levelRequirement)
        {
            return false;
        }

        if(genderSpecific != GenderSpecific.NA)
        {
            if(pokemon.gender.HasValue == false)
            {
                return false;
            }

            if(genderSpecific == GenderSpecific.Male)
            {
                if(pokemon.gender.Value == false)//is female
                {
                    return false;
                }
            }
            else //is it female required
            {
                if (pokemon.gender.Value == true)
                {
                    return false;
                }
            }
        }

        if(specifiedMove != null)
        {
            if (pokemon.moves.Exists(x => x.moveBase == specifiedMove) == false)
            {
                return false;
            }
        }
        return true;
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
