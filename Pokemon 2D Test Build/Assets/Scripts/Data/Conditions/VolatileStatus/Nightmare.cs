using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nightmare : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Nightmare; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Nightmare(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} began having a Nightmare";
    }
    public override void OnEndTurn(Pokemon pokemon)
    {
        if(pokemon.status.Id != ConditionID.Sleep)
        {
            pokemon.CureVolatileStatus(Id);
            return;
        }

        pokemon.UpdateHPDamage(Mathf.CeilToInt((float)pokemon.maxHitPoints / 4f));
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} is locked in a nightmare");
    }
    public override bool RequiredConditionToWork(ConditionID iD)
    {
        return (iD == ConditionID.Sleep);
    }
}
