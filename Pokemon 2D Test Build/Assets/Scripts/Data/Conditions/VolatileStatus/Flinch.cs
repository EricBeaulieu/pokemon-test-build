using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flinch : ConditionBase
{
    public override ConditionID Id { get { return ConditionID.Flinch; } }
    public override ConditionBase ReturnDerivedClassAsNew() { return new Flinch(); }
    public override bool OnBeforeMove(Pokemon source, Pokemon target = null)
    {
        source.statusChanges.Enqueue($"{source.currentName} flinched and couldn't move");
        return false;
    }
    public override void OnEndTurn(Pokemon pokemon)
    {
        pokemon.CureVolatileStatus(ConditionID.Flinch);
    }
}
