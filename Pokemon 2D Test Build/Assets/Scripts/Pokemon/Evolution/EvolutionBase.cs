using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum DayTimeSpecific { NA, Day, Night }

[System.Serializable]
public abstract class EvolutionBase
{
    [SerializeField] protected PokemonBase evolvedPokemon;
    //[SerializeField] DayTimeSpecific timeSpecific;

    public abstract bool CanEvolve(Pokemon pokemon,ItemBase currentHeldItem);

    public PokemonBase NewPokemonEvolution() { return evolvedPokemon; }
}
