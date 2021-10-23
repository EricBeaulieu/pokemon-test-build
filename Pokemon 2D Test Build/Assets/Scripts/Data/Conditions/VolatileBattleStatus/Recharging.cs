using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recharging : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Recharging; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Recharging(); }
    public override bool OnBeforeMove(Pokemon source, Pokemon target = null)
    {
        source.statusChanges.Enqueue($"{source.currentName} is recharging");
        source.CureVolatileStatus(ConditionID.Recharging);
        return false;
    }
}
