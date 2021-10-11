using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tamato : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Tamato; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new Tamato(); }
    public override bool UseInInventory()
    {
        return true;
    }
    EarnableEV removedEv = new EarnableEV(StatAttribute.Speed, 10);
    public override bool UsedInInventoryEffect(Pokemon pokemon)
    {
        return pokemon.RemoveEffortValue(removedEv);
    }
}