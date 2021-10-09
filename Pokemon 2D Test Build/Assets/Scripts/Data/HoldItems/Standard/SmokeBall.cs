using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBall : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SmokeBall; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new SmokeBall(); }
    public override bool FleeWithoutFail()
    {
        return true;
    }
    public override string SpecializedMessage(Pokemon holder, Pokemon opposingPokemon)
    {
        return $"{holder.currentName} fled using its {GlobalTools.SplitCamelCase(HoldItemId.ToString())}";
    }
}
