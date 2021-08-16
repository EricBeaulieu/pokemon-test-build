using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameOrb : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.FlameOrb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new FlameOrb(); }
    public override ConditionID InflictConditionAtTurnEnd()
    {
        return ConditionID.Burn;
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} was burnt by {GlobalTools.SplitCamelCase(Id.ToString())}";
    }
}
