using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelpsy : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Kelpsy; } }
    public override bool UseInInventory()
    {
        return true;
    }
    EarnableEV removedEv = new EarnableEV(StatAttribute.Attack, 10);
    public override bool UsedInInventoryEffect(Pokemon pokemon)
    {
        return pokemon.RemoveEffortValue(removedEv);
    }
}