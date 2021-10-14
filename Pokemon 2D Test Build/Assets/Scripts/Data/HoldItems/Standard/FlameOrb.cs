using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameOrb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.FlameOrb; } }
    public override ConditionID InflictConditionAtTurnEnd()
    {
        return ConditionID.Burn;
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} was burnt by {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
}
