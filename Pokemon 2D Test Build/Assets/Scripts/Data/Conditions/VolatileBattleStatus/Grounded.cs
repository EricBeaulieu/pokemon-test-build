using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Grounded; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Grounded(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} was grounded";
    }
}
