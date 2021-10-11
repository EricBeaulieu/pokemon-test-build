using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qualot : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Qualot; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Qualot(); }
    public override bool UseInInventory()
    {
        return true;
    }
    EarnableEV removedEv = new EarnableEV(StatAttribute.Defense, 10);
    public override bool UsedInInventoryEffect(Pokemon pokemon)
    {
        return pokemon.RemoveEffortValue(removedEv);
    }
}