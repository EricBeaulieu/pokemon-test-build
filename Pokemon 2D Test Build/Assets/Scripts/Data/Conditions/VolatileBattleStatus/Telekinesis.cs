using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Telekinesis; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Telekinesis(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        StatusTime = 3;
        return $"{pokemon.currentName} was hurled into the air";
    }
    public override bool OnEndTurn(Pokemon pokemon)
    {
        StatusTime--;
        if (StatusTime <= 0)
        {
            pokemon.CureVolatileStatus(Id);
            pokemon.statusChanges.Enqueue($"{pokemon.currentName}'s was freed from telekinesis");
        }
        return base.OnEndTurn(pokemon);
    }
}
