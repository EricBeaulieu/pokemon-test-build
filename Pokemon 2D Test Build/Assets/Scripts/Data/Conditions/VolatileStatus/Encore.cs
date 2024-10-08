using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encore : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Encore; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Encore(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        StatusTime = 3;
        return $"{pokemon.currentName} received an Encore!";
    }
    public override bool OnEndTurn(Pokemon pokemon)
    {
        StatusTime--;
        if (StatusTime <= 0)
        {
            pokemon.CureVolatileStatus(Id);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName}'s encore ended");
        }
        return base.OnEndTurn(pokemon);
    }
}
