using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Sleep; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Sleep(); }
    public override string StartMessage(Pokemon pokemon, Pokemon attackingPokemon)
    {
        StatusTime = Random.Range(1, 4);
        if(pokemon.ability.HalfDurationOfSleep() == true)
        {
            StatusTime = Mathf.FloorToInt(StatusTime / 2);
        }
        return $"{pokemon.currentName} has fallen asleep";
    }
    public override string HasConditionMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} is already Asleep";
    }
    public override bool OnBeforeMove(Pokemon source, Pokemon target = null)
    {
        if (StatusTime <= 0)
        {
            source.CureStatus();
            source.statusChanges.Enqueue($"{source.currentName} has woken up");
            return true;
        }

        StatusTime--;
        source.statusChanges.Enqueue($"{source.currentName} is sleeping");
        return false;
    }
    public override bool MoveFailedAnimation()
    {
        return true;
    }
}
