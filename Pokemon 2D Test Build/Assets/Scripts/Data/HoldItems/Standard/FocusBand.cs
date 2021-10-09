using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusBand : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.FocusBand; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new FocusBand(); }
    public override bool EndureOHKOAttack(Pokemon defendingPokemon)
    {
        if(Random.value <= 0.1f)
        {
            return true;
        }

        return false;
    }
}
