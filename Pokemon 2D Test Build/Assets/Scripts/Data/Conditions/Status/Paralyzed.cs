using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralyzed : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Paralyzed; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Paralyzed(); }
    public override string StartMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} has been paralyzed";
    }
    public override string HasConditionMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} is already paralyzed";
    }
    public override float StatEffectedByCondition(StatAttribute statAttribute)
    {
        if (statAttribute == StatAttribute.Speed)
        {
            return 0.5f;
        }
        return 1;
    }
    public override bool OnBeforeMove(Pokemon source, Pokemon target = null)
    {
        if (Random.Range(1, 5) == 1)
        {
            source.statusChanges.Enqueue($"{source.currentName} is paralyzed and cannot move");
            return false;
        }
        return true;
    }
}
