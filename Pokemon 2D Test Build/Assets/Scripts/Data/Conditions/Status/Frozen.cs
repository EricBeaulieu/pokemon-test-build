using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frozen : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Frozen; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Frozen(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        return $"{pokemon.currentName} has been frozen";
    }
    public override string HasConditionMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} is already Frozen";
    }
    public override bool OnBeforeMove(Pokemon source, Pokemon target = null)
    {
        if (Random.Range(1, 6) == 1)
        {
            source.CureStatus();
            source.statusChanges.Enqueue($"{source.currentName} has thawed out");
            return true;
        }
        source.statusChanges.Enqueue($"{source.currentName} is frozen and cannot move");
        return false;
    }
    public override bool MoveFailedAnimation()
    {
        return true;
    }
}
