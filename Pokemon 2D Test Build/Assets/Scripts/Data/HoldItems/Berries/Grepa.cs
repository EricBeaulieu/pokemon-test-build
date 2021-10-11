using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grepa : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Grepa; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Grepa(); }
    public override bool UseInInventory()
    {
        return true;
    }
    EarnableEV removedEv = new EarnableEV(StatAttribute.SpecialDefense, 10);
    public override bool UsedInInventoryEffect(Pokemon pokemon)
    {
        return pokemon.RemoveEffortValue(removedEv);
    }
}