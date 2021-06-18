using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embargo : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Embargo; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Embargo(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        StatusTime = 5;
        return $"{pokemon.currentName} cant use items anymore";
    }
    public override bool OnEndTurn(Pokemon pokemon)
    {
        StatusTime--;

        if (StatusTime <= 0)
        {
            pokemon.statusChanges.Enqueue($"{pokemon.currentName} can use items");
            pokemon.CureVolatileStatus(Id);
        }
        return base.OnEndTurn(pokemon);
    }
    public override bool PreventsItemUse() { return true; }
}
