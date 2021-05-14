using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infatuation : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Infatuation; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Infatuation(); }
    public override string StartMessage(Pokemon pokemon)
    {
        return $"{pokemon.currentName} fell in love!";
    }
    public override bool OnBeforeMove(Pokemon source, Pokemon target)
    {
        source.statusChanges.Enqueue($"{source.currentName} is in love with {target.currentName}");

        if (Random.value > 0.5f)
        {
            return true;
        }

        source.statusChanges.Enqueue($"{source.currentName} is immobilized by love ");
        return false;
    }
}
