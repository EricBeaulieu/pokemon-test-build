using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakingAim : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.TakingAim; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new TakingAim(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{attackingPokemon.currentName} took aim at {pokemon.currentName}";
    }
}
