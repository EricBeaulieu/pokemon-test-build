using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pomeg : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Pomeg; } }
    public override bool UseInInventory()
    {
        return true;
    }
    EarnableEV removedEv = new EarnableEV(StatAttribute.HitPoints, 10);
    public override bool UsedInInventoryEffect(Pokemon pokemon)
    {
        return pokemon.RemoveEffortValue(removedEv);
    }
}