using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hondew : HoldItemBase
{
    public override BerryID BerryId { get { return BerryID.Hondew; } }
    public override bool UseInInventory()
    {
        return true;
    }
    EarnableEV removedEv = new EarnableEV(StatAttribute.SpecialAttack, 10);
    public override bool UsedInInventoryEffect(Pokemon pokemon)
    {
        return pokemon.RemoveEffortValue(removedEv);
    }
}