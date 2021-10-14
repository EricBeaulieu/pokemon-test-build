using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBall : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SmokeBall; } }
    public override bool FleeWithoutFail()
    {
        return true;
    }
    public override string SpecializedMessage(BattleUnit holder, Pokemon opposingPokemon)
    {
        return $"{holder.pokemon.currentName} fled using its {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
}
