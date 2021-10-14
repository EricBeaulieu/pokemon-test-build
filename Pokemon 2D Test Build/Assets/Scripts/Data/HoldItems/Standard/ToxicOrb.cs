using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicOrb : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.ToxicOrb; } }
    public override ConditionID InflictConditionAtTurnEnd()
    {
        return ConditionID.ToxicPoison;
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} was badly poisned by {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
}
